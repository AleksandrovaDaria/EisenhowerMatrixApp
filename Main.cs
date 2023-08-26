using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper;
using System.Globalization;
using EisenhowerMatrixApp;

class TodoItem
{
    private string Title { get; set; }
    private DateTime Deadline { get; set; }
    public bool IsDone { get; set; }
    public bool IsImportant { get; set; }

    public TodoItem(string title, DateTime deadline, bool isImportant)
    {
        Title = title;
        Deadline = deadline;
        IsDone = false;
        IsImportant = isImportant;
    }

    public string GetTitle()
    {
        return Title;
    }

    public DateTime GetDeadline()
    {
        return Deadline;
    }

    public void Mark()
    {
        IsDone = true;
    }

    public void Unmark()
    {
        IsDone = false;
    }

    public override string ToString()
    {
        string status = IsDone ? "[x]" : "[ ]";
        return $"{status} {Deadline:dd-MM} {Title}";
    }
}

class TodoQuarter
{
    private List<TodoItem> TodoItems { get; set; }

    public TodoQuarter()
    {
        TodoItems = new List<TodoItem>();
    }

    public void AddItem(string title, DateTime deadline, bool isImportant)
    {
        TodoItems.Add(new TodoItem(title, deadline, isImportant));
    }

    public void RemoveItem(int index)
    {
        if (index >= 0 && index < TodoItems.Count)
        {
            TodoItems.RemoveAt(index);
        }
    }

    public void ArchiveItems()
    {
        TodoItems.RemoveAll(item => item.IsDone);
    }

    public TodoItem GetItem(int index)
    {
        if (index >= 0 && index < TodoItems.Count)
        {
            return TodoItems[index];
        }
        return null;
    }

    public List<TodoItem> GetItems()
    {
        return TodoItems;
    }

    public override string ToString()
    {
        string[] items = new string[TodoItems.Count];

        for (int i = 0; i < TodoItems.Count; i++)
        {
            items[i] = $"{i + 1}. {TodoItems[i]}";
        }

        return string.Join("\n", items);
    }
}

class TodoMatrix
{
    private Dictionary<string, TodoQuarter> TodoQuarters { get; set; }

    public TodoMatrix()
    {
        TodoQuarters = new Dictionary<string, TodoQuarter>
        {
            {"IU", new TodoQuarter()},
            {"IN", new TodoQuarter()},
            {"NU", new TodoQuarter()},
            {"NN", new TodoQuarter()}
        };
    }

    public TodoQuarter GetQuarter(string status)
    {
        if (TodoQuarters.ContainsKey(status))
        {
            return TodoQuarters[status];
        }
        return null;
    }

    public void AddItem(string title, DateTime deadline, bool isImportant = false)
    {
        string quarterKey = isImportant ? (deadline <= DateTime.Now.AddDays(3) ? "IU" : "IN") : (deadline <= DateTime.Now.AddDays(3) ? "NU" : "NN");
        TodoQuarter quarter = TodoQuarters[quarterKey];
        quarter.AddItem(title, deadline,isImportant);
    }

   public void AddItemsFromFile(string fileName)
    {
        string[] lines = File.ReadAllLines(fileName);

        foreach (string line in lines)
        {
            string[] parts = line.Split('|'); // Use '|' as the delimiter
            if (parts.Length >= 2)
            {
                string title = parts[0];
                DateTime deadline;
                if (DateTime.TryParseExact(parts[1], "dd-MM", null, System.Globalization.DateTimeStyles.None, out deadline))
                {
                     bool isImportant;
                    bool.TryParse(parts[2], out isImportant);
                    AddItem(title, deadline, isImportant);
                }
            }
        }
    }
  
    public void SaveItemsToFile(string fileName)
    {
        List<string> lines = new List<string>();

        foreach (var quarter in TodoQuarters.Values)
        {
            foreach (var item in quarter.GetItems())
            {
               
                lines.Add($"{item.GetTitle()}|{item.GetDeadline():dd-MM}|{item.IsImportant}");
            }
        }

        File.WriteAllLines(fileName, lines);
    }


