using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Models.ViewModels.TournamentViewModels;
using ESportsTeams.Infrastructure.Data;
using ESportsTeams.Infrastructure.Data.Entity;
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
        public TournamentService(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task AddTeamToTournamentAsync(string userId, int tournamentId)
        {
            var user = await _userService.FindUserByIdAsync(userId);
            var team = await _context.Teams.FirstOrDefaultAsync(x => x.OwnerId == user.Id);
            var tournament = await _context.Tournaments.FirstOrDefaultAsync(x => x.Id == tournamentId);
            if (tournament == null)
            {
                throw new ArgumentException("Tournament not found!");
            }
            if (team == null)
            {
                throw new ArgumentException("Team not found!");
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
