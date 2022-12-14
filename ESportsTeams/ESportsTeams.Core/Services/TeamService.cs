using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Models.BindingModels.Team;
using ESportsTeams.Core.Models.ViewModels.TeamViewModels;
using ESportsTeams.Infrastructure.Data;
using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using static ESportsTeams.Infrastructure.Data.Common.CommonConstants;
using Microsoft.EntityFrameworkCore.Storage;
using CloudinaryDotNet.Actions;
using static System.Net.WebRequestMethods;
using ESportsTeams.Infrastructure.Data.Helpers;

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
        public async Task AddTeamAsync(AddTeamBindingModel model, string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new ArgumentNullException(InvalidUser);
            }
            if (user.OwnedTeams != null)
            {
                if (user.OwnedTeams.Any(t => t.Category == model.Category))
                {
                    throw new ArgumentNullException(InvalidTeamCategory);
                }
            }

            ImageUploadResult photoResult = null;
            if (model.Image != null)
            {
                photoResult = await _photoService.AddPhotoAsync(model.Image);

            }

            //var result = await _photoService.AddPhotoAsync(model.Image);
            var team = new Team()
            {
                Name = Html_String_Utility.EncodeProperty(model.Name),
                Description = Html_String_Utility.EncodeProperty(model.Description),
                Image = photoResult?.Url.ToString(),
                Category = model.Category,
                OwnerId = userId,
                Address = new Address()
                {
                    Street = model.Address.Street,
                    City = model.Address.City,
                    Country = model.Address.Country,
                }
            };
            if (team.Image == null)
            {
                team.Image = "https://res.cloudinary.com/dzac3ggur/image/upload/v1670776928/Question_mark_lcqxho.png";
            }

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
            return await _context.Teams
                .Where(t => t.IsBanned == false)
                .CountAsync(t => t.Category == category);
        }


        //SLICE TEAM FOR PAGES
        public async Task<IEnumerable<Team>> GetSliceAsync(int offset, int size)
        {
            return await _context.Teams
                .Where(t => t.IsBanned == false)
                .Include(x => x.Address).Skip(offset).Take(size).ToListAsync();
        }

        //GETTING TEAMS BY COUNTRY
      

        //GETTING ALL TEAMS IN CATEGORY AND SLICE
        public async Task<IEnumerable<Team>> GetTeamsByCategoryAndSliceAsync(Category category, int offset, int size)
        {
            return await _context.Teams
                .Include(x => x.Address)
                .Where(x => x.Category == category && x.IsBanned == false)
                .Skip(offset)
                .Take(size)
                .ToListAsync();
        }

        //GETTING ALL TEAMS IN USER THAT OWN THEM AND SLICE
        public async Task<IEnumerable<Team>> GetSliceOfUserOwnedAsync(string userId, int offset, int size)
        {
            return await _context.Teams
                .Where(t => t.OwnerId == userId && t.IsBanned == false)
                .Include(x => x.Address)
                .Skip(offset)
                .Take(size)
                .ToListAsync();
        }

        //GETTING THE COUNT OF OWNED TEAMS
        public async Task<int> GetOwnedTeamCountAsync(string userId)
        {
            return await _context.Teams.Where(t => t.OwnerId == userId && t.IsBanned == false).CountAsync();
        }

        //GETTING THE COUNT BY CATEGORY OF USER THAT OWN THEM
        public async Task<int> GetCountByCategoryOfUserOwnedAsync(string userId, Category category)
        {
            return await _context.Teams
                .Where(t => t.OwnerId == userId && t.IsBanned == false)
                .CountAsync(t => t.Category == category);
        }


        public async Task<DetailsTeamViewModel?> GetTeamDetailsByIdAsync(int id, string loggedUserId)
        {
            var result = await _context.Teams
                .Where(t => t.IsBanned == false)
                .Include(x => x.Requests)
                .Include(x => x.AppUsers)
                .Include(x => x.Owner)
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (result == null)
            {
                throw new ArgumentNullException(TeamNotFound);
            }
            int avrMMR = 0;
            var listOfUsers = result.AppUsers.ToList();

            if (result.Category == Category.Dota2)
            {
                avrMMR = (int)result.AppUsers.Average(x => x.Dota2MMR ?? 0);
            }
            else if (result.Category == Category.CSGO)
            {
                avrMMR = (int)result.AppUsers.Average(x => x.CSGOMMR ?? 0);
            }
            else if (result.Category == Category.PUBG)
            {
                avrMMR = (int)result.AppUsers.Average(x => x.PUBGMMR ?? 0);
            }
            else if (result.Category == Category.LeagueOfLegends)
            {
                avrMMR = (int)result.AppUsers.Average(x => x.LeagueOfLegendsMMR ?? 0);
            }
            else if (result.Category == Category.VALORANT)
            {
                avrMMR = (int)result.AppUsers.Average(x => x.VALORANTMMR ?? 0);
            }
            var finalResult = new DetailsTeamViewModel()
            {
                Id = result.Id,
                loggedUserId = loggedUserId,
                Name = Html_String_Utility.DecodeProperty(result.Name),
                Description = Html_String_Utility.DecodeProperty(result.Description),
                Category = result.Category,
                Image = result.Image,
                AddressId = result.AddressId,
                Address = result.Address,
                TournamentWin = result.TournamentWin,
                OwnerId = result.OwnerId,
                Owner = result.Owner,
                TeamTournaments = result.TeamTournaments,
                AvarageMMR = avrMMR,
                Requests = result.Requests,
                Players = result.AppUsers

            };
            foreach (var request in result.Requests)
            {
                var user = _userService.GetUserByID(request.RequesterId).Result;
                finalResult.RequestersNames.Add(request.RequesterId, user.Username);
            }
            return finalResult;
        }

        public async Task<Team?> GetTeamByIdAsync(int id)
        {
            return await _context.Teams
                .Where(t => t.IsBanned == false)
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task EditTeamAsync(EditTeamBindingModel model)
        {


            var teamToChange = await _context.Teams
                .Where(t => t.IsBanned == false)
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (teamToChange == null)
            {
                throw new ArgumentNullException(TeamNotFound);
            }
            ImageUploadResult photoResult = null;
            if (model.Image !=null)
            {
                 photoResult = await _photoService.AddPhotoAsync(model.Image);
                if (!string.IsNullOrEmpty(teamToChange.Image))
                {
                    _ = _photoService.DeletePhotoAsync(teamToChange.Image);
                }
            }

           
            teamToChange.Name = Html_String_Utility.EncodeProperty(model.Name);
            teamToChange.Description = Html_String_Utility.EncodeProperty(model.Description);
            if (photoResult != null)
            {
                teamToChange.Image = photoResult.Url.ToString();
            }
            teamToChange.Category = model.Category;
            teamToChange.AddressId = model.AddressId;
            teamToChange.Address = model.Address;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> TeamExistsAsync(string name)
        {
            var teams = await _context.Teams
                .Where(t => t.IsBanned == false)
                .ToListAsync();

            var result = teams.Any(x => x.Name == name);

            if (result)
            {
                return result;
            }

            return result;
        }

        public async Task<IEnumerable<GetTeamsViewModel>> GetAllTeamsAsync()
        {
            var teams = await _context.Teams
                .Include(x => x.TeamTournaments)
                .Include(x => x.Owner)
                .ToListAsync();

            return teams
                .Select(t => new GetTeamsViewModel()
                {
                    Id = t.Id,
                    Name = Html_String_Utility.DecodeProperty(t.Name),
                    Description = Html_String_Utility.DecodeProperty(t.Description),
                    Category = t.Category,
                    Image = t.Image,
                    Address = t.Address,
                    TournamentWin = t.TournamentWin,
                    Owner = t.Owner,
                    TeamTournaments = t.TeamTournaments,
                    AvarageMMR = t.AvarageMMR,
                    IsBanned = t.IsBanned,
                });
        }

        public async Task JoinTeam(string userId, int teamId)
        {
            var team = await _context.Teams
                .FirstOrDefaultAsync(x => x.Id == teamId);
            if (team != null)
            {
                Request request = new Request()
                {
                    Status = RequestStatus.Pending,
                    Team = team,
                    RequesterId = userId,
                };
                team.Requests.Add(request);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ApproveUser(int reqId)
        {
            var req = await _context.Requests
                .Include(r => r.Team)
                .FirstOrDefaultAsync(x => x.Id == reqId);

            if (req == null)
            {
                throw new ArgumentNullException(RequestNotFound);
            }
            var user = await _userService.FindUserByIdAsync(req.RequesterId);

            if (user == null)
            {
                throw new ArgumentNullException(UserNotFound);
            }
            var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == req.TeamId);

            if (team == null)
            {
                throw new ArgumentNullException(TeamNotFound);
            }
            if (!team.AppUsers.Contains(user))
            {
                team.AppUsers.Add(user);
                user.TeamId = team.Id;
            }
            req.Status = RequestStatus.Accepted;
            await _context.SaveChangesAsync();
        }

        public async Task DeclineUser(int reqId)
        {
            var req = await _context.Requests
                .Include(r => r.Team)
                .FirstOrDefaultAsync(x => x.Id == reqId);

            if (req == null)
            {
                throw new ArgumentNullException(RequestNotFound);
            }
            req.Status = RequestStatus.Declined;
            await _context.SaveChangesAsync();
        }
    }
}
