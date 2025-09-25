public class TournamentDto
{
    public required string Name { get; set; }
    public required string Location { get; set; }
    public required string StartDate { get; set; }
    public required string EndDate { get; set; }
    public required Ruleset Ruleset { get; set; }
    public required int OrganizerId { get; set; }
}
