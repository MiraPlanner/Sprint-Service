using Sprint_Service.Dtos;
using Sprint_Service.Models;

namespace Sprint_Service.Interfaces;

public interface ISprintService
{
    public Task<IEnumerable<SprintDto>> GetAll();
    public Task<IEnumerable<Issue>> GetBacklog();
    public Task<SprintDto> GetById(Guid id);
    public Task<SprintDto> Create(CreateSprintDto createSprintDto);
    public Task<SprintDto?> Update(Guid id, UpdateSprintDto updateSprintDto);
    public Task<SprintDto?> Delete(Guid id);
}