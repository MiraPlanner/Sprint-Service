using MassTransit;
using Mira_Contracts.IssueContracts;
using Sprint_Service.Interfaces;

namespace Sprint_Service.Consumers;

public class IssueDeletedConsumer : IConsumer<IssueDeleted>
{
    private readonly IIssueService _issueService;

    public IssueDeletedConsumer(IIssueService issueService)
    {
        _issueService = issueService;
    }

    public async Task Consume(ConsumeContext<IssueDeleted> context)
    {
        var message = context.Message;

        var issue = await _issueService.Get(message.IssueId);

        if (issue == null) return;

        await _issueService.Delete(message.IssueId);

    }
}