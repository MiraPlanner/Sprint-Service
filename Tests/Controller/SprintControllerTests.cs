using Microsoft.AspNetCore.Mvc;
using Sprint_Service.Controllers;
using Sprint_Service.Dtos;
using Sprint_Service.Interfaces;
using Sprint_Service.Models;

namespace Tests.Controller;

public class SprintControllerTests
{
    private readonly Mock<ISprintService> _mockService;
    
    public SprintControllerTests()
    {
        _mockService = new Mock<ISprintService>();
    }

    [Fact]
    public void GetAll_ReturnsAllSprints()
    {
        // Arrange
        _mockService.Setup(service => service.GetAll())
            .ReturnsAsync(GetSprintDtos());
        var controller = new SprintController(_mockService.Object);

        // Act
        var result = controller.GetAll();

        // Assert
        var task = Assert.IsType<Task<ActionResult<IEnumerable<SprintDto>>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(task.Result.Result);
        var returnValue = Assert.IsType<List<SprintDto>>(okResult.Value);
        var sprint0 = returnValue.First();
        var sprint1 = returnValue.Last();
        Assert.Equal("Sprint 0", sprint0.Name);
        Assert.Equal("Sprint 1", sprint1.Name);
        Assert.IsType<List<Issue>>(sprint0.Issues);
        Assert.Single(sprint0.Issues!);
        Assert.Null(sprint1.Issues!);
    }

    [Fact]
    public void GetAll_ReturnsNoSprints_WhenSprintsNotFound()
    {
        // Arrange
        _mockService.Setup(service => service.GetAll())
            .ReturnsAsync(new List<SprintDto>());
        var controller = new SprintController(_mockService.Object);

        // Act
        var result = controller.GetAll();

        // Assert
        var task = Assert.IsType<Task<ActionResult<IEnumerable<SprintDto>>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(task.Result.Result);
        var returnValue = Assert.IsType<List<SprintDto>>(okResult.Value);
        Assert.Empty(returnValue);
    }
    
    [Fact]
    public void GetById_ReturnsSprint()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");

        _mockService.Setup(service => service.GetById(testSessionGuid))
            .ReturnsAsync(GetSprintDto(testSessionGuid));
        var controller = new SprintController(_mockService.Object);

        // Act
        var result = controller.GetById(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<ActionResult<SprintDto>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(task.Result.Result);
        var sprint = Assert.IsType<SprintDto>(okResult.Value);
        Assert.Equal(testSessionGuid, sprint.Id);
        Assert.Equal("Sprint Title", sprint.Name);
        Assert.Equal("Sprint Goal", sprint.Goal);
        Assert.NotNull(sprint.Issues);
        Assert.Single(sprint.Issues!);

    }

    [Fact]
    public void GetById_ReturnsNotFound_WhenSprintNotFound()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
        var mockService = new Mock<ISprintService>();
        mockService.Setup(service => service.GetById(testSessionGuid))
            .ReturnsAsync((SprintDto) null!);
        var controller = new SprintController(mockService.Object);

        // Act
        var result = controller.GetById(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<ActionResult<SprintDto>>>(result);
        var actionResult = task.Result;
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }
    
    private static SprintDto GetSprintDto(Guid id)
    {
        return new Sprint
        {
            Id = id,
            Name = "Sprint Title",
            Goal = "Sprint Goal",
            StartDate = DateTimeOffset.Now,
            EndDate = DateTimeOffset.Now + TimeSpan.FromDays(14),
            Issues = GetIssues(id)
        }.AsDto();
    }

    private static IEnumerable<SprintDto> GetSprintDtos()
    {
        List<SprintDto> sprintDtos = new List<SprintDto>
        {
            new Sprint
            {
                Id = new Guid("AAFA647C-AD54-4BCC-A860-E5A2664B0123"),
                Name = "Sprint 0",
                Goal = "Project Analysis",
                StartDate = DateTimeOffset.Now,
                EndDate = DateTimeOffset.Now + TimeSpan.FromDays(14),
                Issues = GetIssues(new Guid("AAFA647C-AD54-4BCC-A860-E5A2664B0123"))
            }.AsDto(),
            new Sprint
            {
                Id = new Guid("AAFA647C-AD54-4BCC-A860-E5A2664B0456"),
                Name = "Sprint 1",
                Goal = "Design",
                StartDate = DateTimeOffset.Now,
                EndDate = DateTimeOffset.Now + TimeSpan.FromDays(21),
                Issues = null
            }.AsDto()
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
                Title = "Title",
                IssueStatus = IssueStatus.ToDo,
                IssueType = IssueType.UserStory
            }
        };
    }
}