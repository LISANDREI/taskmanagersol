using System.Text.Json;
using System.Xml.Serialization;

namespace TaskManagerLibrary;

public class TaskRepository
{
    // сохр в жсон
    public void SaveToJson(string filePath, IEnumerable<TaskItem> tasks)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));

        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
            };

            var json = JsonSerializer.Serialize(tasks, options);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ошибка при сохранении в JSON: {ex.Message}", ex);
        }
    }

    // загр из жсон
    public IEnumerable<TaskItem> LoadFromJson(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл не найден: {filePath}");

        try
        {
            var json = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions
            {
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
            };

            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json, options);
            return tasks ?? new List<TaskItem>();
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Ошибка десериализации JSON: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ошибка при загрузке из JSON: {ex.Message}", ex);
        }
    }

    // сохр в хмл
    public void SaveToXml(string filePath, IEnumerable<TaskItem> tasks)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));

        try
        {
            var serializer = new XmlSerializer(typeof(List<TaskItem>));
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, tasks.ToList());
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ошибка при сохранении в XML: {ex.Message}", ex);
        }
    }

    // загр из хмл
    public IEnumerable<TaskItem> LoadFromXml(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл не найден: {filePath}");

        try
        {
            var serializer = new XmlSerializer(typeof(List<TaskItem>));
            using var reader = new StreamReader(filePath);
            var tasks = serializer.Deserialize(reader) as List<TaskItem>;
            return tasks ?? new List<TaskItem>();
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException($"Ошибка десериализации XML: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ошибка при загрузке из XML: {ex.Message}", ex);
        }
    }
}
