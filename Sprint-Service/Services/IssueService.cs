using System.Linq.Expressions;
using Mira_Common;
using MongoDB.Driver;
using Sprint_Service.Interfaces;
using Sprint_Service.Models;

namespace Sprint_Service.Services;

public class IssueService : IIssueService
{
    private readonly IRepository<Issue> _issueRepository;
    
    public IssueService(IRepository<Issue> issueRepository)
    {
        _issueRepository = issueRepository;
    }
    
    public Task<Issue> Get(Guid id)
    {
        var issue = _issueRepository.Get(id);

        return issue;
    }
    
    public async Task<IEnumerable<Issue>> GetBySprintId(Guid sprintId)
    {
        Expression<Func<Issue, bool>> filter = i => i.SprintId == sprintId;
        
        var issues = await _issueRepository.GetAll(filter);

        return issues;
    }

    public async Task<Issue> Create(Issue issue)
    {
        await _issueRepository.Create(issue);

        return issue;
    }

    public async Task<Issue> Update(Guid id, Issue issue)
    {
        var existingIssue = await _issueRepository.Get(id);

        if (existingIssue == null) return null!;

        existingIssue.SprintId = issue.SprintId;
        existingIssue.Title = issue.Title;
        existingIssue.IssueStatus = issue.IssueStatus;
        existingIssue.IssueType = issue.IssueType;
        
        await _issueRepository.Update(existingIssue);

        return existingIssue;
    }

    public async Task<Issue?> Delete(Guid id)
    {
        var issue = await _issueRepository.Get(id);

        if (issue == null) return null;

        await _issueRepository.Delete(issue.Id);

        return issue;
    }
}