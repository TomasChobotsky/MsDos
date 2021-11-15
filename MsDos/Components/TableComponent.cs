using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MsDos
{
    public class TableComponent : Component
    {
        public List<Column> Columns { get; set; } = new List<Column>();
        public string Header { get; set; }
        public List<DirFile> Directories { get; set; } = new List<DirFile>();
        public bool IsSelected { get; set; } = false;
        public int SelectedIndex { get; set; } = 0;

        private int offset = 0;
        private int mouseY = 0;
        
        public TableComponent(int width, int height, int posX, int posY, string header)
        {
            Width = width;
            Height = height;
            PosX = posX;
            PosY = posY;
            Header = header;
        }

        public void ChangeFocusedDirectory (int sum)
        {
            SelectedIndex += sum;
            mouseY += sum;

            if (mouseY == Height - 4)
            {
                mouseY -= sum;
                
                if (Directories.Count() - offset > Height - 4)
                    offset++;
            }
            if (mouseY == -1)
            {
                mouseY -= sum;
                offset--;
            }
            
            if (SelectedIndex == Directories.Count())
            {
                SelectedIndex = 0;
                mouseY = 0;
                offset = 0;
            }
            else if (SelectedIndex < 0)
            {
                SelectedIndex = Directories.Count() - 1;
                mouseY = Height - 5;
                offset = Directories.Count() - (Height - 4);
            }
        }

        public override void OnResize(int width, int height)
        {
            if (mouseY > height)
            {
                SelectedIndex = 0;
                mouseY = 0;
                offset = 0;
            }
            
            Height = height;
            Width = width;
            //PosX = posX;
            //PosY = posY;
        }

        public override void CreateComponent()
        {
            int textPosition = Width / 2;
            string text = $" {Header} ";
            for (int x = 0; x < Width; x++)
            {
                if (x >= textPosition - text.Length / 2 && x < textPosition + text.Length / 2 + 1)
                {
                    if (IsSelected)
                        Window.Buffer[x, 0] = new Pixel(text[x - (textPosition - text.Length / 2)], ConsoleColor.White,
                            ConsoleColor.Black);
                    else
                        Window.Buffer[x, 0] = new Pixel(text[x - (textPosition - text.Length / 2)], ConsoleColor.Gray,
                            ConsoleColor.Black);
                }
                else
                    Window.Buffer[x, 0] = new Pixel('─', ConsoleColor.Blue, ConsoleColor.White);

                Window.Buffer[x, Height - 2] = new Pixel('─', ConsoleColor.Blue, ConsoleColor.White);
            }

            for (int v = 1; v < Height - 2; v++)
            {
                Window.Buffer[0, v] = new Pixel('│', ConsoleColor.Blue, ConsoleColor.White);
                Window.Buffer[Width - 1, v] = new Pixel('│', ConsoleColor.Blue, ConsoleColor.White);
            }

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
                foreach (var content in column.Content)
                {
                    if (y > Height - 3)
                    {
                        Window.Buffer[columnWidth, y] = new Pixel('│', ConsoleColor.Blue, ConsoleColor.White);
                        break;
                    }

                    ConsoleColor bgColor = ConsoleColor.Blue;
                    ConsoleColor fgColor = ConsoleColor.White;

                    for (int x = 0; x < Width - 1; x++)
                    {
                        if (x > content.Length)
                            break;

                        Window.Buffer[x + 1, y] = new Pixel(content[x], bgColor, fgColor);
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