using Mira_Common;
using Sprint_Service.Dtos;
using Sprint_Service.Interfaces;
using Sprint_Service.Models;
using Sprint_Service.Services;

namespace Tests.Services;

public class SprintServiceTests
{
    private readonly Mock<IIssueService> _issueService;
    private readonly Mock<IRepository<Sprint>> _mockRepository;

    public SprintServiceTests()
    {
        _issueService = new Mock<IIssueService>();
        _mockRepository = new Mock<IRepository<Sprint>>();
    }
    
   [Fact]
   public void GetAll_ReturnsAllSprints()
   {
       // Arrange
       Guid sprint0Id = new Guid("11FA647C-AD54-4BCC-A860-E5A2664B0123");

       _mockRepository.Setup(repo => repo.GetAll())
           .ReturnsAsync(GetSprints());
       _issueService.Setup(service => service.GetBySprintId(sprint0Id))
           .ReturnsAsync(GetIssues(sprint0Id));
       var service = new SprintService(_mockRepository.Object, _issueService.Object);

       // Act
       var result = service.GetAll();

       // Assert
       var task = Assert.IsType<Task<IEnumerable<SprintDto>>>(result);
       var returnValue = task.Result;
       var sprintDtos = returnValue.ToList();
       var sprint0 = sprintDtos.First();
       var sprint1 = sprintDtos.Last();
       Assert.Equal("First Sprint", sprint0.Name);
       Assert.Equal("Second Sprint", sprint1.Name);
       Assert.IsType<List<Issue>>(sprint0.Issues);
       Assert.Single(sprint0.Issues!);
       Assert.Empty(sprint1.Issues!);
   }

   [Fact]
   public void GetById_ReturnsSprintWithIssues()
   {
       // Arrange
       Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
       _mockRepository.Setup(repo => repo.Get(testSessionGuid))
           .ReturnsAsync(GetSprint(testSessionGuid));
       _issueService.Setup(service => service.GetBySprintId(testSessionGuid))
           .ReturnsAsync(GetIssues(testSessionGuid));
       var service = new SprintService(_mockRepository.Object, _issueService.Object);

       // Act
       var result = service.GetById(testSessionGuid);

       // Assert
       var task = Assert.IsType<Task<SprintDto>>(result);
       var sprint = Assert.IsType<SprintDto>(task.Result);
       Assert.Equal("Sprint Title", sprint.Name);
       Assert.NotNull(sprint.Issues);
       Assert.Equal("Issue Title", sprint.Issues!.FirstOrDefault()!.Title);
   }
   
   [Fact]
   public void GetById_ReturnsNull_WhenSprintNotFound()
   {
       // Arrange
       Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");

       _mockRepository.Setup(repo => repo.Get(testSessionGuid))
           .ReturnsAsync((Sprint) null!);
       _issueService.Setup(service => service.GetBySprintId(testSessionGuid))
           .ReturnsAsync(GetIssues(testSessionGuid));
       var service = new SprintService(_mockRepository.Object, _issueService.Object);

       // Act
       var result = service.GetById(testSessionGuid);

       // Assert
       var task = Assert.IsType<Task<SprintDto>>(result);
       Assert.Null(task.Result);
   }

   private static Sprint GetSprint(Guid id)
    {
        return new Sprint
        {
            Id = id,
            Name = "Sprint Title",
            Goal = "Sprint Goal",
            StartDate = DateTimeOffset.Now,
            EndDate = DateTimeOffset.Now + TimeSpan.FromDays(14),
        };
    }
   
   private static IReadOnlyCollection<Sprint> GetSprints()
   {
       List<Sprint> sprintDtos = new List<Sprint>
       {
           new()
           {
               Id = new Guid("11FA647C-AD54-4BCC-A860-E5A2664B0123"),
               Name = "First Sprint",
               Goal = "Analysis Phase",
               StartDate = DateTimeOffset.Now,
               EndDate = DateTimeOffset.Now + TimeSpan.FromDays(14),
           },
           new()
           {
               Id = new Guid("11FA647C-AD54-4BCC-A860-E5A2664B0456"),
               Name = "Second Sprint",
               Goal = "Design Phase",
               StartDate = DateTimeOffset.Now,
               EndDate = DateTimeOffset.Now + TimeSpan.FromDays(21),
           }
       };

       return sprintDtos;
   }
   
   private static IEnumerable<Issue> GetIssues(Guid sprintId)
       {
           return new List<Issue>
           {
               new()
               {
                   Id = new Guid("BBFA647C-AD54-4BCC-A860-E5A2664B0456"),
                   SprintId = sprintId,
                   Title = "Issue Title",
                   IssueStatus = IssueStatus.ToDo,
                   IssueType = IssueType.UserStory
               }
           };
       }
}