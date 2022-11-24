using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Models.BindingModels.Team;
using ESportsTeams.Core.Models.ViewModels.TeamViewModels;
using ESportsTeams.Infrastructure.Data;
using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;


        public TeamService(ApplicationDbContext context, IPhotoService photoService, IUserService userService)
        {
            _context = context;
            _photoService = photoService;
            _userService = userService;
        }

        //Adding TEAMS
        public async Task AddTeamAsync(AddTeamViewModel model, string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new ArgumentException("Invalid User!");
            }

            var result = await _photoService.AddPhotoAsync(model.Image);
            var team = new Team()
            {
                Name = model.Name,
                Description = model.Description,
                Image = result.Url.ToString(),
                Category = model.Category,
                OwnerId = userId,
                Address = new Address()
                {
                    Street = model.Address.Street,
                    City = model.Address.City,
                    Country = model.Address.Country,
                }
            };

            team.AppUsers.Add(user);

            user.TeamId = team.Id;
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
        }

        //GETTING THE TEAMS USER OWNED
        public async Task<IEnumerable<Team>> GetOwnedTeams(string userId, Category category, int offset, int size)
        {
            return await _userService.GetUserOwnedTeamsAsync(userId, category, offset, size);
        }

        //GETTING COUNT OF ALL TEAMS
        public async Task<int> GetCountAsync()
        {
            return await _context.Teams.CountAsync();
        }

        //GETTING COUNT OF ALL TEAMS IN CATEGORY
        public async Task<int> GetCountByCategoryAsync(Category category)
        {
            return await _context.Teams.CountAsync(t => t.Category == category);
        }


        //SLICE TEAM FOR PAGES
        public async Task<IEnumerable<Team>> GetSliceAsync(int offset, int size)
        {
            return await _context.Teams.Include(x => x.Address).Skip(offset).Take(size).ToListAsync();
        }

        //GETTING TEAMS BY COUNTRY
        public async Task<IEnumerable<Team>> GetTeamByCountry(string country)
        {
            return await _context.Teams.Where(t => t.Address.Country.Contains(country)).Distinct().ToListAsync();
        }

        //GETTING ALL TEAMS IN CATEGORY AND SLICE
        public async Task<IEnumerable<Team>> GetTeamsByCategoryAndSliceAsync(Category category, int offset, int size)
        {
            return await _context.Teams
                .Include(x => x.Address)
                .Where(x => x.Category == category)
                .Skip(offset)
                .Take(size)
                .ToListAsync();
        }

        //GETTING ALL TEAMS IN USER THAT OWN THEM AND SLICE
        public async Task<IEnumerable<Team>> GetSliceOfUserOwnedAsync(string userId, int offset, int size)
        {
            return await _context.Teams
                .Where(t => t.OwnerId == userId)
                .Include(x => x.Address)
                .Skip(offset)
                .Take(size)
                .ToListAsync();
        }

        //GETTING THE COUNT OF OWNED TEAMS
        public async Task<int> GetOwnedTeamCountAsync(string userId)
        {
            return await _context.Teams.Where(t => t.OwnerId == userId).CountAsync();
        }

        //GETTING THE COUNT BY CATEGORY OF USER THAT OWN THEM
        public async Task<int> GetCountByCategoryOfUserOwnedAsync(string userId, Category category)
        {
            return await _context.Teams
                .Where(t => t.OwnerId == userId)
                .CountAsync(t => t.Category == category);
        }


        public async Task<DetailsTeamViewModel?> GetTeamDetailsByIdAsync(int id, string loggedUserId)
        {
            var result = await _context.Teams
                .Include(x => x.Owner)
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                throw new ArgumentException("Team not found!");
            }

            var finalResult = new DetailsTeamViewModel()
            {
                Id = result.Id,
                loggedUserId = loggedUserId,
                Name = result.Name,
                Description = result.Description,
                Category = result.Category,
                Image = result.Image,
                AddressId = result.AddressId,
                Address = result.Address,
                TournamentWin = result.TournamentWin,
                OwnerId = result.OwnerId,
                Owner = result.Owner,
                TeamTournaments = result.TeamTournaments,
                AvarageMMR = result.AvarageMMR,
            };

            return finalResult;
        }

        public async Task<Team?> GetTeamByIdAsync(int id)
        {
            return await _context.Teams
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task EditTeamAsync(EditTeamViewModel model)
        {


            var teamToChange = await _context.Teams
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (teamToChange == null)
            {
                throw new ArgumentException("Team not found!");
            }

            var photoResult = await _photoService.AddPhotoAsync(model.Image);

            if (!string.IsNullOrEmpty(teamToChange.Image))
            {
                _ = _photoService.DeletePhotoAsync(teamToChange.Image);
            }
            teamToChange.Name = model.Name;
            teamToChange.Description = model.Description;
            teamToChange.Image = photoResult.Url.ToString();
            teamToChange.Category = model.Category;
            teamToChange.AddressId = model.AddressId;
            teamToChange.Address = model.Address;

            await _context.SaveChangesAsync();
        }

        //public async Task<bool> DeleteTeamAsync(int id)
        //{
        //    var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);
        //    if (team == null)
        //    {
        //        throw new ArgumentException("Team not found!");
        //    }
        //    if (!string.IsNullOrEmpty(team.Image))
        //    {
        //        _ = _photoService.DeletePhotoAsync(team.Image);
        //    }
        //    _context.Teams.Remove(team);

        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        public async Task<bool> TeamExistsAsync(string name)
        {
            var teams = await _context.Teams.ToListAsync();

            if (teams == null || teams.Count == 0)
            {
                return false;
            }

            return teams.Any(x => x.Name == name);
        }
    }
}
