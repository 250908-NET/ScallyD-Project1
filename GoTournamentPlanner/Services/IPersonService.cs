public interface IPersonService
{
    Task<IEnumerable<Person>> ListAllPlayersAsync();
    Task<Person> RegisterPlayerAsync(Person person);
}
