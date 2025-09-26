using Microsoft.EntityFrameworkCore;

public class TournamentRepository : ITournamentRepository
{
    private readonly AppDbContext _context;

    public TournamentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Tournament>> GetAllAsync()
    {
        return await _context.Tournaments
            .Include(t => t.Organizer)
            .ToListAsync();
    }

    public async Task<Tournament?> GetByIdAsync(int id)
    {
        return await _context.Tournaments
            .Include(t => t.Organizer)
            .Include(t => t.Participants)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddAsync(Tournament tournament)
    {
        _context.Tournaments.Add(tournament);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tournament tournament)
    {
        _context.Tournaments.Update(tournament);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Tournament tournament)
    {
        _context.Tournaments.Remove(tournament);
        await _context.SaveChangesAsync();
    }
}
