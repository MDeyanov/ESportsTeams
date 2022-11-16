using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Models.BindingModels.Team;
using ESportsTeams.Infrastructure.Data;
using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsTeams.Core.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;


        public TeamService(ApplicationDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

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

        public async Task<int> GetCountAsync()
        {
            return await _context.Teams.CountAsync();
        }

        public async Task<int> GetCountByCategoryAsync(Category category)
        {
            return await _context.Teams.CountAsync(t=>t.Category== category);
        }

        public async Task<IEnumerable<Team>> GetSliceAsync(int offset, int size)
        {
            return await _context.Teams.Include(x => x.Address).Skip(offset).Take(size).ToListAsync();
        }

        public async Task<IEnumerable<Team>> GetTeamByCountry(string country)
        {
            return await _context.Teams.Where(t => t.Address.Country.Contains(country)).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<Team>> GetTeamsByCategoryAndSliceAsync(Category category, int offset, int size)
        {
            return await _context.Teams
                .Include(x => x.Address)
                .Where(x => x.Category == category)
                .Skip(offset)
                .Take(size)
                .ToListAsync();
        }
    }
}
