using System;
using System.Runtime.CompilerServices;
using MsDos.Contracts;

namespace MsDos.Core
{
    public class WindowResizedEventArgs
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}