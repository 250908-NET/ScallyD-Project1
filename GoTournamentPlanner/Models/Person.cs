using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Person
{
    [Key]
    public int Id { get; set; }
    public required Name Name { get; set; }
    public string? Email { get; set; }
    public Rank? Rank { get; set; }
    public List<Tournament> ParticipatingIn { get; set; } = [];
}

[Owned]
public class Name
{
    public required string Family { get; set; }
    public required string Given { get; set; }
    public required bool IsGivenFirst { get; set; }
}
