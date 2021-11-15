using System;

namespace MsDos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.BufferHeight = Console.WindowHeight;
            Window window = new Window(100, 50);

            Console.ReadKey();
        }
    }
};