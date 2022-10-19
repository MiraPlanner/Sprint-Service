using Mira_Common;
using Sprint_Service.Dtos;

namespace Sprint_Service.Models;

public class Sprint : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Goal { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    
    public SprintDto AsDto()
    {
        return new SprintDto(
            Id, Name, Goal, StartDate, EndDate
        );
    }
}