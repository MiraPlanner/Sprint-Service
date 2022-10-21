﻿using Mira_Common;
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
    
    public Task<Issue> GetBySprintId(Guid sprintId)
    {
        throw new NotImplementedException();
    }

    public async Task<Issue> Create(Issue issue)
    {
        await _issueRepository.Create(issue);

        return issue;
    }

    public async Task<Issue> Update(Guid id, Issue updatedIssue)
    {
        var issue = await _issueRepository.Get(id);

        if (issue == null) return null!;

        issue.Title = updatedIssue.Title;
        issue.IssueStatus = updatedIssue.IssueStatus;
        issue.IssueType = updatedIssue.IssueType;
        
        await _issueRepository.Update(issue);

        return issue;
    }

    public async Task<Issue> Delete(Guid id)
    {
        var issue = await _issueRepository.Get(id);

        if (issue == null) return null;

        await _issueRepository.Delete(issue.Id);

        return issue;
    }
}