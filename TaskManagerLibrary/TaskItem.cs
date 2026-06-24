using System.ComponentModel.DataAnnotations;

namespace TaskManagerLibrary;

public class TaskItem
{
    public Guid Id { get; set; }
    
    private string title = string.Empty;
    
    [Required(ErrorMessage = "Название задачи обязательно")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Название должно быть от 1 до 200 символов")]
    public string Title
    {
        get => title;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Название задачи не может быть пустым");
            if (value.Length > 200)
                throw new ArgumentException("Название задачи не может превышать 200 символов");
            title = value.Trim();
        }
    }

    private string description = string.Empty;
    
    [StringLength(1000, ErrorMessage = "Описание не может превышать 1000 символов")]
    public string Description
    {
        get => description;
        set
        {
            if (value != null && value.Length > 1000)
                throw new ArgumentException("Описание не может превышать 1000 символов");
            description = value?.Trim() ?? string.Empty;
        }
    }

    public TaskPriority Priority { get; set; }
    public DateTime DueDate { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public TaskItem()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        DueDate = DateTime.Now.AddDays(7);
        Priority = TaskPriority.Medium;
        Status = TaskStatus.New;
    }

    public TaskItem(string title, string description, TaskPriority priority, DateTime dueDate, TaskStatus status)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        Status = status;
        CreatedAt = DateTime.Now;
    }
}
