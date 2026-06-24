using TaskManagerLibrary;

namespace TaskManagerLibrary.Tests;

public class TaskManagerTests
{
    [Fact]
    public void AddTask_WithValidTask_AddsToCollection()
    {
        var manager = new TaskManager();
        var task = new TaskItem("Test Task", "Description", TaskPriority.Medium, DateTime.Now, TaskStatus.New);

        manager.AddTask(task);

        Assert.Equal(1, manager.Count);
        Assert.Contains(task, manager.GetAllTasks());
    }

    [Fact]
    public void AddTask_WithEmptyTitle_ThrowsException()
    {
        var manager = new TaskManager();
        var task = new TaskItem();

        Assert.Throws<ArgumentException>(() => manager.AddTask(task));
    }

    [Fact]
    public void DeleteTask_WithExistingId_RemovesTask()
    {
        var manager = new TaskManager();
        var task = new TaskItem("Test Task", "Description", TaskPriority.Medium, DateTime.Now, TaskStatus.New);
        manager.AddTask(task);

        var result = manager.DeleteTask(task.Id);

        Assert.True(result);
        Assert.Equal(0, manager.Count);
    }

    [Fact]
    public void DeleteTask_WithNonExistingId_ReturnsFalse()
    {
        var manager = new TaskManager();

        var result = manager.DeleteTask(Guid.NewGuid());

        Assert.False(result);
    }

    [Fact]
    public void UpdateTask_WithValidData_UpdatesTask()
    {
        var manager = new TaskManager();
        var task = new TaskItem("Original", "Description", TaskPriority.Low, DateTime.Now, TaskStatus.New);
        manager.AddTask(task);

        var updatedTask = new TaskItem("Updated", "New Description", TaskPriority.High, DateTime.Now, TaskStatus.Completed);
        updatedTask.Id = task.Id;

        manager.UpdateTask(updatedTask);

        var result = manager.GetTaskById(task.Id);
        Assert.NotNull(result);
        Assert.Equal("Updated", result.Title);
        Assert.Equal(TaskPriority.High, result.Priority);
        Assert.Equal(TaskStatus.Completed, result.Status);
    }

    [Fact]
    public void FilterByStatus_ReturnsOnlyMatchingTasks()
    {
        var manager = new TaskManager();
        manager.AddTask(new TaskItem("Task 1", "Desc 1", TaskPriority.Low, DateTime.Now, TaskStatus.New));
        manager.AddTask(new TaskItem("Task 2", "Desc 2", TaskPriority.Medium, DateTime.Now, TaskStatus.Completed));
        manager.AddTask(new TaskItem("Task 3", "Desc 3", TaskPriority.High, DateTime.Now, TaskStatus.New));

        var newTasks = manager.FilterByStatus(TaskStatus.New).ToList();

        Assert.Equal(2, newTasks.Count);
        Assert.All(newTasks, t => Assert.Equal(TaskStatus.New, t.Status));
    }

    [Fact]
    public void FilterByStatus_WithNull_ReturnsAllTasks()
    {
        var manager = new TaskManager();
        manager.AddTask(new TaskItem("Task 1", "Desc 1", TaskPriority.Low, DateTime.Now, TaskStatus.New));
        manager.AddTask(new TaskItem("Task 2", "Desc 2", TaskPriority.Medium, DateTime.Now, TaskStatus.Completed));

        var allTasks = manager.FilterByStatus(null).ToList();

        Assert.Equal(2, allTasks.Count);
    }

    [Fact]
    public void Search_FindsTasksByTitle()
    {
        var manager = new TaskManager();
        manager.AddTask(new TaskItem("Buy groceries", "Milk and eggs", TaskPriority.Low, DateTime.Now, TaskStatus.New));
        manager.AddTask(new TaskItem("Fix bug", "Critical issue", TaskPriority.High, DateTime.Now, TaskStatus.InProgress));

        var results = manager.Search("bug").ToList();

        Assert.Single(results);
        Assert.Equal("Fix bug", results[0].Title);
    }

    [Fact]
    public void Search_FindsTasksByDescription()
    {
        var manager = new TaskManager();
        manager.AddTask(new TaskItem("Buy groceries", "Milk and eggs", TaskPriority.Low, DateTime.Now, TaskStatus.New));
        manager.AddTask(new TaskItem("Fix bug", "Critical issue", TaskPriority.High, DateTime.Now, TaskStatus.InProgress));

        var results = manager.Search("milk").ToList();

        Assert.Single(results);
        Assert.Equal("Buy groceries", results[0].Title);
    }

    [Fact]
    public void Search_IsCaseInsensitive()
    {
        var manager = new TaskManager();
        manager.AddTask(new TaskItem("Test Task", "Description", TaskPriority.Medium, DateTime.Now, TaskStatus.New));

        var results = manager.Search("TEST").ToList();

        Assert.Single(results);
    }

    [Fact]
    public void SortByPriority_ReturnsSortedList()
    {
        var manager = new TaskManager();
        manager.AddTask(new TaskItem("Low", "Desc", TaskPriority.Low, DateTime.Now, TaskStatus.New));
        manager.AddTask(new TaskItem("High", "Desc", TaskPriority.High, DateTime.Now, TaskStatus.New));
        manager.AddTask(new TaskItem("Medium", "Desc", TaskPriority.Medium, DateTime.Now, TaskStatus.New));

        var sorted = manager.SortByPriority().ToList();

        Assert.Equal(TaskPriority.High, sorted[0].Priority);
        Assert.Equal(TaskPriority.Medium, sorted[1].Priority);
        Assert.Equal(TaskPriority.Low, sorted[2].Priority);
    }

    [Fact]
    public void SortByDueDate_ReturnsSortedList()
    {
        var manager = new TaskManager();
        var today = DateTime.Now;
        manager.AddTask(new TaskItem("Task 1", "Desc", TaskPriority.Low, today.AddDays(5), TaskStatus.New));
        manager.AddTask(new TaskItem("Task 2", "Desc", TaskPriority.Medium, today.AddDays(1), TaskStatus.New));
        manager.AddTask(new TaskItem("Task 3", "Desc", TaskPriority.High, today.AddDays(3), TaskStatus.New));

        var sorted = manager.SortByDueDate().ToList();

        Assert.True(sorted[0].DueDate < sorted[1].DueDate);
        Assert.True(sorted[1].DueDate < sorted[2].DueDate);
    }

    [Fact]
    public void SortByTitle_ReturnsSortedList()
    {
        var manager = new TaskManager();
        manager.AddTask(new TaskItem("Zebra", "Desc", TaskPriority.Low, DateTime.Now, TaskStatus.New));
        manager.AddTask(new TaskItem("Apple", "Desc", TaskPriority.Medium, DateTime.Now, TaskStatus.New));
        manager.AddTask(new TaskItem("Mango", "Desc", TaskPriority.High, DateTime.Now, TaskStatus.New));

        var sorted = manager.SortByTitle().ToList();

        Assert.Equal("Apple", sorted[0].Title);
        Assert.Equal("Mango", sorted[1].Title);
        Assert.Equal("Zebra", sorted[2].Title);
    }
}
