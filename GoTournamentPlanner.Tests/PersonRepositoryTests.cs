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
    public async Task GetByIdAsync_GivenValidId_ShouldReturnPlayer()
    {
        // Arrange
        await SeedDataAsync();
        Person existingPlayer = (await _repository.GetAllAsync()).First();

        // Act
        var result = await _repository.GetByIdAsync(existingPlayer.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingPlayer.Id, result.Id);
        Assert.Equal(existingPlayer.Name.Family, result.Name.Family);
        Assert.Equal(existingPlayer.Name.Given, result.Name.Given);
    }

    [Fact]
    public async Task GetByIdAsync_GivenInvalidId_ShouldReturnNull()
    {
        // Arrange
        await SeedDataAsync();
        int invalidId = 999;

        // Act
        var result = await _repository.GetByIdAsync(invalidId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldCreatePerson()
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
    public async Task AddAsync_WithoutEmail_ShouldCreatePersonWithNullEmail()
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
    public async Task AddAsync_WithoutRank_ShouldCreatePersonWithNullRank()
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

    [Fact]
    public async Task DeleteAsync_ShouldDeletePerson()
    {
        // Arrange
        await SeedDataAsync();
        Person existingPlayer = (await _repository.GetAllAsync()).First();

        // Act
        await _repository.DeleteAsync(existingPlayer);
        var result = await _repository.GetByIdAsync(existingPlayer.Id);

        // Assert
        Assert.Null(result);
    }
}
