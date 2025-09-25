namespace GoTournamentPlanner.Tests;
public class PersonRepositoryTests : TestBase
{
    private readonly PersonRepository _repository;

    public PersonRepositoryTests() : base()
    {
        _repository = new PersonRepository(Context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPlayers()
    {
        // Arrange
        await SeedDataAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(result, p => p.Name.Family == "Redmond");
        Assert.Contains(result, p => p.Name.Family == "Lee");
        Assert.Contains(result, p => p.Name.Family == "Randomlastname");
    }
}
