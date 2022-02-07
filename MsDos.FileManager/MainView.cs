using System;
using System.Collections.Generic;
using System.Linq;
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
        private Layout layout;
        private DialogComponent noAccessDialog;
        private string drive1 = "X:\\";
        private string drive2 = "D:\\";
        private ComponentControl controller = new ComponentControl();
        private FileManager fileManager = new FileManager();
        
        public override void ConstructView()
        {
            window = new Window();
            layout = new Layout(100, 3, ConsoleColor.White, ConsoleColor.Black, window);
            layout.AddTextBox("FILE", 20, 0, ConsoleColor.Gray, ConsoleColor.White);
            layout.AddTextBox("EDIT", 20, 25, ConsoleColor.Gray, ConsoleColor.White);
            table1 = new TableComponent(48, 100, 1, 1, drive1, ConsoleColor.Blue, ConsoleColor.White, window);
            fileManager.ReadDirectories(drive1);
            table1.Columns.Add(new TableComponent.ColumnDefinition(40, "Name", fileManager.Directories.Select(t => t.Name).ToList()));
            table1.Columns.Add(new TableComponent.ColumnDefinition(20, "Size", fileManager.Directories.Select(t => t.Size).ToList()));
            //-1 fills the rest of the component (Last column was acting stupid because of the rounding...)
            table1.Columns.Add(new TableComponent.ColumnDefinition(-1, "Date", fileManager.Directories.Select(t => t.Date).ToList()));
            table1.IsSelected = true;
            
            table2 = new TableComponent(48, 100, 51, 1, drive2, ConsoleColor.Blue, ConsoleColor.White, window);
            fileManager.ReadDirectories(drive2);
            table2.Columns.Add(new TableComponent.ColumnDefinition(40, "Name", fileManager.Directories.Select(t => t.Name).ToList()));
            table2.Columns.Add(new TableComponent.ColumnDefinition(20, "Size", fileManager.Directories.Select(t => t.Size).ToList()));
            table2.Columns.Add(new TableComponent.ColumnDefinition(-1, "Date", fileManager.Directories.Select(t => t.Date).ToList()));

            noAccessDialog = new DialogComponent(48, 100, 1, 1, "No Access", ConsoleColor.Blue, ConsoleColor.White, window);
            noAccessDialog.IsActive = false;
            
            table1.CreateBorder();
            table1.CreateBody();
            table2.CreateBorder();
            table2.CreateBody();
            layout.CreateBody();

            controller.Components.Add(table1);
            controller.Components.Add(table2);
            controller.SelectedComponentIndex = 0;

            window.Start();
        }
        
        public override void OnKeyDown(object sender, ConsoleKey key)
        {
            if (key == ConsoleKey.UpArrow)
                ((TableComponent)controller.SelectedComponent).ChangeFocusedContent(-1);

            if (key == ConsoleKey.DownArrow)
                ((TableComponent)controller.SelectedComponent).ChangeFocusedContent(1);
            
            if (key == ConsoleKey.Tab)
            {
                if (controller.SelectedComponentIndex < controller.Components.Count - 1)
                    controller.SelectedComponentIndex++;
                else
                    controller.SelectedComponentIndex = 0;
                foreach (var component in controller.Components)
                    component.IsSelected = false;
                controller.Components[controller.SelectedComponentIndex].IsSelected = true;
                controller.RecreateComponents();
            }

            if (key == ConsoleKey.Spacebar)
            {
                var selected = (TableComponent)controller.SelectedComponent;
                var tempHeader = selected.Header;
                try
                {
                    fileManager.ReadDirectories(selected.Header);

                    selected.Header +=
                        $"{selected.Columns[0].Content[selected.SelectedIndex]}\\";
                
                    RegenerateColumns(selected);
                }
                catch
                {
                    noAccessDialog.IsActive = true;
                    noAccessDialog.EmptyComponent();
                    noAccessDialog.CreateBody();
                    noAccessDialog.CreateBorder();
                    window.Render();
                    selected.Header = tempHeader;
                    return;
                }
            }

            if (key == ConsoleKey.Backspace)
            {
                var selected = (TableComponent)controller.SelectedComponent;

                if (selected.Header == drive1 || selected.Header == drive2)
                    return;

                var splittedHeader = selected.Header.Split("\\");
                selected.Header =
                    selected.Header.Remove(selected.Header.Length - splittedHeader[^2].Length - 1);
                
                RegenerateColumns(selected);
            }
        }

        private void RegenerateColumns(TableComponent selected)
        {
            fileManager.ReadDirectories(selected.Header);

            selected.SelectedIndex = 0;
            selected.Offset = 0;
            selected.MouseY = 0;
                    
            selected.Columns[0].Content = fileManager.Directories.Select(t => t.Name).ToList();
            selected.Columns[1].Content = fileManager.Directories.Select(t => t.Size).ToList();
            selected.Columns[2].Content = fileManager.Directories.Select(t => t.Date).ToList();

            controller.RecreateComponents();
        }
    }
}