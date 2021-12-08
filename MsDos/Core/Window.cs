using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using MsDos.Components;
using MsDos.Contracts;
using MsDos.Data;

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
        public ComponentControl ComponentControl { get; set; } = new ComponentControl();

        private bool isDrawn = false;
        private string outputString = "";
        private ConsoleColor currentBGColor;
        private ConsoleColor currentFGColor;

        public event EventHandler<WindowResizedEventArgs> WindowResizedEvent;

        public Window()
        {
            FillBuffers(Width, Height);
        }

        public void Start()
        {
            Console.BufferHeight = Console.WindowHeight;
            CreateWindow();
            while (true)
            {
                if (Height != Console.WindowHeight || Width != Console.WindowWidth)
                {
                    //Necessary to be here to stop the statement above from being true IMMEDIATELY
                    Height = Console.WindowHeight;
                    Width = Console.WindowWidth;
                    try
                    {
                        Console.BufferHeight = Console.WindowHeight;
                    }
                    catch
                    {
                        continue;
                    }

                    CreateWindow();
                }
            }
        }

        public int GetWidthByPortion(double portion)
        {
            return (int)Math.Round(Width * (portion / 100), 0);
        }

        public int GetHeightByPortion(double portion)
        {
            return (int)Math.Round(Height * (portion / 100), 0);
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

        public void CreateWindow()
        {
            FillBuffers(Width, Height);
            WindowResizedEvent?.Invoke(this, new WindowResizedEventArgs() { Width = Width, Height = Height });

            Render();
        }

        private System.Timers.Timer _resizeTimer = new();

        public void Render()
        {
            IsResizing = false;
            for (var y = 0; y < Height - 1; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (IsResizing)
                        return;

                    if (currentBGColor != Buffer[x, y].BackgroundColor || currentFGColor != Buffer[x, y].ForegroundColor)
                    {
                        WriteOutputString();
                        currentBGColor = Buffer[x, y].BackgroundColor;
                        currentFGColor = Buffer[x, y].ForegroundColor;
                    }

                    outputString += Buffer[x, y].Character;
                }
                WriteOutputString();
            }
            Console.SetCursorPosition(0, 0);
            isDrawn = false;
        }

        private void WriteOutputString()
        {
            Console.BackgroundColor = currentBGColor;
            Console.ForegroundColor = currentFGColor;
            Console.Write(outputString);
            outputString = "";
        }
    }
}