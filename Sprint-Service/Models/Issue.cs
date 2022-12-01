using Mira_Common;

namespace Sprint_Service.Models;

public class Issue : IEntity
{
    public Guid Id { get; set; }
    public Guid? SprintId { get; set; }
    public string? Title { get; set; }
    public IssueStatus IssueStatus { get; set; }
    public IssueType IssueType { get; set; }
}