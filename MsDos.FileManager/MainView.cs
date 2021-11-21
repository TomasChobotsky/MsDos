using System;
using System.Collections.Generic;
using MsDos.Components;
using MsDos.Contracts;
using MsDos.Core;

namespace MsDos
{
    public class MainView : View
    {
        private IWindow window;
        private TableComponent table1;
        private TableComponent table2;
        private ComponentControl controller = new ComponentControl();
        
        public override void ConstructView()
        {
            window = new Window();
            table1 = new TableComponent(48, 100, 0, 2, @"C:\", window);
            table1.Columns.Add(new TableComponent.ColumnDefinition(40, "Name", new List<string>() { "Sus", "Gus", "Pus"}));
            table1.Columns.Add(new TableComponent.ColumnDefinition(20, "Size", new List<string>() {"big size", "huge size", "extremely huge size"}));
            //-1 fills the rest of the component (Last column was acting stupid because of the rounding...)
            table1.Columns.Add(new TableComponent.ColumnDefinition(-1, "Date", new List<string>() {"Nice date", "Lovely date", "Cool date"}));
            table1.IsSelected = true;
            
            table2 = new TableComponent(48, 100, 51, 2, @"D:\", window);
            table2.Columns.Add(new TableComponent.ColumnDefinition(40, "Name", new List<string>() { "Sus", "Gus", "Pus"}));
            table2.Columns.Add(new TableComponent.ColumnDefinition(20, "Size", new List<string>() {"big size", "huge size", "extremely huge size"}));
            //-1 fills the rest of the component (Last column was acting stupid because of the rounding...)
            table2.Columns.Add(new TableComponent.ColumnDefinition(40, "Date", new List<string>() {"Nice date", "Lovely date", "Cool date"}));
            
            table1.CreateBorder();
            table1.CreateBody();
            table2.CreateBorder();
            table2.CreateBody();

            controller.Components.Add(table1);
            controller.Components.Add(table2);

            window.Start();
        }
        
        public override void OnKeyDown(object sender, ConsoleKey key)
        {
            if (key == ConsoleKey.UpArrow)
            {
                ((TableComponent)controller.Components[controller.SelectedComponent]).ChangeFocusedDirectory(-1);
            }
            if (key == ConsoleKey.DownArrow)
                ((TableComponent)controller.Components[controller.SelectedComponent]).ChangeFocusedDirectory(1);
        }
    }
}