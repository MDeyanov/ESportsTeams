using ESportsTeams.Core.Interfaces;
using ESportsTeams.Infrastructure.Data;
using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static ESportsTeams.Infrastructure.Data.Common.CommonConstants;

namespace ESportsTeams.Core.Services
{
    public class UserService : IUserService
    {     
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CurrentUserTeamsHaveCategory(string userId, Category category)
        {
            var result = await _context.Teams
                .Where(x=>x.IsBanned == false && x.OwnerId == userId)
               .FirstOrDefaultAsync(x=>x.Category == category);

            if (result == null)
            {
                return true;
            }
            return false;
        }

        public async Task<AppUser> FindUserByIdAsync(string userId)
        {
            var user = await _context.Users
                .Include(x=>x.Team)
                .Include(x=>x.OwnedTeams)
                .FirstOrDefaultAsync(x=>x.Id== userId);
            if (user==null)
            {
                throw new ArgumentException(UserNotFound);
            }
            
            return user;
        }

      

        public async Task<IEnumerable<Team>> GetUserOwnedTeamsAsync(string userId, Category category, int offset, int size)
        {
            return await _context.Teams
             .Where(x => x.OwnerId == userId)
             .Include(x => x.Address)
             .Where(x => x.Category == category)
             .Skip(offset)
             .Take(size)
             .ToListAsync();
        }


    }
}
