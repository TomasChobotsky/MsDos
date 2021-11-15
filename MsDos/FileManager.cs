using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace MsDos
{
    public class FileManager : Component
    {
        public bool IsResizing = false;
        public int SelectedWindow = 0;
        
        private ManagementWindow window1;
        private ManagementWindow window2;
        private bool isDrawn = false;

        private List<ManagementWindow> windows = new();

        public FileManager()
        {
            Height = Console.WindowHeight;
            Width = Console.WindowWidth;
            
            windows = new List<ManagementWindow>();
            window1 = new ManagementWindow((int)Math.Floor(Width / 2.1), Height - 3, 1, 2, @"C:\");
            window2 = new ManagementWindow((int)Math.Floor(Width / 2.1), Height - 3, window1.Width + window1.Width / 10, 2, @"D:\");
            
            windows.Add(window1);
            windows.Add(window2);

            CreateWindow();
            
            while(true)
            {
                while (Console.KeyAvailable && !isDrawn)
                {
                    isDrawn = true;
                    var input = Console.ReadKey(true);
                    if (input.Key == ConsoleKey.Tab)
                    {
                        foreach (var window in windows)
                            window.IsSelected = false;
                        if (SelectedWindow < windows.Count - 1)
                            SelectedWindow++;
                        else
                        {
                            SelectedWindow = 0;
                        }
                    }

                    if (input.Key == ConsoleKey.UpArrow)
                    {
                        windows[SelectedWindow].ChangeFocusedDirectory(-1);
                    }

                    if (input.Key == ConsoleKey.DownArrow)
                    {
                        windows[SelectedWindow].ChangeFocusedDirectory(1);
                    }
                    
                    RerenderWindowBuffers();
                }
                if (Height != Console.WindowHeight || Width != Console.WindowWidth)
                {
                    //Necessary to be here to stop the statement above from being true
                    Height = Console.WindowHeight;
                    Width = Console.WindowWidth;

                    OnResize();
                }
            }
        }

        private void CreateWindow()
        {
            Height = Console.WindowHeight;
            Width = Console.WindowWidth;
            Console.Clear();
            FillBuffers(Width, Height);

            RerenderWindowBuffers();
        }

        public void RerenderWindowBuffers()
        {
            window1.ResizeWindow((int)Math.Floor(Width / 2.1), Height - 3, 1, 2);
            window2.ResizeWindow((int)Math.Floor(Width / 2.1), Height - 3, window1.Width + window1.Width / 10, 2);
            
            windows[SelectedWindow].IsSelected = true;
            
            window1.WriteToBuffer();
            window2.WriteToBuffer();

            ConnectBuffers();

            Render();
        }

        public void ConnectBuffers()
        {
            try
            {
                foreach (var window in windows)
                {
                    for (var y = window.PosY; y < window.Height - 1 + window.PosY; y++)
                    {
                        for (var x = window.PosX; x < window.Width + window.PosX; x++)
                            Buffer[x, y] = window.Buffer[x - window.PosX, y - window.PosY];
                    }
                }
            }
            catch
            {
                return;
            }
        }

        private System.Timers.Timer _resizeTimer = new();

        void _resizeTimer_Tick(object sender, ElapsedEventArgs e)
        {
            Console.CursorVisible = false;
            _resizeTimer.Enabled = false;

            if (IsResizing)
            {
                CreateWindow();
            }
        }
        private void OnResize()
        {
            IsResizing = true;
            _resizeTimer.Interval = 500;
            _resizeTimer.Elapsed += new ElapsedEventHandler(_resizeTimer_Tick);
            _resizeTimer.Enabled = true;
        }

        public override void Render()
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