public interface IPersonService
{
    Task<IEnumerable<Person>> ListAllPlayersAsync();

    Task<Person?> GetPlayerProfileAsync(int playerId);

    Task<Person> RegisterPlayerAsync(Person person);

    Task<IEnumerable<Tournament>> ListPlayerTournaments(int playerId);

    Task RemovePlayerAsync(int playerId);
}
