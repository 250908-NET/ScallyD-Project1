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

        // TODO: Add tournaments

        Context.Persons.AddRange(player1, player2, player3);

        await Context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}
