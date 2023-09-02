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
        string color = GetColor();
        return $"{color}{status}\u001b[0m {color}{Deadline:dd-MM}\u001b[0m {color}{Title}\u001b[0m";
    }

    public string GetColor()
    {
        TimeSpan timeUntilDeadline = Deadline.Date - DateTime.Now.Date;
        if (timeUntilDeadline.Days > 3)
        {
            return "\u001b[32m"; // Green
        }
        else if (timeUntilDeadline.Days > 0)
        {
            return "\u001b[33m"; // Orange
        }
        else
        {
            return "\u001b[31m"; // Red
        }
    }

}
