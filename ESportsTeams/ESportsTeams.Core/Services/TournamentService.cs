using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Models.BindingModels.Tournament;
using ESportsTeams.Core.Models.ViewModels.TournamentViewModels;
using ESportsTeams.Infrastructure.Data;
using ESportsTeams.Infrastructure.Data.Entity;
using static ESportsTeams.Infrastructure.Data.Common.CommonConstants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
                Where(x=>x.Category.ToString()==eventTitle)
                .FirstOrDefaultAsync(x => x.OwnerId == user.Id);
           
            if (team == null)
            {
                throw new ArgumentException(TeamNotFound);
            }          
                               
            tournament.TeamTournaments.Add(new TeamTournament()
            {
                TeamId= team.Id,
                Team = team,
                TournamentId= tournament.Id,
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
            var newTournament = new Tournament()
            {
                Title = model.Title,
                Description= model.Description,
                StartTime= model.StartTime,
                EntryFee= model.EntryFee,
                Website = model.Website,
                Twitter= model.Twitter,
                Facebook= model.Facebook,
                Contact = model.Contact,
                PrizePool   = model.PrizePool,
                Image = result.Url.ToString(),
                Address = new Address()
                {
                    Street = model.Address.Street,
                    City = model.Address.City,
                    Country = model.Address.Country,
                },
                Event = eventByTitle
            };
            await _context.Tournaments.AddAsync(newTournament); 
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TournamentViewModel>> GetTournamentByEventIdAsync(int id)
        {
            var tournaments = await _context.Tournaments
                .Include(t=>t.Address)
                .Include(t=>t.TeamTournaments)
                .ThenInclude(t=>t.Team)
                .Where(x=>x.EventId== id).ToListAsync();

            if (tournaments == null)
            {
                throw new ArgumentException("Invalid Event ID!");
            }

            return tournaments.Select(t => new TournamentViewModel()
            {
                Id= t.Id,
                Title= t.Title,
                Description= t.Description,
                Image = t.Image,
                StartTime= t.StartTime,
                EntryFee= t.EntryFee,
                Website= t.Website,
                Twitter = t.Twitter,
                Facebook= t.Facebook,
                Contact= t.Contact,
                PrizePool= t.PrizePool,
                Address = t.Address,
                TeamTournaments= t.TeamTournaments,
                Reviews = t.Reviews,
            });
        }

        public async Task<Tournament?> GetTournamentByIdAsync(int id)
        {
            return await _context.Tournaments
                .Include(x=>x.Event)
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<TournamentDetailsViewModel?> GetTournamentDetailsByIdAsync(int id)
        {
            var tournament = await _context.Tournaments
                .Include(t => t.Address)
                .Include(t => t.TeamTournaments)
                .ThenInclude(t => t.Team)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (tournament==null)
            {
                throw new ArgumentException("Tournament not found!");
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
               Address = tournament.Address,
               TeamTournaments = tournament.TeamTournaments,
               Reviews = tournament.Reviews,

           };
            return result;
        }
    }
}
