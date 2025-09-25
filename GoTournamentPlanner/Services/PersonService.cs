public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<IEnumerable<Person>> ListAllPlayersAsync()
    {
        return await _personRepository.GetAllAsync();
    }
}
