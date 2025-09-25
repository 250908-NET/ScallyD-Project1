namespace GoTournamentPlanner.Tests;

using Microsoft.EntityFrameworkCore;

public class TestBase : IDisposable
{
    protected AppDbContext Context { get; private set; }

    public TestBase()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new AppDbContext(options);
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }

    protected async Task SeedDataAsync()
    {
        Person player1 = new Person
        {
            // Id = 1,
            Name = new Name { Family = "Redmond", Given = "Michael", IsGivenFirst = true },
            Rank = Rank.Dan9
        };
        Person player2 = new Person
        {
            // Id = 1,
            Name = new Name { Family = "Lee", Given = "Sedol", IsGivenFirst = false },
            Rank = Rank.Dan9
        };
        Person player3 = new Person
        {
            // Id = 1,
            Name = new Name { Family = "Randomlastname", Given = "Bob", IsGivenFirst = true },
            Rank = Rank.Kyuu10,
            Email = "bob@therandomlastnamefamily.net"
        };

        Tournament tournament1 = new Tournament
        {
            Name = "Spring Open 2026",
            Location = "Baltimore, MD",
            StartDate = new DateOnly(2026, 3, 15),
            EndDate = new DateOnly(2026, 3, 17),
            Ruleset = Ruleset.AGA,
            Organizer = player3,
        };
        Tournament tournament2 = new Tournament
        {
            Name = "Summer Open 2020",
            Location = "Washington, D.C.",
            StartDate = new DateOnly(2020, 6, 20),
            EndDate = new DateOnly(2020, 6, 22),
            Ruleset = Ruleset.Japanese,
            Organizer = player3,
            Participants = new List<Person> { player1, player2 }
        };

        Context.Persons.AddRange(player1, player2, player3);
        Context.Tournaments.AddRange(tournament1, tournament2);

        await Context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}
