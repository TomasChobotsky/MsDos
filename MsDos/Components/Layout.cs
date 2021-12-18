using System;
using System.Collections.Generic;
using MsDos.Contracts;

namespace MsDos.Components
{
    public class Layout
    {
        public ConsoleColor BgColor { get; set; }
        public ConsoleColor FgColor { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private IWindow _window;
        private List<TextBoxComponent> TextBoxList { get; set; } = new List<TextBoxComponent>();

        public Layout(double percentWidth, int height, ConsoleColor bgColor, ConsoleColor fgColor, IWindow window)
        {
            BgColor = bgColor;
            FgColor = fgColor;
            _window = window;
            
            Width = window.GetWidthByPortion(percentWidth);
            Height = height;
        }

        public void CreateBody()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _window.Buffer[x, y] = new Pixel(' ', BgColor, FgColor);
                }
            }
            foreach (var textBox in TextBoxList)
            {
                textBox.CreateBody();
            }
        }

        public void AddTextBox(string text, int percentWidth, int percentX, ConsoleColor bgColor, ConsoleColor fgColor)
        {
            TextBoxList.Add(new TextBoxComponent(percentWidth, 1, percentX, 0, text, bgColor, fgColor, _window));
        }

        public void DeleteTextBox(TextBoxComponent textBox)
        {
            TextBoxList.Remove(textBox);
        }
    }
}