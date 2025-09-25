public interface ITournamentService
{
    Task<IEnumerable<Tournament>> ListAllTournamentsAsync();
}
