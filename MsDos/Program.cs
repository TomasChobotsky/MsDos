using System;

namespace MsDos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.BufferHeight = Console.WindowHeight;
            FileManager manager = new FileManager();

            Console.ReadKey();
        }
    }
}