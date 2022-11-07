using MassTransit;
using Mira_Contracts.IssueContracts;
using Sprint_Service.Interfaces;
using Sprint_Service.Models;

namespace Sprint_Service.Consumers;

public class IssueCreatedConsumer : IConsumer<IssueCreated>
{
    private readonly IIssueService _issueService;

    public IssueCreatedConsumer(IIssueService issueService)
    {
        _issueService = issueService;
    }

    public async Task Consume(ConsumeContext<IssueCreated> context)
    {
        var message = context.Message;

        var item = await _issueService.Get(message.IssueId);

        if (item != null) return;

        item = new Issue
        {
            Id = message.IssueId,
            SprintId = message.SprintId,
            Title = message.Title,
            IssueStatus = message.IssueStatus,
            IssueType = message.IssueType
        };

        await _issueService.Create(item);
    }
}