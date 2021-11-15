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
        public Pixel[,] Buffer { get; private set; }
        protected Pixel[,] TempBuffer { get; private set; }

        protected void FillBuffers(int width, int height)
        {
            Buffer = new Pixel[width, height];
            TempBuffer = new Pixel[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Buffer[x, y] = new Pixel(' ', ConsoleColor.Blue, ConsoleColor.White);
                }
            }
        }

        public abstract void Render();
    }
}