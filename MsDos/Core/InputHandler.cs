using System;
using System.Runtime.CompilerServices;
using MsDos.Contracts;

namespace MsDos.Core
{
    public class InputHandler : IInputHandler
    {
        public event EventHandler<ConsoleKey> KeyDownEvent;
        
        public void ListenToInput ()
        {
            while (true) 
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey input = Console.ReadKey().Key;
                    KeyDownEvent?.Invoke(this, input);
                }
            }
        }
    }
}