using System;
using System.Runtime.CompilerServices;
using MsDos.Contracts;

namespace MsDos.Data
{
    public class WindowResizedEventArgs
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}