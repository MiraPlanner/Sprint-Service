using Mira_Common;

namespace Sprint_Service.Models;

public class Issue : IEntity
{
    public Guid Id { get; set; }
    public IssueType IssueType { get; set; }
}