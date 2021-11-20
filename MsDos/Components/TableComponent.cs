using MsDos.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MsDos.Core;
using MsDos.Data;

namespace MsDos
{
    public class TableComponent : Component
    {
        public class ColumnDefinition
        {
            public int Portion;
            public string Header;
            public List<TableContent> DeserializedContent;
            
            private List<string> Content;

            public ColumnDefinition(int portion, string header, List<string> content)
            {
                Portion = portion;
                Header = header;
                Content = content;

                DeserializeContent();
            }

            public void DeserializeContent(int selectedIndex = 0)
            {
                DeserializedContent = new List<TableContent>();
                foreach (var content in Content)
                {
                    DeserializedContent.Add(new TableContent(false, content));
                }

                DeserializedContent[selectedIndex].IsSelected = true;
            }
        }
        
        public List<ColumnDefinition> Columns { get; set; } = new List<ColumnDefinition>();
        public int SelectedIndex { get; set; } = 0;

        private int offset = 0;
        private int mouseY = 0;

        public TableComponent(double percentWidth, double percentHeight, double percentX, int posY, string header, IWindow window) : base(
            header, window)
        {
            PercentWidth = percentWidth;
            PercentHeight = percentHeight;
            Width = (int)Math.Round(window.Width * (percentWidth / 100), 0);
            Height = (int)Math.Round(window.Height * (percentHeight / 100), 0);

            //PosX is currently defined in percentage to be responsible... Could be done with some kind of simplified FlexBox
            //PosY doesn't need to be defined by percent, because it doesn't change when you resize your screen
            PercentX = percentX;
            PosX = (int)Math.Round(Width * (PercentX / 100), 0);
            PosY = posY;
        }

        public void ChangeFocusedDirectory (int sum)
        { 
            SelectedIndex += sum;
            mouseY += sum;

            if (mouseY == Height - 4)
            {
                mouseY -= sum;
                
                if (Columns[0].DeserializedContent.Count() - offset > Height - 4)
                    offset++;
            }
            if (mouseY == -1)
            {
                mouseY -= sum;
                offset--;
            }
            
            if (SelectedIndex == Columns[0].DeserializedContent.Count())
            {
                SelectedIndex = 0;
                mouseY = 0;
                offset = 0;
            }
            else if (SelectedIndex < 0)
            {
                SelectedIndex = Columns[0].DeserializedContent.Count() - 1;
                mouseY = Height - 5;
                offset = Columns[0].DeserializedContent.Count() - (Height - 4);
            }
            
            Render();
        }
        
        
        public override void OnResize(object sender, WindowResizedEventArgs e)
        {
            if (mouseY > Window.Height)
            {
                SelectedIndex = 0;
                mouseY = 0;
                offset = 0;
            }
            
            Height = (int)Math.Round(e.Height * (PercentHeight / 100));
            Width = (int)Math.Round(e.Width * (PercentWidth / 100));
            
            PosX = (int)Math.Round(e.Width * (PercentX / 100), 0);
            
            CreateBorder();
            CreateBody();
        }

        public override void CreateBody()
        {
            int columnStartX = 0;
            foreach (var column in Columns)
            {
                column.DeserializeContent(SelectedIndex);
                int columnWidth = 0;
                if (column.Portion == -1)
                {
                    columnWidth = Width - columnStartX - 1;
                }
                else
                {
                    columnWidth = (int)Math.Floor(Width * ((double)column.Portion / 100));
                }
                int columnMiddle = (int)Math.Ceiling((double)columnWidth / 2);

                for (int x = (columnMiddle - column.Header.Length / 2) + columnStartX; x < (columnMiddle + column.Header.Length / 2) + columnStartX; x++)
                {
                    Window.Buffer[x, 1] = new Pixel(column.Header[x - ((columnMiddle - column.Header.Length / 2) + columnStartX)],
                        ConsoleColor.Blue, ConsoleColor.White);
                }
                Window.Buffer[columnWidth + columnStartX, 1] = new Pixel('│', ConsoleColor.Blue, ConsoleColor.White);

                int y = 2;
                foreach (var content in column.DeserializedContent)
                {
                    if (y > Height - 3)
                    {
                        break;
                    }

                    Window.Buffer[columnWidth + columnStartX, y] = new Pixel('│', ConsoleColor.Blue, ConsoleColor.White);

                    ConsoleColor bgColor = ConsoleColor.Blue;
                    ConsoleColor fgColor = ConsoleColor.White;
                    if (content.IsSelected)
                    {
                        bgColor = ConsoleColor.Cyan;
                    }

                    for (int x = columnStartX; x < columnWidth + columnStartX - 1; x++)
                    {
                        if (x - columnStartX > content.Value.Length - 1)
                            break;
                        Window.Buffer[x + 1, y] = new Pixel(content.Value[x - columnStartX], bgColor, fgColor);
                    }

                    y++;
                }

                columnStartX = columnWidth + columnStartX;
            }
        }
        
        public override void Render()
        {
            CreateBorder();
            CreateBody();
            
            for (int y = 0; y < Height - 1; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.SetCursorPosition(x, y);
                    
                    if (Window.Buffer[x, y] != Window.TempBuffer[x, y])
                    {
                        Console.BackgroundColor = Window.Buffer[x, y].BackgroundColor;
                        Console.ForegroundColor = Window.Buffer[x, y].ForegroundColor;
                        Console.Write(Window.Buffer[x, y].Character);
                        Window.TempBuffer[x, y] = Window.Buffer[x, y];
                    }
                }
            }
        }
    }
}