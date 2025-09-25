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

    [Fact]
    public async Task RegisterPlayerAsync_ShouldCreatePerson()
    {
        // Arrange
        Person newPlayer = new()
        {
            Name = new Name { Family = "Smith", Given = "John", IsGivenFirst = true },
            Email = "myemail@example.com",
            Rank = Rank.Kyuu15
        };

        // Act
        await _repository.AddAsync(newPlayer);
        var retrievedPlayer = await _repository.GetByIdAsync(newPlayer.Id);

        // Assert
        Assert.NotNull(retrievedPlayer);
        Assert.Equal("Smith", retrievedPlayer.Name.Family);
        Assert.Equal("John", retrievedPlayer.Name.Given);
        Assert.Equal("myemail@example.com", retrievedPlayer.Email);
        Assert.Equal(Rank.Kyuu15, retrievedPlayer.Rank);
    }

    [Fact]
    public async Task RegisterPlayerAsync_WithoutEmail_ShouldCreatePersonWithNullEmail()
    {
        // Arrange
        Person newPlayer = new()
        {
            Name = new Name { Family = "Smith", Given = "John", IsGivenFirst = true },
            Rank = Rank.Kyuu15
        };

        // Act
        await _repository.AddAsync(newPlayer);
        var retrievedPlayer = await _repository.GetByIdAsync(newPlayer.Id);

        // Assert
        Assert.NotNull(retrievedPlayer);
        Assert.Equal("Smith", retrievedPlayer.Name.Family);
        Assert.Equal("John", retrievedPlayer.Name.Given);
        Assert.Equal(Rank.Kyuu15, retrievedPlayer.Rank);
        Assert.Null(retrievedPlayer.Email);
    }

    [Fact]
    public async Task RegisterPlayerAsync_WithoutRank_ShouldCreatePersonWithNullRank()
    {
        // Arrange
        Person newPlayer = new()
        {
            Name = new Name { Family = "Smith", Given = "John", IsGivenFirst = true },
            Email = "myemail@example.com"
        };

        // Act
        await _repository.AddAsync(newPlayer);
        var retrievedPlayer = await _repository.GetByIdAsync(newPlayer.Id);

        // Assert
        Assert.NotNull(retrievedPlayer);
        Assert.Equal("Smith", retrievedPlayer.Name.Family);
        Assert.Equal("John", retrievedPlayer.Name.Given);
        Assert.Equal("myemail@example.com", retrievedPlayer.Email);
        Assert.Null(retrievedPlayer.Rank);
    }
}
