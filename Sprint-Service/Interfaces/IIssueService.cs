using Sprint_Service.Models;

namespace Sprint_Service.Interfaces;

public interface IIssueService
{
    public Task<Issue> Get(Guid id);
    public Task<IEnumerable<Issue>> GetBySprintId(Guid sprintId);
    public Task<Issue> Create(Issue issue);
    public Task<Issue> Update(Guid id, Issue issue);
    public Task<Issue?> Delete(Guid id);
}