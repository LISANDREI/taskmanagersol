using TaskManagerLibrary;

namespace TaskManagerLibrary.Tests;

public class TaskItemTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesTask()
    {
        var task = new TaskItem(
            "Test Task",
            "Test Description",
            TaskPriority.High,
            DateTime.Now.AddDays(5),
            TaskStatus.New
        );

        Assert.Equal("Test Task", task.Title);
        Assert.Equal("Test Description", task.Description);
        Assert.Equal(TaskPriority.High, task.Priority);
        Assert.Equal(TaskStatus.New, task.Status);
        Assert.NotEqual(Guid.Empty, task.Id);
    }

    [Fact]
    public void Title_WhenEmpty_ThrowsException()
    {
        var task = new TaskItem();

        Assert.Throws<ArgumentException>(() => task.Title = "");
        Assert.Throws<ArgumentException>(() => task.Title = "   ");
    }

    [Fact]
    public void Title_WhenTooLong_ThrowsException()
    {
        var task = new TaskItem();
        var longTitle = new string('a', 201);

        Assert.Throws<ArgumentException>(() => task.Title = longTitle);
    }

    [Fact]
    public void Description_WhenTooLong_ThrowsException()
    {
        var task = new TaskItem();
        var longDescription = new string('a', 1001);

        Assert.Throws<ArgumentException>(() => task.Description = longDescription);
    }

    [Fact]
    public void DefaultConstructor_SetsDefaultValues()
    {
        var task = new TaskItem();

        Assert.Equal(TaskPriority.Medium, task.Priority);
        Assert.Equal(TaskStatus.New, task.Status);
        Assert.NotEqual(Guid.Empty, task.Id);
        Assert.True(task.DueDate > DateTime.Now);
    }
}
