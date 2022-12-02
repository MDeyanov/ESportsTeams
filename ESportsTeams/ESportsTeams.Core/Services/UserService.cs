using ESportsTeams.Core.Interfaces;
using ESportsTeams.Infrastructure.Data;
using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
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

        public async Task<AppUser> FindUserByIdAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Id== userId);
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
