using MassTransit;
using Mira_Contracts.IssueContracts;
using Sprint_Service.Interfaces;
using Sprint_Service.Models;

namespace Sprint_Service.Consumers;

public class IssueUpdatedConsumer : IConsumer<IssueUpdated>
{
    private readonly IIssueService _issueService;

    public IssueUpdatedConsumer(IIssueService issueService)
    {
        _issueService = issueService;
    }

    public async Task Consume(ConsumeContext<IssueUpdated> context)
    {
        var message = context.Message;

        var issue = await _issueService.Get(message.IssueId);

        if (issue == null) 
        {
            issue = new Issue
            {
                Id = message.IssueId,
                SprintId = message.SprintId,
                Title = message.Title,
                IssueStatus = message.IssueStatus,
                IssueType = message.IssueType
            };

            await _issueService.Create(issue);
        }
        
        else
        {
            issue.SprintId = message.SprintId;
            issue.Title = message.Title;
            issue.IssueStatus = message.IssueStatus;
            issue.IssueType = message.IssueType;
        }
       
    }
}