    public void ArchiveItems()
    {
        foreach (var quarter in TodoQuarters.Values)
        {
            quarter.ArchiveItems();
        }
    }
  
    public override string ToString()
    {
        string separator = "--------------|--------------------------------|--------------------------------";
        string[] rows = new string[7];  // Adjust the number of rows

        rows[0] = "              |            URGENT              |           NOT URGENT           ";
        rows[1] = separator;

        int maxImportantItems = Math.Max(TodoQuarters["IU"].GetItems().Count, TodoQuarters["IN"].GetItems().Count);
        int maxNotImportantItems = Math.Max(TodoQuarters["NU"].GetItems().Count, TodoQuarters["NN"].GetItems().Count);

        for (int i = 0; i < maxImportantItems; i++)
        {
            string[] items = new string[4];  // Adjust the number of items per row

            AddItemToRow(items, TodoQuarters["IU"], i, 0);
            AddItemToRow(items, TodoQuarters["IN"], i, 1);

            rows[i + 2] = "IMPORTANT".PadRight(13) + " | " + items[0] + " | " + items[1];
        }

        rows[maxImportantItems + 2] = separator;

        int rowNum = maxImportantItems + 3;

        for (int i = 0; i < maxNotImportantItems; i++)
        {
            string[] items = new string[4];  // Adjust the number of items per row

            AddItemToRow(items, TodoQuarters["NU"], i, 2);
            AddItemToRow(items, TodoQuarters["NN"], i, 3);

            rows[rowNum] = "NOT IMPORTANT".PadRight(13) + " | " + items[2] + " | " + items[3];
            rowNum++;
        }

        rows[rowNum] = separator;

        return string.Join("\n", rows);
    }

    private void AddItemToRow(string[] items, TodoQuarter quarter, int index, int column)
    {
        List<TodoItem> itemsInQuarter = quarter.GetItems();
        if (index < itemsInQuarter.Count)
        {
            items[column] = itemsInQuarter[index].ToString().PadRight(34);
        }
        else
        {
            items[column] = "".PadRight(34);
        }
    }


}

class Program
{
    static void Main(string[] args)
    {
        TodoMatrix matrix = new TodoMatrix();

        // Add some sample items to the matrix
        matrix.AddItem("Go to the doctor", new DateTime(2023, 8, 28), true);
        matrix.AddItem("Submit assignment", new DateTime(2023, 8, 30), true);
        matrix.AddItem("Buy a ticket", new DateTime(2023, 8, 31), false);
        matrix.AddItem("House of Cards", new DateTime(2023, 8, 25), false);

        // Mark an item as done
        matrix.GetQuarter("IU").GetItem(0).Mark();

        // Print the matrix before archiving
        Console.WriteLine("Matrix before archiving:\n");
        Console.WriteLine(matrix);

        // Archive done items
        matrix.ArchiveItems();

        // Print the matrix after archiving
        Console.WriteLine("\nMatrix after archiving:\n");
        Console.WriteLine(matrix);

        string FileName = @"C:\Users\dasha\source\repos\EisenhowerMatrixApp\EisenhowerMatrix.csv";
        // Save items to the file
        matrix.SaveItemsToFile(FileName);

        // Clear the matrix
        matrix = new TodoMatrix();

        // Load items from the file
        matrix.AddItemsFromFile(FileName);

        // Print the matrix after loading from file
        Console.WriteLine("\nMatrix after loading from file:\n");
        Console.WriteLine(matrix);

        // Example of how to interact with TodoQuarter
        TodoQuarter importantUrgentQuarter = matrix.GetQuarter("IU");
        importantUrgentQuarter.AddItem("Finish project", new DateTime(2023, 8, 27), true);

        Console.WriteLine("\nImportant & Urgent quarter:\n");
        Console.WriteLine(importantUrgentQuarter);
    }
}

