using ESportsTeams.Core.Interfaces;
using ESportsTeams.Infrastructure.Data;
using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;
        public TournamentService(ApplicationDbContext context, IPhotoService photoService, IUserService userService)
        {
            _context = context;
            _photoService = photoService;
            _userService = userService;
        }


        public async Task<Tournament?> GetTournamentByIdAsync(int id)
        {
            var result = await _context.Tournaments
                .Include(x=>x.Event)
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }
    }
}
