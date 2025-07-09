using Microsoft.EntityFrameworkCore;
using Moq;
using TaskTracking.Domain.Entites.Projects;
using TaskTracking.Domain.Entites.Projects.Dtos;
using TaskTracking.Domain.Enums;
using TaskTracking.Domain.Intefaces;
using TaskTracking.Domain.Intefaces.Projects;
using TaskTracking.Presistance.SQL;
using TaskTracking.Services.Services.Projects;
using Xunit;

namespace TaskTracking.Tests
{
    public class ProjectServicesTests
    {
        private TaskTrackingContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<TaskTrackingContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TaskTrackingContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldCreateProject_WhenValidProjectDtoProvided()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockProjectRepository = new Mock<IProjectRepository>();

            mockUnitOfWork.Setup(u => u.ProjectRepository).Returns(mockProjectRepository.Object);
            mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.FromResult(1));

            mockProjectRepository.Setup(r => r.AddAsync(It.IsAny<Project>()))
                .Returns(Task.FromResult(OperationResult.Success));
            
            mockProjectRepository.Setup(r => r.CreateUserProjectAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(OperationResult.Success));

            var projectService = new ProjectServices(mockUnitOfWork.Object);

            var projectDto = new ProjectDto
            {
                Title = "Test Project",
                description = "Test Description",
                DueDate = DateTimeOffset.UtcNow.AddDays(30),
                Completion = DateTimeOffset.UtcNow,
                status = 1
            };

            var userId = "test-user-id";

            // Act
            var result = await projectService.AddAsync(projectDto, userId);

            // Assert
            Assert.Equal(OperationResult.Success, result);
            mockProjectRepository.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
            mockProjectRepository.Verify(r => r.CreateUserProjectAsync(userId, It.IsAny<Guid>()), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetProjectsByUserIdAsync_ShouldReturnUserProjects_WhenValidUserIdProvided()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockProjectRepository = new Mock<IProjectRepository>();

            var projects = new List<Project>
            {
                new Project
                {
                    Id = Guid.NewGuid(),
                    Title = "Project 1",
                    description = "Description 1",
                    DueDate = DateTimeOffset.UtcNow.AddDays(30),
                    Completion = DateTimeOffset.UtcNow,
                    status = 1
                },
                new Project
                {
                    Id = Guid.NewGuid(),
                    Title = "Project 2",
                    description = "Description 2",
                    DueDate = DateTimeOffset.UtcNow.AddDays(60),
                    Completion = DateTimeOffset.UtcNow,
                    status = 2
                }
            };

            mockUnitOfWork.Setup(u => u.ProjectRepository).Returns(mockProjectRepository.Object);
            mockProjectRepository.Setup(r => r.GetProjectsByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync(projects);

            var projectService = new ProjectServices(mockUnitOfWork.Object);
            var userId = "test-user-id";

            // Act
            var result = await projectService.GetProjectsByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Project 1", result[0].Title);
            Assert.Equal("Project 2", result[1].Title);
            mockProjectRepository.Verify(r => r.GetProjectsByUserIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteProject_WhenValidIdProvided()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockProjectRepository = new Mock<IProjectRepository>();

            mockUnitOfWork.Setup(u => u.ProjectRepository).Returns(mockProjectRepository.Object);
            mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.FromResult(1));

            mockProjectRepository.Setup(r => r.DeleteAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(OperationResult.Success));

            var projectService = new ProjectServices(mockUnitOfWork.Object);
            var projectId = Guid.NewGuid();

            // Act
            var result = await projectService.DeleteAsync(projectId);

            // Assert
            Assert.Equal(OperationResult.Success, result);
            mockProjectRepository.Verify(r => r.DeleteAsync(projectId), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task IsUserInProjectAsync_ShouldReturnTrue_WhenUserIsInProject()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockProjectRepository = new Mock<IProjectRepository>();

            mockUnitOfWork.Setup(u => u.ProjectRepository).Returns(mockProjectRepository.Object);
            mockProjectRepository.Setup(r => r.IsUserInProjectAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var projectService = new ProjectServices(mockUnitOfWork.Object);
            var userId = "test-user-id";
            var projectId = Guid.NewGuid();

            // Act
            var result = await projectService.IsUserInProjectAsync(userId, projectId);

            // Assert
            Assert.True(result);
            mockProjectRepository.Verify(r => r.IsUserInProjectAsync(userId, projectId), Times.Once);
        }
    }
}

