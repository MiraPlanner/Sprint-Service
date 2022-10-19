using Mira_Common;
using Sprint_Service.Dtos;
using Sprint_Service.Interfaces;
using Sprint_Service.Models;

namespace Sprint_Service.Services;

public class SprintService : ISprintService
{
    private readonly IRepository<Sprint> _sprintRepository;

    public SprintService(IRepository<Sprint> sprintRepository)
    {
        _sprintRepository = sprintRepository;
    }
    
    public async Task<IEnumerable<SprintDto>> GetAll()
    {
        var sprints = (await _sprintRepository.GetAll()).Select(sprint => sprint.AsDto());

        return sprints;
    }

    public async Task<SprintDto> GetById(Guid id)
    {
        var sprint = await _sprintRepository.Get(id);

        if (sprint == null) return null!;
        
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

    public async Task<SprintDto?> Update(Guid id, UpdateSprintDto updateIssueDto)
    {
        var sprint = await _sprintRepository.Get(id);

        if (sprint == null) return null!;

        sprint.Name = updateIssueDto.Name;
        sprint.Goal = updateIssueDto.Goal;
        sprint.StartDate = updateIssueDto.StartDate;
        sprint.EndDate = updateIssueDto.EndDate;
        
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