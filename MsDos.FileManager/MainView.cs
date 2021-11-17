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
            table1.Columns.Add(new TableComponent.ColumnDefinition(45, "Name", new List<string>() { "Sus", "Gus", "Pus"}));
            table1.Columns.Add(new TableComponent.ColumnDefinition(15, "Size", new List<string>() {"big size", "huge size", "extremely huge size"}));
            table1.Columns.Add(new TableComponent.ColumnDefinition(40, "Date", new List<string>() {"Nice date", "Lovely date", "Cool date"}));
        }
        
        public override void OnKeyDown(object sender, ConsoleKey key)
        {
            
        }
    }
}