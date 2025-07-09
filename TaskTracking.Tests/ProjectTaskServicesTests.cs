using Microsoft.EntityFrameworkCore;
using Moq;
using TaskTracking.Domain.Entites.Tasks;
using TaskTracking.Domain.Entites.Tasks.Dtos;
using TaskTracking.Domain.Enums;
using TaskTracking.Domain.Intefaces;
using TaskTracking.Domain.Intefaces.Tasks;
using TaskTracking.Presistance.SQL;
using Xunit;

namespace TaskTracking.Tests
{
    public class ProjectTaskServicesTests
    {
        private TaskTrackingContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<TaskTrackingContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TaskTrackingContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldCreateTask_WhenValidTaskDtoProvided()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockTaskRepository = new Mock<IProjectTaskRepostroy>();

            mockUnitOfWork.Setup(u => u.ProjectTaskRepository).Returns(mockTaskRepository.Object);
            mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.FromResult(1));

            mockTaskRepository.Setup(r => r.AddAsync(It.IsAny<ProjectTask>()))
                .Returns(Task.FromResult(OperationResult.Success));

            var taskService = new ProjectTaskServices(mockUnitOfWork.Object);

            var taskDto = new ProjectTaskDto
            {
                Title = "Test Task",
                description = "Test Task Description",
                DueDate = DateTimeOffset.UtcNow.AddDays(7),
                Completion = DateTimeOffset.UtcNow,
                status = 1,
                UserId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid()
            };

            // Act
            var result = await taskService.AddAsync(taskDto);

            // Assert
            Assert.Equal(OperationResult.Success, result);
            mockTaskRepository.Verify(r => r.AddAsync(It.IsAny<ProjectTask>()), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTasksByProjectIdAsync_ShouldReturnProjectTasks_WhenValidProjectIdProvided()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockTaskRepository = new Mock<IProjectTaskRepostroy>();

            var projectId = Guid.NewGuid();
            var tasks = new List<ProjectTask>
            {
                new ProjectTask
                {
                    Id = Guid.NewGuid(),
                    Title = "Task 1",
                    description = "Task 1 Description",
                    DueDate = DateTimeOffset.UtcNow.AddDays(7),
                    Completion = DateTimeOffset.UtcNow,
                    status = 1,
                    ProjectId = projectId,
                    UserId = Guid.NewGuid()
                },
                new ProjectTask
                {
                    Id = Guid.NewGuid(),
                    Title = "Task 2",
                    description = "Task 2 Description",
                    DueDate = DateTimeOffset.UtcNow.AddDays(14),
                    Completion = DateTimeOffset.UtcNow,
                    status = 2,
                    ProjectId = projectId,
                    UserId = Guid.NewGuid()
                }
            };

            mockUnitOfWork.Setup(u => u.ProjectTaskRepository).Returns(mockTaskRepository.Object);
            mockTaskRepository.Setup(r => r.GetTasksByProjectIdAsync(projectId))
                .ReturnsAsync(tasks);

            var taskService = new ProjectTaskServices(mockUnitOfWork.Object);

            // Act
            var result = await taskService.GetTasksByProjectIdAsync(projectId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Task 1", result[0].Title);
            Assert.Equal("Task 2", result[1].Title);
            mockTaskRepository.Verify(r => r.GetTasksByProjectIdAsync(projectId), Times.Once);
        }

        [Fact]
        public async Task GetTasksByUserIdAsync_ShouldReturnUserTasks_WhenValidUserIdProvided()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockTaskRepository = new Mock<IProjectTaskRepostroy>();

            var userId = "550e8400-e29b-41d4-a716-446655440000"; // Valid GUID format
            var tasks = new List<ProjectTask>
            {
                new ProjectTask
                {
                    Id = Guid.NewGuid(),
                    Title = "User Task 1",
                    description = "User Task 1 Description",
                    DueDate = DateTimeOffset.UtcNow.AddDays(7),
                    Completion = DateTimeOffset.UtcNow,
                    status = 1,
                    ProjectId = Guid.NewGuid(),
                    UserId = Guid.Parse(userId)
                }
            };

            mockUnitOfWork.Setup(u => u.ProjectTaskRepository).Returns(mockTaskRepository.Object);
            mockTaskRepository.Setup(r => r.GetTasksByUserIdAsync(userId))
                .ReturnsAsync(tasks);

            var taskService = new ProjectTaskServices(mockUnitOfWork.Object);

            // Act
            var result = await taskService.GetTasksByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("User Task 1", result[0].Title);
            mockTaskRepository.Verify(r => r.GetTasksByUserIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteTask_WhenValidIdProvided()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockTaskRepository = new Mock<IProjectTaskRepostroy>();

            var taskId = Guid.NewGuid();
            var existingTask = new ProjectTask
            {
                Id = taskId,
                Title = "Existing Task",
                description = "Existing Task Description",
                DueDate = DateTimeOffset.UtcNow.AddDays(7),
                Completion = DateTimeOffset.UtcNow,
                status = 1,
                ProjectId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            mockUnitOfWork.Setup(u => u.ProjectTaskRepository).Returns(mockTaskRepository.Object);
            mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.FromResult(1));

            mockTaskRepository.Setup(r => r.FindByIdAsync(taskId))
                .ReturnsAsync(existingTask);
            mockTaskRepository.Setup(r => r.DeleteAsync(taskId))
                .Returns(Task.FromResult(OperationResult.Success));

            var taskService = new ProjectTaskServices(mockUnitOfWork.Object);

            // Act
            var result = await taskService.DeleteAsync(taskId);

            // Assert
            Assert.Equal(OperationResult.Success, result);
            mockTaskRepository.Verify(r => r.FindByIdAsync(taskId), Times.Once);
            mockTaskRepository.Verify(r => r.DeleteAsync(taskId), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockTaskRepository = new Mock<IProjectTaskRepostroy>();

            var taskId = Guid.NewGuid();

            mockUnitOfWork.Setup(u => u.ProjectTaskRepository).Returns(mockTaskRepository.Object);
            mockTaskRepository.Setup(r => r.FindByIdAsync(taskId))
                .ReturnsAsync((ProjectTask)null);

            var taskService = new ProjectTaskServices(mockUnitOfWork.Object);

            // Act
            var result = await taskService.DeleteAsync(taskId);

            // Assert
            Assert.Equal(OperationResult.NotFound, result);
            mockTaskRepository.Verify(r => r.FindByIdAsync(taskId), Times.Once);
            mockTaskRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
            mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task GetOverdueTasksAsync_ShouldReturnOverdueTasks()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockTaskRepository = new Mock<IProjectTaskRepostroy>();

            var overdueTasks = new List<ProjectTask>
            {
                new ProjectTask
                {
                    Id = Guid.NewGuid(),
                    Title = "Overdue Task",
                    description = "Overdue Task Description",
                    DueDate = DateTimeOffset.UtcNow.AddDays(-1), // Past due date
                    Completion = DateTimeOffset.UtcNow,
                    status = 1, // Not completed
                    ProjectId = Guid.NewGuid(),
                    UserId = Guid.NewGuid()
                }
            };

            mockUnitOfWork.Setup(u => u.ProjectTaskRepository).Returns(mockTaskRepository.Object);
            mockTaskRepository.Setup(r => r.GetOverdueTasksAsync())
                .ReturnsAsync(overdueTasks);

            var taskService = new ProjectTaskServices(mockUnitOfWork.Object);

            // Act
            var result = await taskService.GetOverdueTasksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Overdue Task", result[0].Title);
            mockTaskRepository.Verify(r => r.GetOverdueTasksAsync(), Times.Once);
        }
    }
}

