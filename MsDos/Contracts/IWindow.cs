using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsDos.Components;

namespace MsDos.Contracts
{
    public interface IWindow
    {
        int Width { get; set; }
        int Height { get; set; }
        Pixel[,] Buffer { get; set; }
        Pixel[,] TempBuffer { get; set; }
        public ComponentControl ComponentControl { get; set; }

        void Start();
        void Render();

        public event EventHandler WindowResizedEvent;
    }
}