using MsDos.Contracts;
using MsDos.Core;
using System;
using System.Threading.Tasks;

namespace MsDos
{
    public abstract class Component
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int PosX { get; protected set; }
        public int PosY { get; protected set; }
        public string Header { get; protected set; }
        public bool IsSelected { get; protected set; } = false;
        public IWindow Window { get; protected set; }

        public Component(string header,IWindow window)
        {
            Header = header;
            Window = window;
            Window.ComponentControl.Components.Add(this);
            Window.WindowResizedEvent += OnResize;
        }
        
        /// <summary>
        /// Use after you define all additional styling options and add all content
        /// </summary>
        public abstract void OnCreate();
        public abstract void Render();
        public abstract void CreateBody();

        public virtual void OnResize(object sender, EventArgs e) { }
        
        /// <summary>
        /// Generates the border of current component given the specified attributes (thickness, color, type)
        /// </summary>
        public void CreateBorder()
        {
            int textPosition = Width / 2;
            string text = $" {Header} ";
            for (int x = 0; x < Width; x++)
            {
                if (x >= textPosition - text.Length / 2 && x < textPosition + text.Length / 2 + 1)
                {
                    if (IsSelected)
                        Window.Buffer[x, 0] = new Pixel(text[x - (textPosition - text.Length / 2)], ConsoleColor.White,
                            ConsoleColor.Black);
                    else
                        Window.Buffer[x, 0] = new Pixel(text[x - (textPosition - text.Length / 2)], ConsoleColor.Gray,
                            ConsoleColor.Black);
                }
                else
                    Window.Buffer[x, 0] = new Pixel('─', ConsoleColor.Blue, ConsoleColor.White);

                Window.Buffer[x, Height - 2] = new Pixel('─', ConsoleColor.Blue, ConsoleColor.White);
            }

            for (int v = 1; v < Height - 2; v++)
            {
                Window.Buffer[0, v] = new Pixel('│', ConsoleColor.Blue, ConsoleColor.White);
                Window.Buffer[Width - 1, v] = new Pixel('│', ConsoleColor.Blue, ConsoleColor.White);
            }
        }
    }
}