public class TournamentService : ITournamentService
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IPersonRepository _personRepository;

    public TournamentService(ITournamentRepository tournamentRepository, IPersonRepository personRepository)
    {
        _tournamentRepository = tournamentRepository;
        _personRepository = personRepository;
    }

    public async Task<IEnumerable<Tournament>> ListAllTournamentsAsync()
    {
        return await _tournamentRepository.GetAllAsync();
    }

    public async Task<Tournament?> GetTournamentDetailsAsync(int id)
    {
        return await _tournamentRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Person>> ListTournamentParticipantsAsync(int tournamentId)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);

        if (tournament == null)
        {
            throw new KeyNotFoundException("Tournament not found.");
        }

        return tournament.Participants;
    }

    public async Task<Tournament> AddTournamentAsync(TournamentDto tournamentDto)
    {
        Person organizer = await _personRepository.GetByIdAsync(tournamentDto.OrganizerId)
            ?? throw new ArgumentException("Organizer not found.");

        Tournament tournament = new()
        {
            Name = tournamentDto.Name,
            Location = tournamentDto.Location,
            StartDate = DateOnly.Parse(tournamentDto.StartDate),
            EndDate = DateOnly.Parse(tournamentDto.EndDate),
            Ruleset = tournamentDto.Ruleset,
            Organizer = organizer
        };

        await _tournamentRepository.AddAsync(tournament);
        return tournament;
    }

    public async Task RegisterParticipantAsync(int tournamentId, int playerId)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(tournamentId)
            ?? throw new KeyNotFoundException("Tournament not found.");

        var player = await _personRepository.GetByIdAsync(playerId)
            ?? throw new KeyNotFoundException("Player not found.");

        if (tournament.Participants.Any(p => p.Id == playerId))
        {
            throw new InvalidOperationException("Player is already registered for this tournament.");
        }

        tournament.Participants.Add(player);
        await _tournamentRepository.UpdateAsync(tournament);
    }
}
