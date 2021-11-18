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

            MainView mainView = new MainView();
            mainView.ConstructView();
        }
    }
}