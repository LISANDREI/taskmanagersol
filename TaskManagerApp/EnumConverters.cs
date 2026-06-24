using System.Globalization;
using System.Windows.Data;
using TaskManagerLibrary;
using TaskStatus = TaskManagerLibrary.TaskStatus;

namespace TaskManagerApp;

public class TaskStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TaskStatus status)
        {
            return status switch
            {
                TaskStatus.New => "Новая",
                TaskStatus.InProgress => "В процессе",
                TaskStatus.Completed => "Завершена",
                _ => value.ToString()
            };
        }
        return value?.ToString() ?? "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            return str switch
            {
                "Новая" => TaskStatus.New,
                "В процессе" => TaskStatus.InProgress,
                "Завершена" => TaskStatus.Completed,
                _ => TaskStatus.New
            };
        }
        return TaskStatus.New;
    }
}

public class TaskPriorityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TaskPriority priority)
        {
            return priority switch
            {
                TaskPriority.Low => "Низкий",
                TaskPriority.Medium => "Средний",
                TaskPriority.High => "Высокий",
                _ => value.ToString()
            };
        }
        return value?.ToString() ?? "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            return str switch
            {
                "Низкий" => TaskPriority.Low,
                "Средний" => TaskPriority.Medium,
                "Высокий" => TaskPriority.High,
                _ => TaskPriority.Medium
            };
        }
        return TaskPriority.Medium;
    }
}
