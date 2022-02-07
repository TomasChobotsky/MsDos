using MsDos.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsDos.Components
{
    public class DialogComponent : Component
    {
        public DialogComponent (double percentWidth, double percentHeight, double percentX, int posY, string header, ConsoleColor bgColor, ConsoleColor fgColor,
            IWindow window) : base(header, bgColor, fgColor, window)
        {
            PercentWidth = percentWidth;
            PercentHeight = percentHeight;
            Width = (int)Math.Round(window.Width * (percentWidth / 100), 0);
            Height = (int)Math.Round(window.Height * (percentHeight / 100), 0);
            PercentX = percentX;
            PosX = (int)Math.Round(Width * (PercentX / 100), 0);
            PosY = posY;
        }

        public override void CreateBody()
        {
            
        }
    }
}
