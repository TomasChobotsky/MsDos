using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace MsDos
{
    public static class Window
    {
        public static bool IsResizing = false;
        public static int SelectedWindow = 0;
        public static int Width = Console.WindowWidth;
        public static int Height = Console.WindowHeight;
        public static Pixel[,] Buffer { get; private set; }
        public static Pixel[,] TempBuffer { get; private set; }
        
        private static bool isDrawn = false;

        public delegate void ResizeEvent(int width, int height);

        public static event ResizeEvent WindowResizedEvent;

        public static void Start()
        {
            CreateWindow();
            while(true)
            {
                if (Height != Console.WindowHeight || Width != Console.WindowWidth)
                {
                    //Necessary to be here to stop the statement above from being true IMMEDIATELY
                    Height = Console.WindowHeight;
                    Width = Console.WindowWidth;

                    OnResize();
                }
            }
        }
        
        private static void FillBuffers(int width, int height)
        {
            Buffer = new Pixel[width, height];
            TempBuffer = new Pixel[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Buffer[x, y] = new Pixel(' ', ConsoleColor.Blue, ConsoleColor.White);
                }
            }
        }

        private static void CreateWindow()
        {
            Height = Console.WindowHeight;
            Width = Console.WindowWidth;
            Console.Clear();
            FillBuffers(Width, Height);

            RerenderWindowBuffers();
        }

        public static void RerenderWindowBuffers()
        {
            Render();
        }

        private static System.Timers.Timer _resizeTimer = new();

        static void _resizeTimer_Tick(object sender, ElapsedEventArgs e)
        {
            Console.CursorVisible = false;
            _resizeTimer.Enabled = false;

            if (IsResizing)
            {
                CreateWindow();
            }
        }
        private static void OnResize()
        {
            WindowResizedEvent?.Invoke(Width, Height);
            IsResizing = true;
            _resizeTimer.Interval = 500;
            _resizeTimer.Elapsed += new ElapsedEventHandler(_resizeTimer_Tick);
            _resizeTimer.Enabled = true;
        }

        public static void Render()
        {
            IsResizing = false;
            for (var y = 0; y < Height - 1; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (IsResizing)
                    {
                        IsResizing = false;
                        return;
                    }
                    
                    Console.SetCursorPosition(x, y);

                    if (Buffer[x, y] != TempBuffer[x, y])
                    {
                        Console.BackgroundColor = Buffer[x, y].BackgroundColor;
                        Console.ForegroundColor = Buffer[x, y].ForegroundColor;
                        Console.Write(Buffer[x, y].Character);
                        TempBuffer[x, y] = Buffer[x, y];
                    }
                }
            }
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            isDrawn = false;
        }
    }
}