namespace GoTournamentPlanner.Tests;

using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

public class EndpointTests
{
    private readonly Mock<IPersonService> _mockPersonService;
    private readonly Mock<ITournamentService> _mockTournamentService;

    public EndpointTests()
    {
        _mockPersonService = new Mock<IPersonService>();
        _mockTournamentService = new Mock<ITournamentService>();
    }

    #region Player Endpoints

    [Fact]
    public async Task GetPlayers_ShouldReturnAllPlayers()
    {
        // Arrange
        var players = new List<Person>
        {
            new Person { Id = 1, Name = new Name { Family = "Redmond", Given = "Michael", IsGivenFirst = true }, Rank = Rank.Dan9 },
            new Person { Id = 2, Name = new Name { Family = "Lee", Given = "Sedol", IsGivenFirst = false }, Rank = Rank.Dan9 }
        };
        _mockPersonService.Setup(s => s.ListAllPlayersAsync()).ReturnsAsync(players);

        // Act
        var result = await _mockPersonService.Object.ListAllPlayersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockPersonService.Verify(s => s.ListAllPlayersAsync(), Times.Once);
    }

    [Fact]
    public async Task PostPlayer_ShouldCreatePlayerAndReturnCreated()
    {
        // Arrange
        var newPlayer = new Person 
        { 
            Id = 1,
            Name = new Name { Family = "Smith", Given = "John", IsGivenFirst = true }, 
            Rank = Rank.Kyuu5 
        };
        _mockPersonService.Setup(s => s.RegisterPlayerAsync(It.IsAny<Person>())).ReturnsAsync(newPlayer);

        // Act
        var result = await _mockPersonService.Object.RegisterPlayerAsync(newPlayer);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Smith", result.Name.Family);
        _mockPersonService.Verify(s => s.RegisterPlayerAsync(newPlayer), Times.Once);
    }

