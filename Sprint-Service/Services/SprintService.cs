using Mira_Common;
using Sprint_Service.Dtos;
using Sprint_Service.Interfaces;
using Sprint_Service.Models;

namespace Sprint_Service.Services;

public class SprintService : ISprintService
{
    private readonly IRepository<Sprint> _sprintRepository;
    private readonly IIssueService _issueService;

    public SprintService(IRepository<Sprint> sprintRepository, IIssueService issueService)
    {
        _sprintRepository = sprintRepository;
        _issueService = issueService;
    }
    
    public async Task<IEnumerable<SprintDto>> GetAll()
    {
        var sprints = (await _sprintRepository.GetAll());
        var sprintDtos = new List<SprintDto>();
        
        foreach (var sprint in sprints.ToList())
        {
            sprint.Issues =  await _issueService.GetBySprintId(sprint.Id);
            sprintDtos.Add(sprint.AsDto());
        }
        
        return sprintDtos;
    }

    public async Task<SprintDto> GetById(Guid id)
    {
        var sprint = await _sprintRepository.Get(id);
        var issues = await _issueService.GetBySprintId(id);

        if (sprint == null) return null!;
        sprint.Issues = issues;

        return sprint.AsDto();
    }

    public async Task<SprintDto> Create(CreateSprintDto createSprintDto)
    {
        var sprint = new Sprint
        {
            Name = createSprintDto.Name,
            Goal = createSprintDto.Goal,
            StartDate = createSprintDto.StartDate,
            EndDate = createSprintDto.StartDate
        };

        await _sprintRepository.Create(sprint);

        return sprint.AsDto();
    }

    public async Task<SprintDto?> Update(Guid id, UpdateSprintDto updateSprintDto)
    {
        var sprint = await _sprintRepository.Get(id);

        if (sprint == null) return null!;

        sprint.Name = updateSprintDto.Name;
        sprint.Goal = updateSprintDto.Goal;
        sprint.StartDate = updateSprintDto.StartDate;
        sprint.EndDate = updateSprintDto.EndDate;
        
        await _sprintRepository.Update(sprint);

        return sprint.AsDto();
    }

    public async Task<SprintDto?> Delete(Guid id)
    {
        var sprint = await _sprintRepository.Get(id);

        if (sprint == null) return null;

        await _sprintRepository.Delete(sprint.Id);

        return sprint.AsDto();
    }
}