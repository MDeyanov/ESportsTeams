using ESportsTeams.Core.Interfaces;
using ESportsTeams.Core.Services;

namespace ESportsTeams.Extensions
{
    public static class ApplicationServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
