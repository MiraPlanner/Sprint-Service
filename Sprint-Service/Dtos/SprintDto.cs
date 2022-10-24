using Sprint_Service.Models;

namespace Sprint_Service.Dtos;

public record SprintDto(
    Guid Id, 
    string Name, 
    string? Goal, 
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate,
    IEnumerable<Issue>? Issues
);
