namespace GoTournamentPlanner.Tests;

using Moq;

public class PersonServiceTests
{
    private readonly Mock<IPersonRepository> _mockRepository;
    private readonly PersonService _service;

    public PersonServiceTests()
    {
        _mockRepository = new Mock<IPersonRepository>();
        _service = new PersonService(_mockRepository.Object);
    }

    [Fact]
    public async Task ListAllPlayersAsync_ShouldReturnAllPlayers()
    {
        // Arrange
        var players = new List<Person>
        {
            new Person { Id = 1, Name = new Name { Family = "Redmond", Given = "Michael", IsGivenFirst = true }, Rank = Rank.Dan9 },
            new Person { Id = 2, Name = new Name { Family = "Lee", Given = "Sedol", IsGivenFirst = false }, Rank = Rank.Dan9 }
        };
        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(players);

        // Act
        var result = await _service.ListAllPlayersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetPlayerProfileAsync_GivenValidId_ShouldReturnPlayer()
    {
        // Arrange
        var player = new Person { Id = 1, Name = new Name { Family = "Redmond", Given = "Michael", IsGivenFirst = true }, Rank = Rank.Dan9 };
        _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(player);

        // Act
        var result = await _service.GetPlayerProfileAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Redmond", result.Name.Family);
        _mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetPlayerProfileAsync_GivenInvalidId_ShouldReturnNull()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Person?)null);

        // Act
        var result = await _service.GetPlayerProfileAsync(999);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task RegisterPlayerAsync_ShouldAddPlayerAndReturnIt()
    {
        // Arrange
        var newPlayer = new Person { Name = new Name { Family = "Smith", Given = "John", IsGivenFirst = true }, Rank = Rank.Kyuu5 };
        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Person>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.RegisterPlayerAsync(newPlayer);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Smith", result.Name.Family);
        _mockRepository.Verify(repo => repo.AddAsync(newPlayer), Times.Once);
    }

    [Fact]
    public async Task ListPlayerTournaments_GivenValidId_ShouldReturnTournaments()
    {
        // Arrange
        var tournaments = new List<Tournament>
        {
            new Tournament 
            { 
                Id = 1, 
                Name = "Spring Open", 
                Location = "NYC", 
                StartDate = new DateOnly(2026, 3, 15),
                EndDate = new DateOnly(2026, 3, 17),
                Ruleset = Ruleset.AGA,
                Organizer = new Person { Id = 3, Name = new Name { Family = "Organizer", Given = "Bob", IsGivenFirst = true } }
            }
        };
        var player = new Person 
        { 
            Id = 1, 
            Name = new Name { Family = "Redmond", Given = "Michael", IsGivenFirst = true },
            ParticipatingIn = tournaments
        };
        _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(player);

        // Act
        var result = await _service.ListPlayerTournaments(1);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Spring Open", result.First().Name);
        _mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task ListPlayerTournaments_GivenInvalidId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Person?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ListPlayerTournaments(999));
        _mockRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task RemovePlayerAsync_GivenValidId_ShouldDeletePlayer()
    {
        // Arrange
        var player = new Person { Id = 1, Name = new Name { Family = "Redmond", Given = "Michael", IsGivenFirst = true } };
        _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(player);
        _mockRepository.Setup(repo => repo.DeleteAsync(player)).Returns(Task.CompletedTask);

        // Act
        await _service.RemovePlayerAsync(1);

        // Assert
        _mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        _mockRepository.Verify(repo => repo.DeleteAsync(player), Times.Once);
    }

    [Fact]
    public async Task RemovePlayerAsync_GivenInvalidId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Person?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.RemovePlayerAsync(999));
        _mockRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
    }
}
