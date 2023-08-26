using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper;
using System.Globalization;
using EisenhowerMatrixApp;

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