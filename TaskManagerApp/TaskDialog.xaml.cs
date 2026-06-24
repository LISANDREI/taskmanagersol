using System.Windows;
using TaskManagerLibrary;
using TaskStatus = TaskManagerLibrary.TaskStatus;

namespace TaskManagerApp;

public partial class TaskDialog : Window
{
    public TaskItem TaskItem { get; private set; }
    private bool isEditMode;

    private Dictionary<string, TaskPriority> priorityMap = new()
    {
        { "Низкий", TaskPriority.Low },
        { "Средний", TaskPriority.Medium },
        { "Высокий", TaskPriority.High }
    };

    private Dictionary<string, TaskStatus> statusMap = new()
    {
        { "Новая", TaskStatus.New },
        { "В процессе", TaskStatus.InProgress },
        { "Завершена", TaskStatus.Completed }
    };

    public TaskDialog(TaskItem? existingTask = null)
    {
        InitializeComponent();
        
        isEditMode = existingTask != null;
        
        PriorityComboBox.ItemsSource = priorityMap.Keys;
        StatusComboBox.ItemsSource = statusMap.Keys;

        if (isEditMode && existingTask != null)
        {
            Title = "Редактировать задачу";
            TaskItem = existingTask;
            TitleTextBox.Text = existingTask.Title;
            DescriptionTextBox.Text = existingTask.Description;
            
            PriorityComboBox.SelectedItem = existingTask.Priority switch
            {
                TaskPriority.Low => "Низкий",
                TaskPriority.Medium => "Средний",
                TaskPriority.High => "Высокий",
                _ => "Средний"
            };
            
            StatusComboBox.SelectedItem = existingTask.Status switch
            {
                TaskStatus.New => "Новая",
                TaskStatus.InProgress => "В процессе",
                TaskStatus.Completed => "Завершена",
                _ => "Новая"
            };
            
            DueDatePicker.SelectedDate = existingTask.DueDate;
        }
        else
        {
            Title = "Новая задача";
            TaskItem = new TaskItem();
            PriorityComboBox.SelectedItem = "Средний";
            StatusComboBox.SelectedItem = "Новая";
            DueDatePicker.SelectedDate = DateTime.Now.AddDays(7);
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
        {
            MessageBox.Show("Введите название задачи", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (TitleTextBox.Text.Length > 200)
        {
            MessageBox.Show("Название не может превышать 200 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (DescriptionTextBox.Text.Length > 1000)
        {
            MessageBox.Show("Описание не может превышать 1000 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (DueDatePicker.SelectedDate == null)
        {
            MessageBox.Show("Выберите срок выполнения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var selectedPriority = priorityMap[(string)PriorityComboBox.SelectedItem];
            var selectedStatus = statusMap[(string)StatusComboBox.SelectedItem];

            if (isEditMode)
            {
                TaskItem.Title = TitleTextBox.Text;
                TaskItem.Description = DescriptionTextBox.Text;
                TaskItem.Priority = selectedPriority;
                TaskItem.Status = selectedStatus;
                TaskItem.DueDate = DueDatePicker.SelectedDate.Value;
            }
            else
            {
                TaskItem = new TaskItem(
                    TitleTextBox.Text,
                    DescriptionTextBox.Text,
                    selectedPriority,
                    DueDatePicker.SelectedDate.Value,
                    selectedStatus
                );
            }

            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
