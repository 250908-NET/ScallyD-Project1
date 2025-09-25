public interface ITournamentService
{
    Task<IEnumerable<Tournament>> ListAllTournamentsAsync();

    Task<Tournament?> GetTournamentDetailsAsync(int tournamentId);

    Task<Tournament> AddTournamentAsync(TournamentDto tournamentDto);

    Task<IEnumerable<Person>> ListTournamentParticipantsAsync(int tournamentId);

    Task RegisterParticipantAsync(int tournamentId, int playerId);

    Task WithdrawParticipantAsync(int tournamentId, int playerId);
}
