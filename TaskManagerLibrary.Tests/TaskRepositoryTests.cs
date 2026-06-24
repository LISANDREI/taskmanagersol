using TaskManagerLibrary;

namespace TaskManagerLibrary.Tests;

public class TaskRepositoryTests
{
    private readonly string testJsonPath = Path.Combine(Path.GetTempPath(), "test_tasks.json");
    private readonly string testXmlPath = Path.Combine(Path.GetTempPath(), "test_tasks.xml");

    [Fact]
    public void SaveToJson_AndLoadFromJson_RoundTrip_Success()
    {
        var repository = new TaskRepository();
        var tasks = new List<TaskItem>
        {
            new TaskItem("Task 1", "Description 1", TaskPriority.High, DateTime.Now, TaskStatus.New),
            new TaskItem("Task 2", "Description 2", TaskPriority.Low, DateTime.Now.AddDays(2), TaskStatus.Completed)
        };

        try
        {
            repository.SaveToJson(testJsonPath, tasks);
            var loadedTasks = repository.LoadFromJson(testJsonPath).ToList();

            Assert.Equal(2, loadedTasks.Count);
            Assert.Equal(tasks[0].Title, loadedTasks[0].Title);
            Assert.Equal(tasks[0].Priority, loadedTasks[0].Priority);
            Assert.Equal(tasks[1].Title, loadedTasks[1].Title);
            Assert.Equal(tasks[1].Status, loadedTasks[1].Status);
        }
        finally
        {
            if (File.Exists(testJsonPath))
                File.Delete(testJsonPath);
        }
    }

    [Fact]
    public void SaveToXml_AndLoadFromXml_RoundTrip_Success()
    {
        var repository = new TaskRepository();
        var tasks = new List<TaskItem>
        {
            new TaskItem("Task 1", "Description 1", TaskPriority.Medium, DateTime.Now, TaskStatus.InProgress),
            new TaskItem("Task 2", "Description 2", TaskPriority.High, DateTime.Now.AddDays(3), TaskStatus.New)
        };

        try
        {
            repository.SaveToXml(testXmlPath, tasks);
            var loadedTasks = repository.LoadFromXml(testXmlPath).ToList();

            Assert.Equal(2, loadedTasks.Count);
            Assert.Equal(tasks[0].Title, loadedTasks[0].Title);
            Assert.Equal(tasks[0].Priority, loadedTasks[0].Priority);
            Assert.Equal(tasks[1].Description, loadedTasks[1].Description);
        }
        finally
        {
            if (File.Exists(testXmlPath))
                File.Delete(testXmlPath);
        }
    }

    [Fact]
    public void LoadFromJson_WithNonExistentFile_ThrowsException()
    {
        var repository = new TaskRepository();

        Assert.Throws<FileNotFoundException>(() => repository.LoadFromJson("nonexistent.json"));
    }

    [Fact]
    public void LoadFromXml_WithNonExistentFile_ThrowsException()
    {
        var repository = new TaskRepository();

        Assert.Throws<FileNotFoundException>(() => repository.LoadFromXml("nonexistent.xml"));
    }

    [Fact]
    public void LoadFromJson_WithInvalidJson_ThrowsException()
    {
        var repository = new TaskRepository();
        var invalidJsonPath = Path.Combine(Path.GetTempPath(), "invalid.json");

        try
        {
            File.WriteAllText(invalidJsonPath, "{ invalid json content");

            Assert.Throws<InvalidOperationException>(() => repository.LoadFromJson(invalidJsonPath));
        }
        finally
        {
            if (File.Exists(invalidJsonPath))
                File.Delete(invalidJsonPath);
        }
    }
}
