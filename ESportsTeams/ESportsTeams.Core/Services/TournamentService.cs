using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Models.BindingModels.Tournament;
using ESportsTeams.Core.Models.ViewModels.TournamentViewModels;
using ESportsTeams.Infrastructure.Data;
using ESportsTeams.Infrastructure.Data.Entity;
using static ESportsTeams.Infrastructure.Data.Common.CommonConstants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using ESportsTeams.Core.Models.ViewModels.TeamViewModels;

namespace ESportsTeams.Core.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IPhotoService _photoService;
        private readonly IEventService _eventService;
        public TournamentService(ApplicationDbContext context, IUserService userService, IPhotoService photoService, IEventService eventService)
        {
            _context = context;
            _userService = userService;
            _photoService = photoService;
            _eventService = eventService;
        }

        public async Task AddTeamToTournamentAsync(string userId, int tournamentId)
        {
            var tournament = await _context.Tournaments.FirstOrDefaultAsync(x => x.Id == tournamentId);
            if (tournament == null)
            {
                throw new ArgumentException(TournamentNotFound);
            }
            var eventTitle = tournament.Event.Title;
            var user = await _userService.FindUserByIdAsync(userId);
            var team = await _context.Teams.
                Where(x => x.Category.ToString() == eventTitle)
                .FirstOrDefaultAsync(x => x.OwnerId == user.Id);

            if (team == null)
            {
                throw new ArgumentException(TeamNotFound);
            }

            tournament.TeamTournaments.Add(new TeamTournament()
            {
                TeamId = team.Id,
                Team = team,
                TournamentId = tournament.Id,
                Tournament = tournament
            });
            await _context.SaveChangesAsync();
        }

        public async Task AddTournamentAsync(AddTournamentBindingModel model)
        {
            var result = await _photoService.AddPhotoAsync(model.Image);
            var eventByTitle = await _eventService.GetEventByTitleAsync(model.EventTitle);
            if (eventByTitle == null)
            {
                throw new ArgumentException(EventNotFound);
            }
            var address = new Address();
            if (model.Address == null)
            {
                address.Street = "Online";
                address.City = "Online";
                address.Country = "Online";
            }
            else
            {
                address = model.Address;
            }
            var newTournament = new Tournament()
            {
                Title = model.Title,
                Description = model.Description,
                StartTime = model.StartTime,
                EntryFee = model.EntryFee,
                Website = model.Website,
                Twitter = model.Twitter,
                Facebook = model.Facebook,
                Contact = model.Contact,
                PrizePool = model.PrizePool,
                Image = result.Url.ToString(),
                Address = address,
                Event = eventByTitle
            };
            await _context.Tournaments.AddAsync(newTournament);
            await _context.SaveChangesAsync();
        }

        public async Task EditTournamentAsync(EditTournamentBindingModel model)
        {
            var tournamentToEdit = await _context.Tournaments.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (tournamentToEdit == null)
            {
                throw new ArgumentException(TournamentNotFound);
            }
            if (model.Image != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(model.Image);
                if (!string.IsNullOrEmpty(tournamentToEdit.Image))
                {
                    _ = _photoService.DeletePhotoAsync(tournamentToEdit.Image);
                }
                tournamentToEdit.Image = photoResult.Url.ToString();
            }
            if (model.EventTitle != null)
            {
                var currentEvent = await _context.Events.FirstOrDefaultAsync(x => x.Title.ToLower() == model.EventTitle);
                if (currentEvent != null)
                {
                    tournamentToEdit.Event = currentEvent;
                    tournamentToEdit.EventId = currentEvent.Id;
                }
            }

            tournamentToEdit.Title = model.Title;
            tournamentToEdit.Description = model.Description;
            tournamentToEdit.StartTime = model.StartTime;
            tournamentToEdit.Website = model.Website;
            tournamentToEdit.Twitter = model.Twitter;
            tournamentToEdit.Facebook = model.Facebook;
            tournamentToEdit.Contact = model.Contact;
            tournamentToEdit.PrizePool = model.PrizePool;
            tournamentToEdit.EntryFee = model.EntryFee;
            tournamentToEdit.Address = model.Address;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TournamentViewModel>> GetTournamentByEventIdAsync(int id)
        {
            var tournaments = await _context.Tournaments
                .Include(t => t.Address)
                .Include(t => t.TeamTournaments)
                .ThenInclude(t => t.Team)
                .Where(x => x.EventId == id).ToListAsync();

            if (tournaments == null)
            {
                throw new ArgumentException(EventNotFound);
            }

            return tournaments.Select(t => new TournamentViewModel()
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Image = t.Image,
                StartTime = t.StartTime,
                EntryFee = t.EntryFee,
                Website = t.Website,
                Twitter = t.Twitter,
                Facebook = t.Facebook,
                Contact = t.Contact,
                PrizePool = t.PrizePool,
                Address = t.Address,
                TeamTournaments = t.TeamTournaments,
            });
        }

        public async Task<Tournament?> GetTournamentByIdAsync(int id)
        {
            return await _context.Tournaments
                .Include(x => x.Event)
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<TournamentDetailsViewModel?> GetTournamentDetailsByIdAsync(int id)
        {
            var tournament = await _context.Tournaments
                .Include(t => t.Address)
                .Include(t => t.Event)
                .Include(t => t.TeamTournaments)
                .ThenInclude(t => t.Team)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (tournament == null)
            {
                throw new ArgumentException(TournamentNotFound);
            }
            var address = new Address();
            if (tournament.Address == null)
            {
                address.Street = "Online";
                address.City = "Online";
                address.Country = "Online";
            }
            else
            {
                address = tournament.Address;
            }
            var result = new TournamentDetailsViewModel()
            {
                Id = tournament.Id,
                Title = tournament.Title,
                Description = tournament.Description,
                Image = tournament.Image,
                StartTime = tournament.StartTime,
                EntryFee = tournament.EntryFee,
                Website = tournament.Website,
                Twitter = tournament.Twitter,
                Facebook = tournament.Facebook,
                Contact = tournament.Contact,
                PrizePool = tournament.PrizePool,
                Address = address,
                TeamTournaments = tournament.TeamTournaments,
                EventTitle = tournament.Event.Title
            };
            return result;
        }

        public async Task<IEnumerable<GetTeamsViewModel>> ListOfTeamsInTournamentAsync(int tournamentId)
        {
            var tournament = await _context.Tournaments
                 .Where(t => t.Id == tournamentId)
                 .Include(t => t.TeamTournaments)
                 .ThenInclude(t => t.Team)
                 .ThenInclude(t => t.Owner)
                 .FirstOrDefaultAsync();

            if (tournament == null)
            {
                throw new ArgumentException(InvalidTournamentId);
            }

            return tournament.TeamTournaments
                .Select(tt => new GetTeamsViewModel()
                {
                    Id = tt.TeamId,
                    Name = tt.Team.Name,
                    Description = tt.Team.Description,
                    Category = tt.Team.Category,
                    Image= tt.Team.Image,
                    Address= tt.Team.Address,
                    TournamentWin = tt.Team.TournamentWin,
                    Owner = tt.Team.Owner,
                    AvarageMMR = tt.Team.AvarageMMR,
                });
        }

        public async Task TeamJoinToTournaments(string userId, int tournamentId)
        {

            var tournament = await _context.Tournaments
                .Where(x => x.Id == tournamentId)
                .Include(x => x.TeamTournaments)
                .Include(x=>x.Event)
                .FirstOrDefaultAsync();

            if (tournament == null)
            {
                throw new ArgumentException(InvalidTournamentId);
            }

            var user = await _context.Users
               .Where(x => x.Id == userId)
               .Include(x => x.OwnedTeams)
               .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException(InvalidUser);
            }

            var teams = await _context.Teams
                .Where(x => x.OwnerId == user.Id)
                .ToListAsync();
            if (teams == null)
            {
                throw new ArgumentException(DoNotOwnTeam);
            }

            string eventTitle = tournament.Event.Title.ToLower();

            var team = teams.Where(t => t.Category.ToString().ToLower() == eventTitle).FirstOrDefault();

            if (team == null)
            {
                throw new ArgumentException(InvalidTeamCategory);
            }          

            tournament.TeamTournaments.Add(new TeamTournament()
            {
                TeamId = team.Id,
                Team = team,
                TournamentId = tournament.Id,
                Tournament = tournament
            });
            await _context.SaveChangesAsync();

        }
    }
}
