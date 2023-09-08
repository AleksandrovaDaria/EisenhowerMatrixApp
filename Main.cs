using System;
using System.Data;
using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        TodoMatrix matrix = new TodoMatrix();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Menu:");
            Console.WriteLine("1 - Add some sample items to the matrix");
            Console.WriteLine("2 - Add your own task");
            Console.WriteLine("3 - Mark an item as done");
            Console.WriteLine("4 - Print the matrix before archiving");
            Console.WriteLine("5 - Archive done items");
            Console.WriteLine("6 - Print the matrix after archiving");
            Console.WriteLine("7 - Save items to the file");
            Console.WriteLine("8 - Clear the matrix");
            Console.WriteLine("9 - Load items from the file");
            Console.WriteLine("10 - Print the matrix after loading from file");
            Console.WriteLine("0 - End Program");

            Console.Write("Enter your choice: ");
            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    Console.WriteLine("Selected option: Add some sample items to the matrix");
                    AddSampleItems(matrix);
                    break;
                case "2":
                    Console.WriteLine("Selected option: Add your own task");
                    AddCustomTask(matrix);
                    break;
                case "3":
                    Console.WriteLine("Selected option: Mark an item as done");
                    MarkItemAsDone(matrix);
                    break;
                case "4":
                    Console.WriteLine("Selected option: Print the matrix before archiving");
                    Console.WriteLine(matrix);
                    break;
                case "5":
                    Console.WriteLine("Selected option: Archive done items");
                    matrix.ArchiveItems();
                    break;
                case "6":
                    Console.WriteLine("Selected option: Print the matrix after archiving");
                    Console.WriteLine(matrix);
                    break;
                case "7":
                    Console.WriteLine("Selected option: Save items to the file");
                    SaveItemsToFile(matrix);
                    break;
                case "8":
                    Console.WriteLine("Selected option: Clear the matrix");
                    matrix = new TodoMatrix();
                    break;
                case "9":
                    Console.WriteLine("Selected option: Load items from the file");
                    LoadItemsFromFile(matrix);
                    break;
                case "10":
                    Console.WriteLine("Selected option: Print the matrix after loading from file");
                    Console.WriteLine(matrix);
                    break;
                case "0":
                    Console.WriteLine("End Program");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

        }

    }

    static void AddSampleItems(TodoMatrix matrix)
    {
        matrix.AddItem("Go to the doctor", new DateTime(2023, 8, 2), true);
        matrix.AddItem("Submit assignment", new DateTime(2023, 9, 4), true);
        matrix.AddItem("Buy a ticket", new DateTime(2023, 8, 2), false);
        matrix.AddItem("House of Cards", new DateTime(2023, 9, 6), false);
    }

    static void AddCustomTask(TodoMatrix matrix)
    {
        Console.Write("Enter task title: ");
        string title = Console.ReadLine();
        Console.Write("Enter task deadline (yyyy-MM-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime deadline))
        {
            Console.Write("Is the task important? (true/false): ");
            if (bool.TryParse(Console.ReadLine(), out bool isImportant))
            {
                matrix.AddItem(title, deadline, isImportant);
                Console.WriteLine("Task added successfully.");
            }
            else
            {
                Console.WriteLine("Invalid input for 'Is the task important?'");
            }
        }
        else
        {
            Console.WriteLine("Invalid input for task deadline.");
        }
    }

    static void MarkItemAsDone(TodoMatrix matrix)
    {
        Console.Write("Enter the quarter (IU, UR, UU, IU): ");
        string quarter = Console.ReadLine();
        Console.Write("Enter the index of the item to mark as done: ");
        if (int.TryParse(Console.ReadLine(), out int index))
        {
            matrix.GetQuarter(quarter).GetItem(index).Mark();
        }
        else
        {
            Console.WriteLine("Invalid index.");
        }
    }

    static void SaveItemsToFile(TodoMatrix matrix)
    {
        Console.Write("Enter the file name: ");
        string fileName = Console.ReadLine();

        try
        {
            matrix.SaveItemsToFile(fileName);
            Console.WriteLine("Items have been saved to the file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving to file: {ex.Message}");
        }
    }

    static void LoadItemsFromFile(TodoMatrix matrix)
    {
        Console.Write("Enter the file name: ");
        string fileName = Console.ReadLine();

        try
        {
            matrix.AddItemsFromFile(fileName);
            Console.WriteLine("Items have been loaded from the file.");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading from file: {ex.Message}");
        }
    }

}