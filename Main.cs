using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper;
using System.Globalization;
using EisenhowerMatrixApp;

class Program
{
    static void Main(string[] args)
    {
        TodoMatrix matrix = new TodoMatrix();

        // Add some sample items to the matrix
        matrix.AddItem("Go to the doctor", new DateTime(2023, 8, 2), true);
        matrix.AddItem("Submit assign", new DateTime(2023,9,4), true);
        matrix.AddItem("Buy a ticket", new DateTime(2023, 8, 2), false);
        matrix.AddItem("House of Cards", new DateTime(2023, 9, 6), false);

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
    }
}

