using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Services;

namespace ESportsTeams.Extensions
{
    public static class ApplicationServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ITournamentService, TournamentService>();
            

            return services;
        }
    }
}
