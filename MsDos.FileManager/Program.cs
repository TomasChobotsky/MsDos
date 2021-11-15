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
            Window window = new Window(100, 50);

            TableComponent table1 = new TableComponent((int)Math.Floor(window.Width / 2.1), window.Height - 3, 1, 2);
            table1.Columns.Add(new Column(45, "Name", new List<string>() {"Sus", "Gus", "Pus"}));
            table1.Columns.Add(new Column(15, "Size", new List<string>() {"big size", "huge size", "extremely huge size"}));
            table1.Columns.Add(new Column(40, "Date", new List<string>() {"Nice date", "Lovely date", "Cool date"}));

            Console.ReadKey();
        }
    }
};