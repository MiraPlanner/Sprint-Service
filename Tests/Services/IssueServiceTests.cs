using System.Linq.Expressions;
using Mira_Common;
using Sprint_Service.Dtos;
using Sprint_Service.Interfaces;
using Sprint_Service.Models;
using Sprint_Service.Services;

namespace Tests.Services;

public class IssueServiceTests
{
    private readonly Mock<IRepository<Issue>> _mockRepository;

    public IssueServiceTests()
    {
        _mockRepository = new Mock<IRepository<Issue>>();
    }
    
   [Fact]
   public void GetBySprintId_ReturnsAllIssues_BelongingToSprint()
   {
       // Arrange
       Guid sprintId = new Guid("11FA647C-AD54-4BCC-A860-E5A2664B0123");

       _mockRepository.Setup(repo => repo.GetAll(It.IsAny<Expression<Func<Issue, bool>>>()))
           .ReturnsAsync(GetIssues(sprintId));

       var service = new IssueService(_mockRepository.Object);

       // Act
       var result = service.GetBySprintId(sprintId);

       // Assert
       var task = Assert.IsType<Task<IEnumerable<Issue>>>(result);
       var returnValue = task.Result;
       var issues = returnValue.ToList();
       var issue0 = issues.FirstOrDefault();
       var issue1 = issues.LastOrDefault();
       Assert.NotEmpty(issues);
       Assert.Equal("First Issue Title", issue0?.Title);
       Assert.Equal("Second Issue Title", issue1?.Title);
   }
   
   private static IReadOnlyCollection<Issue> GetIssues(Guid sprintId)
       {
           return new List<Issue>
           {
               new()
               {
                   Id = new Guid("BBFA647C-AD54-4BCC-A860-E5A2664B0456"),
                   SprintId = sprintId,
                   Title = "First Issue Title",
                   IssueStatus = IssueStatus.ToDo,
                   IssueType = IssueType.UserStory
               },
               new()
               {
                   Id = new Guid("BBFA647C-AD54-4BCC-A860-E5A2664B0123"),
                   SprintId = sprintId,
                   Title = "Second Issue Title",
                   IssueStatus = IssueStatus.ToDo,
                   IssueType = IssueType.UserStory
               }
           };
       }
}