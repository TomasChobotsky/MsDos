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
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;

            MainView mainView = new MainView();
            mainView.ConstructView();
        }
    }
}