public interface ITournamentService
{
    Task<IEnumerable<Tournament>> ListAllTournamentsAsync();

    Task<Tournament?> GetTournamentDetailsAsync(int tournamentId);

    Task<Tournament> AddTournamentAsync(TournamentDto tournamentDto);

    Task<IEnumerable<Person>> ListTournamentParticipantsAsync(int tournamentId);
}
