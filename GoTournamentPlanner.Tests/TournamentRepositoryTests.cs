
namespace GoTournamentPlanner.Tests;

public class TournamentRepositoryTests : TestBase
{
    private readonly TournamentRepository _repository;

    public TournamentRepositoryTests() : base()
    {
        _repository = new TournamentRepository(Context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTournaments()
    {
        // Arrange
        await SeedDataAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, t => t.Name == "Spring Open 2026");
        Assert.Contains(result, t => t.Name == "Summer Open 2020");
    }
}
