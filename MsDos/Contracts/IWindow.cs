using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsDos.Contracts
{
    public interface IWindow
    {
        Pixel[,] Buffer { get; set; }
        Pixel[,] TempBuffer { get; set; }
        int Width { get; set; }
        int Height { get; set; }

        void Start();
        void Render();

        public event EventHandler WindowResizedEvent;
    }
}