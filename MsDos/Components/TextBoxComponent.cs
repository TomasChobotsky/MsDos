using System;
using MsDos.Contracts;

namespace MsDos.Components
{
    public class TextBoxComponent : Component
    {
        public TextBoxComponent(double percentWidth, int height, double percentX, int posY, string header, 
            ConsoleColor backgroundColor, ConsoleColor foregroundColor, IWindow window) : base (header, backgroundColor, foregroundColor, window)
        {
            Width = window.GetWidthByPortion(percentWidth);
            PosX = window.GetWidthByPortion(percentX);
            Height = height;
            PosY = posY;
        }
        
        public override void CreateBody()
        {
            int textPosition = Width / 2 + PosX;
            for (int y = PosY; y < Height + PosY; y++)
            {
                for (int x = PosX; x < Width + PosX; x++)
                {
                    if (x >= textPosition - Header.Length / 2 && x < textPosition + Header.Length / 2)
                        Window.Buffer[x, y] = new Pixel(Header[x - (textPosition - Header.Length / 2)], BgColor, FgColor);
                }
            }
        }
    }
}