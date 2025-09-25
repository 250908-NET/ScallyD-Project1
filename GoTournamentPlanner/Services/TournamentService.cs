public class TournamentService : ITournamentService
{
    private readonly ITournamentRepository _tournamentRepository;

    public TournamentService(ITournamentRepository tournamentRepository)
    {
        _tournamentRepository = tournamentRepository;
    }

    public async Task<IEnumerable<Tournament>> ListAllTournamentsAsync()
    {
        return await _tournamentRepository.GetAllAsync();
    }
}
