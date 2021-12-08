using System;
using MsDos.Contracts;

namespace MsDos.Components
{
    public class TextBoxComponent : Component
    {
        public TextBoxComponent(double percentWidth, double percentHeight, double percentX, int posY, string header, 
            ConsoleColor backgroundColor, ConsoleColor foregroundColor, IWindow window) : base (header, backgroundColor, foregroundColor, window)
        {
            Width = window.GetWidthByPortion(percentWidth);
            PosX = window.GetWidthByPortion(percentX);
            Height = window.GetHeightByPortion(percentHeight);
        }
        
        public override void CreateBody()
        {
            throw new System.NotImplementedException();
        }
    }
}