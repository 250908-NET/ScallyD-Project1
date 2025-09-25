using System.ComponentModel.DataAnnotations;

public class Tournament
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Location { get; set; }
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    public required Ruleset Ruleset { get; set; }
    public required Person Organizer { get; set; }
    public List<Person> Participants { get; set; } = [];
}
