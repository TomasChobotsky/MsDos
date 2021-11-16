using MsDos.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                DeserializedContent = new List<TableContent>();

                DeserializeContent();
            }

            private void DeserializeContent()
            {
                foreach (var content in Content)
                {
                    DeserializedContent.Add(new TableContent(false, content));
                }
            }
        }
        
        public List<ColumnDefinition> Columns { get; set; } = new List<ColumnDefinition>();
        public int SelectedIndex { get; set; } = 0;

        private int offset = 0;
        private int mouseY = 0;
        
        public TableComponent(int width, int height, int posX, int posY, string header, IWindow window) : base(header, window)
        {
            Width = width;
            Height = height;
            PosX = posX;
            PosY = posY;
        }

        public override void OnCreate()
        {
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
        }
        
        
        public override void OnResize(object sender, EventArgs e)
        {
            if (mouseY > Window.Height)
            {
                SelectedIndex = 0;
                mouseY = 0;
                offset = 0;
            }
            
            Height = Window.Height;
            Width = Window.Width;
            
            //Currently sets static posX and posY with gap in the middle depending on the width and height of the component
            //PosX = posX;
            //PosY = posY;
        }

        public override void CreateBody()
        {
            int columnStartX = 0;
            foreach (var column in Columns)
            {
                int columnWidth = Width * (column.Portion / 100);
                int columnMiddle = columnWidth / 2;

                for (int x = columnMiddle - column.Header.Length / 2; x < columnMiddle + column.Header.Length / 2; x++)
                {
                    Window.Buffer[x, 1] = new Pixel(column.Header[x - (columnMiddle - column.Header.Length / 2)],
                        ConsoleColor.Blue, ConsoleColor.White);
                }

                int y = 2;
                foreach (var content in column.DeserializedContent)
                {
                    if (y > Height - 3)
                    {
                        Window.Buffer[columnWidth, y] = new Pixel('│', ConsoleColor.Blue, ConsoleColor.White);
                        break;
                    }

                    ConsoleColor bgColor = ConsoleColor.Blue;
                    ConsoleColor fgColor = ConsoleColor.White;

                    for (int x = 0; x < content.Value.Length; x++)
                    {
                        Window.Buffer[x + 1, y] = new Pixel(content.Value[x], bgColor, fgColor);
                    }

                    y++;
                }

            }
        }

        public override void Render()
        {
            for (int y = 0; y < Height - 1; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.SetCursorPosition(x, y);
                    
                    if (Window.Buffer[x, y].Character != Window.TempBuffer[x, y].Character)
                        Console.Write(Window.Buffer[x, y].Character);
                    
                    if (Window.Buffer[x, y].BackgroundColor != Window.TempBuffer[x, y].BackgroundColor)
                        Console.BackgroundColor = Window.Buffer[x, y].BackgroundColor;
                    
                    if (Window.Buffer[x, y].ForegroundColor != Window.TempBuffer[x, y].ForegroundColor)
                        Console.ForegroundColor = Window.Buffer[x, y].ForegroundColor;
                }
            }

            for (int y = 0; y < Height - 1; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Window.TempBuffer[x, y] = Window.Buffer[x, y];
                }
            }
        }
    }
}