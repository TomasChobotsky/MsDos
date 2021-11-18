using System;
using System.Collections.Generic;
using MsDos.Contracts;
using MsDos.Core;

namespace MsDos
{
    public class MainView : View
    {
        IWindow window = new Window();
        
        public override void ConstructView()
        {
            TableComponent table1 = new TableComponent((int)Math.Floor(window.Width / 2.1), window.Height - 3, 0, 2, @"C:\", window);
            table1.CreateBorder();
            table1.Columns.Add(new TableComponent.ColumnDefinition(40, "Name", new List<string>() { "Sus", "Gus", "Pus"}));
            table1.Columns.Add(new TableComponent.ColumnDefinition(20, "Size", new List<string>() {"big size", "huge size", "extremely huge size"}));
            
            //-1 fills the rest of the component (Last column was acting stupid because of the rounding...)
            table1.Columns.Add(new TableComponent.ColumnDefinition(-1, "Date", new List<string>() {"Nice date", "Lovely date", "Cool date"}));
            table1.CreateBody();

            window.CreateWindow();
        }
        
        public override void OnKeyDown(object sender, ConsoleKey key)
        {
        }
    }
}