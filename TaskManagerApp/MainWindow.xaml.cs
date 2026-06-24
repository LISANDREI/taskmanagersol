using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using TaskManagerLibrary;

namespace TaskManagerApp;

public partial class MainWindow : Window
{
    private TaskManager manager;
    private TaskRepository repo;

    public MainWindow()
    {
        InitializeComponent();
        manager = new TaskManager();
        repo = new TaskRepository();
        
        InitializeComboBoxes();
        RefreshTaskList();
    }

    private void InitializeComboBoxes()
    {
        StatusFilterComboBox.Items.Add("Все задачи");
        StatusFilterComboBox.Items.Add("Новая");
        StatusFilterComboBox.Items.Add("В процессе");
        StatusFilterComboBox.Items.Add("Завершена");
        StatusFilterComboBox.SelectedIndex = 0;

        SortComboBox.Items.Add("По умолчанию");
        SortComboBox.Items.Add("По приоритету");
        SortComboBox.Items.Add("По сроку");
        SortComboBox.Items.Add("По названию");
        SortComboBox.SelectedIndex = 0;
    }

    private void RefreshTaskList()
    {
        IEnumerable<TaskItem> tasks = manager.GetAllTasks();

        if (StatusFilterComboBox.SelectedIndex > 0)
        {
            TaskManagerLibrary.TaskStatus? status = StatusFilterComboBox.SelectedIndex switch
            {
                1 => TaskManagerLibrary.TaskStatus.New,
                2 => TaskManagerLibrary.TaskStatus.InProgress,
                3 => TaskManagerLibrary.TaskStatus.Completed,
                _ => null
            };
            tasks = manager.FilterByStatus(status);
        }



        
        if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
        {
            tasks = manager.Search(SearchTextBox.Text);
        }




        
        tasks = SortComboBox.SelectedIndex switch
        {
            1 => manager.SortByPriority(),
            2 => manager.SortByDueDate(),
            3 => manager.SortByTitle(),
            _ => tasks
        };

        TasksDataGrid.ItemsSource = tasks.ToList();
    }

    private void AddTaskButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new TaskDialog();
        if (dialog.ShowDialog() == true)
        {
            try
            {
                manager.AddTask(dialog.TaskItem);
                RefreshTaskList();
                MessageBox.Show("Задача успешно добавлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void EditTaskButton_Click(object sender, RoutedEventArgs e)
    {
        if (TasksDataGrid.SelectedItem is TaskItem selectedTask)
        {
            var dialog = new TaskDialog(selectedTask);
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    manager.UpdateTask(dialog.TaskItem);
                    RefreshTaskList();
                    MessageBox.Show("Задача обновлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Выберите задачу для редактирования", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
    {
        if (TasksDataGrid.SelectedItem is TaskItem selectedTask)
        {
            var result = MessageBox.Show(
                $"Удалить задачу '{selectedTask.Title}'?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                manager.DeleteTask(selectedTask.Id);
                RefreshTaskList();
                MessageBox.Show("Задача удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        else
        {
            MessageBox.Show("Выберите задачу для удаления", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshTaskList();
    }

    private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        RefreshTaskList();
    }

    private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RefreshTaskList();
    }

    private void TasksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        
    }

    private void LoadJsonButton_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
            Title = "Загрузить задачи из JSON"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                var tasks = repo.LoadFromJson(openFileDialog.FileName);
                manager.LoadTasks(tasks);
                RefreshTaskList();
                MessageBox.Show("Задачи загружены из JSON", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void SaveJsonButton_Click(object sender, RoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
            Title = "Сохранить задачи в JSON",
            FileName = "tasks.json"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            try
            {
                repo.SaveToJson(saveFileDialog.FileName, manager.GetAllTasks());
                MessageBox.Show("Задачи сохранены в JSON", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void LoadXmlButton_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
            Title = "Загрузить задачи из XML"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                var tasks = repo.LoadFromXml(openFileDialog.FileName);
                manager.LoadTasks(tasks);
                RefreshTaskList();
                MessageBox.Show("Задачи загружены из XML", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void SaveXmlButton_Click(object sender, RoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
            Title = "Сохранить задачи в XML",
            FileName = "tasks.xml"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            try
            {
                repo.SaveToXml(saveFileDialog.FileName, manager.GetAllTasks());
                MessageBox.Show("Задачи сохранены в XML", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}