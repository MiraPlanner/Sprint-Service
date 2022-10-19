using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Sprint_Service.Dtos;

public record CreateSprintDto(
    [Required] string Name, 
    [Optional] string? Goal, 
    [Optional] DateTimeOffset? StartDate,
    [Optional] DateTimeOffset? EndDate
);