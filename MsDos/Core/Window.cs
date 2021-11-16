﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using MsDos.Contracts;

namespace MsDos.Core
{
    public class Window : IWindow
    {
        public bool IsResizing = false;
        public int SelectedWindow = 0;
        public int Width { get; set; } = Console.WindowWidth;
        public int Height { get; set; } = Console.WindowHeight;
        public Pixel[,] Buffer { get; set; }
        public Pixel[,] TempBuffer { get; set; }
        
        private static bool isDrawn = false;

        public event EventHandler WindowResizedEvent;

        public void Start()
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
        
        private void FillBuffers(int width, int height)
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
            Render();
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
            WindowResizedEvent?.Invoke(this, new EventArgs());
            IsResizing = true;
            _resizeTimer.Interval = 500;
            _resizeTimer.Elapsed += new ElapsedEventHandler(_resizeTimer_Tick);
            _resizeTimer.Enabled = true;
        }

        public void Render()
        {
            for (var y = 0; y < Height - 1; y++)
            {
                for (var x = 0; x < Width; x++)
                {
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

            IsResizing = false;
            isDrawn = false;
        }
    }
}