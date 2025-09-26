using Microsoft.EntityFrameworkCore;

public class PersonRepository : IPersonRepository
{
    private readonly AppDbContext _context;

    public PersonRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Person>> GetAllAsync()
    {
        return await _context.Persons.ToListAsync();
    }

    public async Task<Person?> GetByIdAsync(int id)
    {
        return await _context.Persons.Include(p => p.ParticipatingIn).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Person person)
    {
        _context.Persons.Add(person);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Person person)
    {
        _context.Persons.Update(person);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Person person)
    {
        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
    }
}
