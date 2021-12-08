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
        private List<TextBoxComponent> TextBoxList { get; set; }

        public Layout(double percentWidth, double percentHeight, ConsoleColor bgColor, ConsoleColor fgColor, IWindow window)
        {
            BgColor = bgColor;
            FgColor = fgColor;
            _window = window;
            
            Width = window.GetWidthByPortion(percentWidth);
            Height = window.GetHeightByPortion(percentHeight);
        }

        public void CreateBody()
        {
            foreach (var textBox in TextBoxList)
            {
                
            }
        }

        public void AddTextBox(string text, int percentWidth, int percentX, ConsoleColor bgColor, ConsoleColor fgColor)
        {
            TextBoxList.Add(new TextBoxComponent(percentWidth, 0, percentX, 0, text, bgColor, fgColor, _window));
        }

        public void DeleteTextBox(TextBoxComponent textBox)
        {
            TextBoxList.Remove(textBox);
        }
    }
}