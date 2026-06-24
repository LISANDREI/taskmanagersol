namespace TaskManagerLibrary;

public class TaskManager
{
    private List<TaskItem> tasks;

    public TaskManager()
    {
        tasks = new List<TaskItem>();
    }

    public IReadOnlyList<TaskItem> GetAllTasks() => tasks.AsReadOnly();

    public void AddTask(TaskItem task)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));
        
        if (string.IsNullOrWhiteSpace(task.Title))
            throw new ArgumentException("Название задачи не может быть пустым");

        tasks.Add(task);
    }

    public TaskItem? GetTaskById(Guid id)
    {
        return tasks.FirstOrDefault(t => t.Id == id);
    }

    public void UpdateTask(TaskItem updatedTask)
    {
        if (updatedTask == null)
            throw new ArgumentNullException(nameof(updatedTask));

        if (string.IsNullOrWhiteSpace(updatedTask.Title))
            throw new ArgumentException("Название задачи не может быть пустым");

        var existingTask = GetTaskById(updatedTask.Id);
        if (existingTask == null)
            throw new InvalidOperationException($"Задача с ID {updatedTask.Id} не найдена");

        var index = tasks.IndexOf(existingTask);
        tasks[index] = updatedTask;
    }

    public bool DeleteTask(Guid id)
    {
        var task = GetTaskById(id);
        if (task == null)
            return false;

        return tasks.Remove(task);
    }

    // фильтрации
    public IEnumerable<TaskItem> FilterByStatus(TaskStatus? status)
    {
        if (status == null)
            return tasks;

        return tasks.Where(t => t.Status == status.Value);
    }
    public IEnumerable<TaskItem> Search(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return tasks;

        searchText = searchText.ToLower();
        return tasks.Where(t =>
            t.Title.ToLower().Contains(searchText) ||
            t.Description.ToLower().Contains(searchText));
    }
    public IEnumerable<TaskItem> SortByPriority()
    {
        return tasks.OrderByDescending(t => t.Priority);
    }
    public IEnumerable<TaskItem> SortByDueDate()
    {
        return tasks.OrderBy(t => t.DueDate);
    }
    public IEnumerable<TaskItem> SortByTitle()
    {
        return tasks.OrderBy(t => t.Title);
    }
    //



    public void LoadTasks(IEnumerable<TaskItem> loadedTasks)
    {
        tasks.Clear();
        if (loadedTasks != null)
        {
            tasks.AddRange(loadedTasks);
        }
    }

    public void Clear()
    {
        tasks.Clear();
    }

    public int Count => tasks.Count;
}
