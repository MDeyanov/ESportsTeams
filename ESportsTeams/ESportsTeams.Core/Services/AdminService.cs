using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Models.ViewModels.UserViewModel;
using ESportsTeams.Infrastructure.Data;
using ESportsTeams.Infrastructure.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static ESportsTeams.Infrastructure.Data.Common.CommonConstants;

namespace ESportsTeams.Core.Services
{
    public class AdminService : IAdminService
    {
        

        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminService(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

       

        public ICollection<UserConciseViewModel> GetAllUsers(string currentUserId)
        {
            var users = _userManager.Users
                .Where(x => x.Id != currentUserId)
                .Select(x => new UserConciseViewModel()
                {
                    Id = x.Id,
                    Username = x.UserName,
                    Email = x.Email,
                    IsBanned = x.IsBanned,
                })
                .ToList();
            return users;
        }

        public UserDetailsViewModel GetUser(string userId)
        {
            var user = _context.Users
                .Include(x=>x.OwnedTeams)
                .Include(x=>x.Address)
                .FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return null;
            }

            user.ProfileImageUrl ??= "https://res.cloudinary.com/dzac3ggur/image/upload/v1669675626/AnonymousProfileImage_ntgywz.png";
            var result = new UserDetailsViewModel()
            {
                UserId = userId,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsBanned = user.IsBanned,
                ProfileImageUrl = user.ProfileImageUrl,
                Dota2MMR = user.Dota2MMR,
                CSGOMMR = user.CSGOMMR,
                PUBGMMR = user.PUBGMMR,
                LeagueOfLegendsMMR = user.LeagueOfLegendsMMR,
                VALORANTMMR = user.VALORANTMMR,
                OwnedTeams = user.OwnedTeams,
            };
            return result;
        }
        public string BanUser(string userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                return ErrorUserId;
            }

            var currentTime = DateTime.UtcNow;
            user.LockoutEnd = currentTime.AddMonths(AccountLockOutInMonths);
            user.IsBanned = true;
            if (user.OwnedTeams != null)
            {
                foreach (var team in user.OwnedTeams)
                {
                    team.IsBanned = true;
                    if (team.AppUsers != null && team.AppUsers.Count > 1)
                    {
                        foreach (var appuser in team.AppUsers)
                        {
                            if (team.OwnerId != appuser.Id)
                            {
                                team.AppUsers.Remove(appuser);
                            }                           
                        }
                    }
                    
                }
            }
            _context.SaveChanges();
            return user.Id;
        }

        public string UnbanUser(string userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                return ErrorUserId;
            }

            var currentTime = DateTime.UtcNow;
            user.LockoutEnd = currentTime.AddDays(AccountUnLockInDays);
            user.IsBanned = false;
            if (user.OwnedTeams != null)
            {
                foreach (var team in user.OwnedTeams)
                {
                    team.IsBanned = false;
                }
            }
            _context.SaveChanges();
            return user.Id;
        }
    }
}
