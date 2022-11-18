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

            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
        }

        //GETTING THE TEAMS USER OWNED
        public async Task<IEnumerable<Team>> GetOwnedTeams (string userId, Category category, int offset, int size)
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
                .Where(t=>t.OwnerId== userId)
                .Include(x => x.Address)
                .Skip(offset)
                .Take(size)
                .ToListAsync();
        }

        //GETTING THE COUNT OF OWNED TEAMS
        public async Task<int> GetOwnedTeamCountAsync(string userId)
        {
            return await _context.Teams.Where(t=>t.OwnerId == userId).CountAsync();
        }

        //GETTING THE COUNT BY CATEGORY OF USER THAT OWN THEM
        public async Task<int> GetCountByCategoryOfUserOwnedAsync(string userId, Category category)
        {
            return await _context.Teams
                .Where(t => t.OwnerId == userId)
                .CountAsync(t => t.Category == category);
        }

        //GET TEAM BY ID
        public async Task<Team?> GetByIdAsync(int id)
        {
           return await _context.Teams
                .Include(x=>x.Owner)
                .Include(x=>x.Address)
                .FirstOrDefaultAsync(x=>x.Id==id);

             
        }
    }
}
