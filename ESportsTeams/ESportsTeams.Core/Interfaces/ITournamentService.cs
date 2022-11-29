using ESportsTeams.Core.Models.BindingModels.Tournament;
using ESportsTeams.Core.Models.ViewModels.TeamViewModels;
using ESportsTeams.Core.Models.ViewModels.TournamentViewModels;
using ESportsTeams.Infrastructure.Data.Entity;
using ESportsTeams.Infrastructure.Data.Enums;


namespace ESportsTeams.Core.Interfaces
{
    public interface ITournamentService
    {
        Task<Tournament?> GetTournamentByIdAsync(int id);

        Task<IEnumerable<TournamentViewModel>> GetTournamentByEventIdAsync(int id);

        Task<TournamentDetailsViewModel?> GetTournamentDetailsByIdAsync(int id);

        Task AddTeamToTournamentAsync(string userId, int tournamentId);

        Task AddTournamentAsync(AddTournamentBindingModel model);

        //Task<IEnumerable<IndexTournamentAdminViewModel>> GetAllTournamentsForAdminAsync();

    }
}