    [Fact]
    public async Task GetPlayerById_GivenValidId_ShouldReturnPlayer()
    {
        // Arrange
        var player = new Person 
        { 
            Id = 1, 
            Name = new Name { Family = "Redmond", Given = "Michael", IsGivenFirst = true }, 
            Rank = Rank.Dan9 
        };
        _mockPersonService.Setup(s => s.GetPlayerProfileAsync(1)).ReturnsAsync(player);

        // Act
        var result = await _mockPersonService.Object.GetPlayerProfileAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _mockPersonService.Verify(s => s.GetPlayerProfileAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetPlayerById_GivenInvalidId_ShouldReturnNull()
    {
        // Arrange
        _mockPersonService.Setup(s => s.GetPlayerProfileAsync(999)).ReturnsAsync((Person?)null);

        // Act
        var result = await _mockPersonService.Object.GetPlayerProfileAsync(999);

        // Assert
        Assert.Null(result);
        _mockPersonService.Verify(s => s.GetPlayerProfileAsync(999), Times.Once);
    }

    [Fact]
    public async Task DeletePlayer_GivenValidId_ShouldRemovePlayer()
    {
        // Arrange
        _mockPersonService.Setup(s => s.RemovePlayerAsync(1)).Returns(Task.CompletedTask);

        // Act
        await _mockPersonService.Object.RemovePlayerAsync(1);

        // Assert
        _mockPersonService.Verify(s => s.RemovePlayerAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetPlayerTournaments_GivenValidId_ShouldReturnTournaments()
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
                Organizer = new Person { Id = 2, Name = new Name { Family = "Organizer", Given = "Bob", IsGivenFirst = true } }
            }
        };
        _mockPersonService.Setup(s => s.ListPlayerTournaments(1)).ReturnsAsync(tournaments);

        // Act
        var result = await _mockPersonService.Object.ListPlayerTournaments(1);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _mockPersonService.Verify(s => s.ListPlayerTournaments(1), Times.Once);
    }

    #endregion

    #region Tournament Endpoints

    [Fact]
    public async Task GetTournaments_ShouldReturnAllTournaments()
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
            }
        };
        _mockTournamentService.Setup(s => s.ListAllTournamentsAsync()).ReturnsAsync(tournaments);

        // Act
        var result = await _mockTournamentService.Object.ListAllTournamentsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _mockTournamentService.Verify(s => s.ListAllTournamentsAsync(), Times.Once);
    }

    [Fact]
    public async Task PostTournament_ShouldCreateTournamentAndReturnCreated()
    {
        // Arrange
        var dto = new TournamentDto 
        { 
            Name = "Spring Open",
            Location = "NYC",
            StartDate = "2026-03-15",
            EndDate = "2026-03-17",
            Ruleset = Ruleset.AGA,
            OrganizerId = 1
        };
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
        _mockTournamentService.Setup(s => s.AddTournamentAsync(dto)).ReturnsAsync(tournament);

        // Act
        var result = await _mockTournamentService.Object.AddTournamentAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Spring Open", result.Name);
        _mockTournamentService.Verify(s => s.AddTournamentAsync(dto), Times.Once);
    }

    [Fact]
    public async Task GetTournamentById_GivenValidId_ShouldReturnTournament()
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
        _mockTournamentService.Setup(s => s.GetTournamentDetailsAsync(1)).ReturnsAsync(tournament);

        // Act
        var result = await _mockTournamentService.Object.GetTournamentDetailsAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _mockTournamentService.Verify(s => s.GetTournamentDetailsAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetTournamentById_GivenInvalidId_ShouldReturnNull()
    {
        // Arrange
        _mockTournamentService.Setup(s => s.GetTournamentDetailsAsync(999)).ReturnsAsync((Tournament?)null);

        // Act
        var result = await _mockTournamentService.Object.GetTournamentDetailsAsync(999);

        // Assert
        Assert.Null(result);
        _mockTournamentService.Verify(s => s.GetTournamentDetailsAsync(999), Times.Once);
    }

    [Fact]
    public async Task DeleteTournament_GivenValidId_ShouldRemoveTournament()
    {
        // Arrange
        _mockTournamentService.Setup(s => s.RemoveTournamentAsync(1)).Returns(Task.CompletedTask);

        // Act
        await _mockTournamentService.Object.RemoveTournamentAsync(1);

        // Assert
        _mockTournamentService.Verify(s => s.RemoveTournamentAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetTournamentParticipants_GivenValidId_ShouldReturnParticipants()
    {
        // Arrange
        var participants = new List<Person>
        {
            new Person { Id = 1, Name = new Name { Family = "Redmond", Given = "Michael", IsGivenFirst = true } },
            new Person { Id = 2, Name = new Name { Family = "Lee", Given = "Sedol", IsGivenFirst = false } }
        };
        _mockTournamentService.Setup(s => s.ListTournamentParticipantsAsync(1)).ReturnsAsync(participants);

        // Act
        var result = await _mockTournamentService.Object.ListTournamentParticipantsAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockTournamentService.Verify(s => s.ListTournamentParticipantsAsync(1), Times.Once);
    }

    [Fact]
    public async Task PostTournamentParticipant_GivenValidIds_ShouldRegisterParticipant()
    {
        // Arrange
        _mockTournamentService.Setup(s => s.RegisterParticipantAsync(1, 1)).Returns(Task.CompletedTask);

        // Act
        await _mockTournamentService.Object.RegisterParticipantAsync(1, 1);

        // Assert
        _mockTournamentService.Verify(s => s.RegisterParticipantAsync(1, 1), Times.Once);
    }

    [Fact]
    public async Task DeleteTournamentParticipant_GivenValidIds_ShouldWithdrawParticipant()
    {
        // Arrange
        _mockTournamentService.Setup(s => s.WithdrawParticipantAsync(1, 1)).Returns(Task.CompletedTask);

        // Act
        await _mockTournamentService.Object.WithdrawParticipantAsync(1, 1);

        // Assert
        _mockTournamentService.Verify(s => s.WithdrawParticipantAsync(1, 1), Times.Once);
    }

    #endregion
}
