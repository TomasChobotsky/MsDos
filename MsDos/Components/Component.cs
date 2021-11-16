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
        public IWindow Window { get; protected set; }

        public Component(IWindow window)
        {
            Window = window;
            Window.WindowResizedEvent += OnResize;
        }

        public abstract void Render();
        public abstract void CreateComponent();

        public virtual void OnResize(object sender, EventArgs e) { }
    }
}