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
}
