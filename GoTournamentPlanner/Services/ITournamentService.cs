public interface ITournamentService
{
    Task<IEnumerable<Tournament>> ListAllTournamentsAsync();

    Task<Tournament> AddTournamentAsync(TournamentDto tournamentDto);
}
