using MsDos.Contracts;
using MsDos.Core;
using System;
using System.Collections.Generic;

namespace MsDos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.BufferHeight = Console.WindowHeight;
            IWindow window = new Window();

            TableComponent table1 = new TableComponent((int)Math.Floor(window.Width / 2.1), window.Height - 3, 1, 2, @"C:\", window);
            table1.Columns.Add(new TableComponent.ColumnDefinition(45, "Name", new List<string>() { "Sus", "Gus", "Pus"}));
            table1.Columns.Add(new TableComponent.ColumnDefinition(15, "Size", new List<string>() {"big size", "huge size", "extremely huge size"}));
            table1.Columns.Add(new TableComponent.ColumnDefinition(40, "Date", new List<string>() {"Nice date", "Lovely date", "Cool date"}));

            Console.ReadKey();
        }
    }
};