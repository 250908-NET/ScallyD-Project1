namespace GoTournamentPlanner.Tests;

using Moq;

public class TournamentServiceTests
{
    private readonly Mock<ITournamentRepository> _mockTournamentRepository;
    private readonly Mock<IPersonRepository> _mockPersonRepository;
    private readonly TournamentService _service;

    public TournamentServiceTests()
    {
        _mockTournamentRepository = new Mock<ITournamentRepository>();
        _mockPersonRepository = new Mock<IPersonRepository>();
        _service = new TournamentService(_mockTournamentRepository.Object, _mockPersonRepository.Object);
    }

    [Fact]
    public async Task ListAllTournamentsAsync_ShouldReturnAllTournaments()
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
                Organizer = new Person { Id = 1, Name = new Name { Family = "Smith", Given = "John", IsGivenFirst = true } }
            },
            new Tournament 
            { 
                Id = 2, 
                Name = "Summer Open", 
                Location = "DC", 
                StartDate = new DateOnly(2026, 6, 20),
                EndDate = new DateOnly(2026, 6, 22),
                Ruleset = Ruleset.Japanese,
                Organizer = new Person { Id = 2, Name = new Name { Family = "Doe", Given = "Jane", IsGivenFirst = true } }
            }
        };
        _mockTournamentRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tournaments);

        // Act
        var result = await _service.ListAllTournamentsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockTournamentRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetTournamentDetailsAsync_GivenValidId_ShouldReturnTournament()
    {
        // Arrange
        var tournament = new Tournament 
        { 
            Id = 1, 
            Name = "Spring Open", 
            Location = "NYC", 
            StartDate = new DateOnly(2026, 3, 15),
            EndDate = new DateOnly(2026, 3, 17),
            Ruleset = Ruleset.AGA,
            Organizer = new Person { Id = 1, Name = new Name { Family = "Smith", Given = "John", IsGivenFirst = true } }
        };
        _mockTournamentRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(tournament);

        // Act
        var result = await _service.GetTournamentDetailsAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Spring Open", result.Name);
        _mockTournamentRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetTournamentDetailsAsync_GivenInvalidId_ShouldReturnNull()
    {
        // Arrange
        _mockTournamentRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Tournament?)null);

        // Act
        var result = await _service.GetTournamentDetailsAsync(999);

        // Assert
        Assert.Null(result);
        _mockTournamentRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task AddTournamentAsync_GivenValidDto_ShouldCreateAndReturnTournament()
    {
        // Arrange
        var organizer = new Person { Id = 1, Name = new Name { Family = "Smith", Given = "John", IsGivenFirst = true } };
        var dto = new TournamentDto 
        { 
            Name = "Spring Open",
            Location = "NYC",
            StartDate = "2026-03-15",
            EndDate = "2026-03-17",
            Ruleset = Ruleset.AGA,
            OrganizerId = 1
        };
        _mockPersonRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(organizer);
        _mockTournamentRepository.Setup(repo => repo.AddAsync(It.IsAny<Tournament>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.AddTournamentAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Spring Open", result.Name);
        Assert.Equal("NYC", result.Location);
        Assert.Equal(new DateOnly(2026, 3, 15), result.StartDate);
        Assert.Equal(new DateOnly(2026, 3, 17), result.EndDate);
        Assert.Equal(Ruleset.AGA, result.Ruleset);
        Assert.Equal(organizer.Id, result.Organizer.Id);
        _mockPersonRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        _mockTournamentRepository.Verify(repo => repo.AddAsync(It.IsAny<Tournament>()), Times.Once);
    }

    [Fact]
    public async Task AddTournamentAsync_GivenInvalidOrganizerId_ShouldThrowArgumentException()
    {
        // Arrange
        var dto = new TournamentDto 
        { 
            Name = "Spring Open",
            Location = "NYC",
            StartDate = "2026-03-15",
            EndDate = "2026-03-17",
            Ruleset = Ruleset.AGA,
            OrganizerId = 999
        };
        _mockPersonRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Person?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.AddTournamentAsync(dto));
        _mockPersonRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task ListTournamentParticipantsAsync_GivenValidId_ShouldReturnParticipants()
    {
        // Arrange
        var participants = new List<Person>
        {
            new Person { Id = 1, Name = new Name { Family = "Redmond", Given = "Michael", IsGivenFirst = true } },
            new Person { Id = 2, Name = new Name { Family = "Lee", Given = "Sedol", IsGivenFirst = false } }
        };
        var tournament = new Tournament 
        { 
            Id = 1, 
            Name = "Spring Open", 
            Location = "NYC", 
            StartDate = new DateOnly(2026, 3, 15),
            EndDate = new DateOnly(2026, 3, 17),
            Ruleset = Ruleset.AGA,
            Organizer = new Person { Id = 3, Name = new Name { Family = "Smith", Given = "John", IsGivenFirst = true } },
            Participants = participants
        };
        _mockTournamentRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(tournament);

        // Act
        var result = await _service.ListTournamentParticipantsAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockTournamentRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task ListTournamentParticipantsAsync_GivenInvalidId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        _mockTournamentRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Tournament?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ListTournamentParticipantsAsync(999));
        _mockTournamentRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task RegisterParticipantAsync_GivenValidIds_ShouldAddParticipant()
    {
        // Arrange
        var player = new Person 
        { 
            Id = 1, 
            Name = new Name { Family = "Redmond", Given = "Michael", IsGivenFirst = true },
            ParticipatingIn = new List<Tournament>()
        };
        var tournament = new Tournament 
        { 
            Id = 1, 
            Name = "Spring Open", 
            Location = "NYC", 
            StartDate = new DateOnly(2026, 3, 15),
            EndDate = new DateOnly(2026, 3, 17),
            Ruleset = Ruleset.AGA,
            Organizer = new Person { Id = 2, Name = new Name { Family = "Smith", Given = "John", IsGivenFirst = true } },
            Participants = new List<Person>()
        };
        _mockTournamentRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(tournament);
        _mockPersonRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(player);
        _mockTournamentRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Tournament>())).Returns(Task.CompletedTask);
        _mockPersonRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Person>())).Returns(Task.CompletedTask);

        // Act
        await _service.RegisterParticipantAsync(1, 1);

        // Assert
        Assert.Contains(player, tournament.Participants);
        Assert.Contains(tournament, player.ParticipatingIn);
        _mockTournamentRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        _mockPersonRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        _mockTournamentRepository.Verify(repo => repo.UpdateAsync(tournament), Times.Once);
        _mockPersonRepository.Verify(repo => repo.UpdateAsync(player), Times.Once);
    }

    [Fact]
    public async Task RegisterParticipantAsync_GivenInvalidTournamentId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        _mockTournamentRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Tournament?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.RegisterParticipantAsync(999, 1));
        _mockTournamentRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task RegisterParticipantAsync_GivenInvalidPlayerId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var tournament = new Tournament 
        { 
            Id = 1, 
            Name = "Spring Open", 
            Location = "NYC", 
            StartDate = new DateOnly(2026, 3, 15),
            EndDate = new DateOnly(2026, 3, 17),
            Ruleset = Ruleset.AGA,
            Organizer = new Person { Id = 2, Name = new Name { Family = "Smith", Given = "John", IsGivenFirst = true } },
            Participants = new List<Person>()
        };
        _mockTournamentRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(tournament);
        _mockPersonRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Person?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.RegisterParticipantAsync(1, 999));
        _mockTournamentRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        _mockPersonRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task RegisterParticipantAsync_WhenPlayerAlreadyRegistered_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var player = new Person 
        { 
            Id = 1, 
            Name = new Name { Family = "Redmond", Given = "Michael", IsGivenFirst = true }
        };
        var tournament = new Tournament 
        { 
            Id = 1, 
            Name = "Spring Open", 
            Location = "NYC", 
            StartDate = new DateOnly(2026, 3, 15),
            EndDate = new DateOnly(2026, 3, 17),
            Ruleset = Ruleset.AGA,
            Organizer = new Person { Id = 2, Name = new Name { Family = "Smith", Given = "John", IsGivenFirst = true } },
            Participants = new List<Person> { player }
        };
        _mockTournamentRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(tournament);
        _mockPersonRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(player);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.RegisterParticipantAsync(1, 1));
        _mockTournamentRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        _mockPersonRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task WithdrawParticipantAsync_GivenValidIds_ShouldRemoveParticipant()
    {
        // Arrange
        var player = new Person 
        { 
            Id = 1, 
            Name = new Name { Family = "Redmond", Given = "Michael", IsGivenFirst = true }
        };
        var tournament = new Tournament 
        { 
            Id = 1, 
            Name = "Spring Open", 
            Location = "NYC", 
            StartDate = new DateOnly(2026, 3, 15),
            EndDate = new DateOnly(2026, 3, 17),
            Ruleset = Ruleset.AGA,
            Organizer = new Person { Id = 2, Name = new Name { Family = "Smith", Given = "John", IsGivenFirst = true } }
        };
        player.ParticipatingIn = new List<Tournament> { tournament };
        tournament.Participants = new List<Person> { player };
        
        _mockTournamentRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(tournament);
        _mockTournamentRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Tournament>())).Returns(Task.CompletedTask);
        _mockPersonRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Person>())).Returns(Task.CompletedTask);

        // Act
        await _service.WithdrawParticipantAsync(1, 1);

        // Assert
        Assert.DoesNotContain(player, tournament.Participants);
        Assert.DoesNotContain(tournament, player.ParticipatingIn);
        _mockTournamentRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        _mockTournamentRepository.Verify(repo => repo.UpdateAsync(tournament), Times.Once);
        _mockPersonRepository.Verify(repo => repo.UpdateAsync(player), Times.Once);
    }

    [Fact]
    public async Task WithdrawParticipantAsync_GivenInvalidTournamentId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        _mockTournamentRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Tournament?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.WithdrawParticipantAsync(999, 1));
        _mockTournamentRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task WithdrawParticipantAsync_GivenPlayerNotInTournament_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var tournament = new Tournament 
        { 
            Id = 1, 
            Name = "Spring Open", 
            Location = "NYC", 
            StartDate = new DateOnly(2026, 3, 15),
            EndDate = new DateOnly(2026, 3, 17),
            Ruleset = Ruleset.AGA,
            Organizer = new Person { Id = 2, Name = new Name { Family = "Smith", Given = "John", IsGivenFirst = true } },
            Participants = new List<Person>()
        };
        _mockTournamentRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(tournament);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.WithdrawParticipantAsync(1, 999));
        _mockTournamentRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task RemoveTournamentAsync_GivenValidId_ShouldDeleteTournament()
    {
        // Arrange
        var tournament = new Tournament 
        { 
            Id = 1, 
            Name = "Spring Open", 
            Location = "NYC", 
            StartDate = new DateOnly(2026, 3, 15),
            EndDate = new DateOnly(2026, 3, 17),
            Ruleset = Ruleset.AGA,
            Organizer = new Person { Id = 1, Name = new Name { Family = "Smith", Given = "John", IsGivenFirst = true } }
        };
        _mockTournamentRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(tournament);
        _mockTournamentRepository.Setup(repo => repo.DeleteAsync(tournament)).Returns(Task.CompletedTask);

        // Act
        await _service.RemoveTournamentAsync(1);

        // Assert
        _mockTournamentRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        _mockTournamentRepository.Verify(repo => repo.DeleteAsync(tournament), Times.Once);
    }

    [Fact]
    public async Task RemoveTournamentAsync_GivenInvalidId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        _mockTournamentRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Tournament?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.RemoveTournamentAsync(999));
        _mockTournamentRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
    }
}
