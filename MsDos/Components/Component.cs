using MsDos.Contracts;
using MsDos.Core;
using System;
using System.Threading.Tasks;
using MsDos.Data;

namespace MsDos
{
    public abstract class Component
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public double PercentWidth { get; set; }
        public double PercentHeight { get; set; }
        
        public int PosX { get; set; }
        public int PosY { get; set; }
        public double PercentX { get; set; }
        
        public string Header { get; set; }
        public bool IsSelected { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public IWindow Window { get; }

        public ConsoleColor BgColor { get; set; }
        public ConsoleColor FgColor { get; set; }

        public Component(string header, ConsoleColor bgColor, ConsoleColor fgColor, IWindow window)
        {
            Header = header;
            BgColor = bgColor;
            FgColor = fgColor;
            Window = window;
            Window.ComponentControl.Components.Add(this);
            Window.WindowResizedEvent += OnResize;
        }

        public Component() {}
        
        public abstract void CreateBody();

        public virtual void OnResize(object sender, WindowResizedEventArgs e) { }
        
        /// <summary>
        /// Generates the border of current component given the specified attributes (thickness, color, type)
        /// </summary>
        public void CreateBorder()
        {
            if (IsActive)
            {
                int textPosition = Width / 2 + PosX;
                string text = $" {Header} ";
                for (int x = PosX; x < Width + PosX; x++)
                {
                    if (x >= textPosition - text.Length / 2 && x < textPosition + text.Length / 2)
                    {
                        if (IsSelected)
                            Window.Buffer[x, PosY] = new Pixel(text[x - (textPosition - text.Length / 2)], ConsoleColor.White,
                                ConsoleColor.Black);
                        else
                            Window.Buffer[x, PosY] = new Pixel(text[x - (textPosition - text.Length / 2)], ConsoleColor.Gray,
                                ConsoleColor.Black);
                    }
                    else
                        Window.Buffer[x, PosY] = new Pixel('─', BgColor, FgColor);

                    Window.Buffer[x, Height - 2] = new Pixel('─', BgColor, FgColor);
                }

                for (int v = PosY + 1; v < Height - 2; v++)
                {
                    Window.Buffer[PosX, v] = new Pixel('│', BgColor, FgColor);
                    Window.Buffer[(Width - 1) + PosX, v] = new Pixel('│', BgColor, FgColor);
                }
            }
        }

        public void EmptyComponent()
        {
            for (int j = PosX; j < PosX + Width; j++)
            {
                for (int i = 0; i < Height; i++)
                {
                    Window.Buffer[j, i] = new Pixel(' ', BgColor, FgColor);
                }
            }
        }
    }
}