
using Microsoft.EntityFrameworkCore;

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

    [Fact]
    public async Task AddAsync_ShouldCreateTournament()
    {
        // Arrange
        await SeedDataAsync();
        Person organizer = await Context.Persons.FirstAsync();
        Tournament newTournament = new Tournament
        {
            Name = "Autumn Open 2023",
            Location = "New York, NY",
            StartDate = new DateOnly(2023, 10, 5),
            EndDate = new DateOnly(2023, 10, 7),
            Ruleset = Ruleset.Chinese,
            Organizer = organizer
        };

        // Act
        await _repository.AddAsync(newTournament);
        var result = await _repository.GetByIdAsync(newTournament.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Autumn Open 2023", result.Name);
        Assert.Equal("New York, NY", result.Location);
        Assert.Equal(new DateOnly(2023, 10, 5), result.StartDate);
        Assert.Equal(new DateOnly(2023, 10, 7), result.EndDate);
        Assert.Equal(Ruleset.Chinese, result.Ruleset);
        Assert.Equal(organizer.Id, result.Organizer.Id);
    }
}
