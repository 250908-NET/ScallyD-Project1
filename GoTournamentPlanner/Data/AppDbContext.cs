using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Tournament> Tournaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasMany(p => p.ParticipatingIn)
            .WithMany(t => t.Participants)
            .UsingEntity("TournamentParticipants");

        modelBuilder.Entity<Tournament>()
            .HasOne(t => t.Organizer)
            .WithMany()
            .HasForeignKey("OrganizerId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
