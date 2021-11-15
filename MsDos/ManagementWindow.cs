using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MsDos
{
    public class ManagementWindow : Component
    {
        public string Drive { get; set; }
        public List<DirFile> Directories { get; set; } = new List<DirFile>();
        public bool IsSelected { get; set; } = false;
        public int SelectedIndex { get; set; } = 0;

        private int offset = 0;
        private int mouseY = 0;
        
        public ManagementWindow(int width, int height, int posX, int posY, string drive)
        {
            Width = width;
            Height = height;
            PosX = posX;
            PosY = posY;
            Drive = drive;

            FillBuffers(width, height);
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

        public void ResizeWindow(int width, int height, int posX, int posY)
        {
            if (mouseY > height)
            {
                SelectedIndex = 0;
                mouseY = 0;
                offset = 0;
            }
            
            Height = height;
            Width = width;
            PosX = posX;
            PosY = posY;

            FillBuffers(width, height);
        }

        public void WriteToBuffer()
        {
            for (int v = 1; v < Height - 2; v++)
            {
                Buffer[0, v] = new Pixel('│', ConsoleColor.Blue, ConsoleColor.White);
                Buffer[Width - 1, v] = new Pixel('│', ConsoleColor.Blue, ConsoleColor.White);
            }
            
            int textPosition = Width / 2;
            string text = $" {Drive} ";
            for (int x = 0; x < Width; x++)
            {
                if (x >= textPosition - text.Length / 2 && x < textPosition + text.Length / 2 + 1)
                {
                    if (IsSelected)
                        Buffer[x, 0] = new Pixel(text[x - (textPosition - text.Length / 2)], ConsoleColor.White, ConsoleColor.Black);
                    else 
                        Buffer[x, 0] = new Pixel(text[x - (textPosition - text.Length / 2)], ConsoleColor.Gray, ConsoleColor.Black);
                }
                else
                    Buffer[x, 0] = new Pixel('─', ConsoleColor.Blue, ConsoleColor.White);
                
                Buffer[x, Height - 2] = new Pixel('─', ConsoleColor.Blue, ConsoleColor.White);
            }


            // Gets postion of the elements depending on the portion of window it takes
            int nameEnd = (int)(Width / 1.8);
            int nameMiddle = nameEnd / 2;
            string name = "Název";
            
            int sizeEnd = (int)(Width / 1.35);
            int sizeMiddle = (sizeEnd - nameEnd) / 2 + nameEnd + 1;
            string size = "Velikost";
            
            int dateEnd = Width;
            int dateMiddle = (dateEnd - sizeEnd) / 2 + sizeEnd;
            string date = "Datum";
            
            for (int x = 0; x < Width; x++)
            {
                if (x == nameEnd)
                    Buffer[x, 1] = new Pixel('│', ConsoleColor.Blue, ConsoleColor.White);
                if (x == sizeEnd)
                    Buffer[x, 1] = new Pixel('│', ConsoleColor.Blue, ConsoleColor.White);
                
                
                if (x >= nameMiddle - name.Length / 2 && x < nameMiddle + name.Length / 2 + 1)
                {
                    Buffer[x, 1] = new Pixel(name[x - (nameMiddle - name.Length / 2)], ConsoleColor.Blue, ConsoleColor.White);
                }
                else if (x >= sizeMiddle - size.Length / 2 && x < sizeMiddle + size.Length / 2)
                {
                    Buffer[x, 1] = new Pixel(size[x - (sizeMiddle - size.Length / 2)], ConsoleColor.Blue, ConsoleColor.White);
                }
                else if (x >= dateMiddle - date.Length / 2 && x < dateMiddle + date.Length / 2 + 1)
                {
                    Buffer[x, 1] = new Pixel(date[x - (dateMiddle - date.Length / 2)], ConsoleColor.Blue, ConsoleColor.White);
                }
            }

            var directoryInfoList = Directory.GetDirectories(Drive).Select(x => new DirectoryInfo(x));
            var fileList = Directory.GetFiles(Drive).Select(x => new FileInfo(x));
            Directories = new List<DirFile>();
            foreach (var directoryInfo in directoryInfoList)
            {
                if (Directories.Count() != SelectedIndex)
                    Directories.Add(new DirFile(false, directoryInfo.Name, 
                        directoryInfo.LastWriteTime.ToString(CultureInfo.CreateSpecificCulture("de-DE")), "<SUB-DIR>"));
                else
                    Directories.Add(new DirFile(true, directoryInfo.Name, 
                        directoryInfo.LastWriteTime.ToString(CultureInfo.CreateSpecificCulture("de-DE")), "<SUB-DIR>"));
            }

            foreach (var fileInfo in fileList)
            {
                if (Directories.Count() != SelectedIndex)
                    Directories.Add(new DirFile(false, fileInfo.Name, 
                        fileInfo.LastWriteTime.ToString(CultureInfo.CreateSpecificCulture("de-DE")), (int)Math.Round((double)fileInfo.Length / 1024, 0) + "kB"));
                else
                    Directories.Add(new DirFile(true, fileInfo.Name, 
                        fileInfo.LastWriteTime.ToString(CultureInfo.CreateSpecificCulture("de-DE")), (int)Math.Round((double)fileInfo.Length / 1024, 0) + "kB"));
            }

            var offsetDirectories = Directories.Skip(offset).Take(Directories.Count - offset);
            
            int y = 2;
            foreach (var directory in offsetDirectories)
            {
                if (y > Height - 3)
                    break;
                string dirName = directory.Name;
                var dirDate = directory.Date.Substring(0, 10);
                string dirSize = directory.Size;
                ConsoleColor bgColor = ConsoleColor.Blue;
                ConsoleColor fgColor = ConsoleColor.White;
                
                if (directory.IsSelected)
                {
                    bgColor = ConsoleColor.Cyan;
                }
                
                for (int x = 0; x < Width - 1; x++)
                {
                    if (x == nameEnd)
                        Buffer[x, y] = new Pixel('│', ConsoleColor.Blue, ConsoleColor.White);
                    if (x == sizeEnd)
                        Buffer[x, y] = new Pixel('│', ConsoleColor.Blue, ConsoleColor.White);


                    if (x >= 0 && x < nameEnd - 1 && dirName.Length > x)
                    {
                        Buffer[x + 1, y] = new Pixel(dirName[x], bgColor, fgColor);
                    }
                    else if (x >= nameEnd && x < sizeEnd - 1 && dirSize.Length > x - nameEnd)
                    {
                        Buffer[x + 1, y] = new Pixel(dirSize[x - nameEnd], bgColor, fgColor);
                    }
                    else if (x >= sizeEnd && x < dateEnd - 2 && dirDate.Length > x - sizeEnd)
                    {
                        Buffer[x + 1, y] = new Pixel(dirDate[x - sizeEnd], bgColor, fgColor);
                    }
                }
                y++;
            }
        }
        public long FileSize(DirectoryInfo d)
        {
            return 0;
        }
        
        public override void Render()
        {
            for (int y = 0; y < Height - 1; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Console.SetCursorPosition(x, y);
                    if (Buffer[x, y].Character != TempBuffer[x, y].Character)
                        Console.Write(Buffer[x, y].Character);
                    if (Buffer[x, y].BackgroundColor != TempBuffer[x, y].BackgroundColor)
                        Console.BackgroundColor = Buffer[x, y].BackgroundColor;
                    if (Buffer[x, y].ForegroundColor != TempBuffer[x, y].ForegroundColor)
                        Console.ForegroundColor = Buffer[x, y].ForegroundColor;
                }
            }

            for (int y = 0; y < Height - 1; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    TempBuffer[x, y] = Buffer[x, y];
                }
            }
        }
    }
}