using System;

namespace MsDos.Contracts
{
    public interface IInputHandler
    {
        event EventHandler<ConsoleKey> KeyDownEvent;

        void ListenToInput();
    }
}