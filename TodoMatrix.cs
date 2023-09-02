using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper;
using System.Globalization;
using EisenhowerMatrixApp;

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
        return TodoQuarters.ContainsKey(status) ? TodoQuarters[status] : null;
    }

    public void AddItem(string title, DateTime deadline, bool isImportant = false)
    {
        string quarterKey = isImportant ? (deadline <= DateTime.Now.AddDays(3) ? "IU" : "IN") : (deadline <= DateTime.Now.AddDays(3) ? "NU" : "NN");
        TodoQuarter quarter = TodoQuarters[quarterKey];
        quarter.AddItem(title, deadline, isImportant);
    }

    public void AddItemsFromFile(string fileName)
    {
        string[] lines = File.ReadAllLines(fileName);

        foreach (string line in lines)
        {
            string[] parts = line.Split('|');
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
    const string separator = "--------------|--------------------------------|--------------------------------";
    const int quarterColumnWidth = 13;
    const int itemColumnWidth = 32;

    List<string> rows = new List<string>
    {
        "              |" + " ".PadRight(quarterColumnWidth) + "URGENT" + " ".PadRight(quarterColumnWidth) + "|" + " ".PadRight(quarterColumnWidth) + "NOT URGENT" + " ".PadRight(quarterColumnWidth),
        separator
    };

    int maxImportantItems = Math.Max(TodoQuarters["IU"].GetItems().Count, TodoQuarters["IN"].GetItems().Count);
    int maxNotImportantItems = Math.Max(TodoQuarters["NU"].GetItems().Count, TodoQuarters["NN"].GetItems().Count);

        rows.Add("IMPORTANT".PadRight(quarterColumnWidth) + " |" + "".PadLeft(itemColumnWidth) + "| " + "".PadRight(itemColumnWidth));
        for (int i = 0; i < maxImportantItems; i++)
    {
        string[] items = new string[4];

        AddItemToRow(items, TodoQuarters["IU"], i, 0);
        AddItemToRow(items, TodoQuarters["IN"], i, 1);
        rows.Add("".PadRight(quarterColumnWidth) + " |" + items[0] + "".PadRight(itemColumnWidth - (items[0].Length-27)) + "|" + items[1]);
    }

    rows.Add(separator);

    int rowNum = maxImportantItems + 3;

        rows.Add("NOT IMPORTANT".PadRight(quarterColumnWidth) + " |" + "".PadRight(itemColumnWidth) + "|" + "".PadRight(itemColumnWidth));
        for (int i = 0; i < maxNotImportantItems; i++)
    {
        string[] items = new string[4];

        AddItemToRow(items, TodoQuarters["NU"], i, 2);
        AddItemToRow(items, TodoQuarters["NN"], i, 3);

        rows.Add("".PadRight(quarterColumnWidth) + " |" + items[2] + "".PadRight(itemColumnWidth - (items[2].Length - 27)) + "|" + items[3]);
        rowNum++;
    }
    rows.Add(separator);

    return string.Join("\n", rows);
}
     private void AddItemToRow(string[] items, TodoQuarter quarter, int index, int column)
    {
        List<TodoItem> itemsInQuarter = quarter.GetItems();
        if (index < itemsInQuarter.Count)
        {
            items[column] = itemsInQuarter[index].ToString();
        }
        else
        {
            items[column] = "";
        }
    }
   

}