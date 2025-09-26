public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<Person?> GetPlayerProfileAsync(int playerId)
    {
        return await _personRepository.GetByIdAsync(playerId);
    }

    public async Task<IEnumerable<Person>> ListAllPlayersAsync()
    {
        return await _personRepository.GetAllAsync();
    }
    public async Task<Person> RegisterPlayerAsync(Person person)
    {
        await _personRepository.AddAsync(person);
        return person;
    }

    public async Task<IEnumerable<Tournament>> ListPlayerTournaments(int playerId)
    {
        var player = await _personRepository.GetByIdAsync(playerId)
            ?? throw new KeyNotFoundException("Player not found.");

        return player.ParticipatingIn;
    }

    public async Task RemovePlayerAsync(int playerId)
    {
        var player = await _personRepository.GetByIdAsync(playerId)
            ?? throw new KeyNotFoundException("Player not found.");

        await _personRepository.DeleteAsync(player);
    }
}
