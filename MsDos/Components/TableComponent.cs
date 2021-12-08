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
            public List<string> Content { get; set; }

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

                if(DeserializedContent.Count > 0)
                    DeserializedContent[selectedIndex].IsSelected = true;
            }
        }
        
        public List<ColumnDefinition> Columns { get; set; } = new List<ColumnDefinition>();
        public int SelectedIndex { get; set; } = 0;

        public int Offset { get; set; } = 0;
        public int MouseY { get; set; } = 0;

        public TableComponent(double percentWidth, double percentHeight, double percentX, int posY, string header, 
            ConsoleColor backgroundColor, ConsoleColor foregroundColor, IWindow window) : base(header, backgroundColor, foregroundColor, window)
        {
            PercentWidth = percentWidth;
            PercentHeight = percentHeight;
            Width = window.GetWidthByPortion(percentWidth);
            Height = window.GetHeightByPortion(percentHeight);

            //PosX is currently defined in percentage to be responsible... Could be done with some kind of simplified FlexBox
            //PosY doesn't need to be defined by percent, because it doesn't change when you resize your screen
            PercentX = percentX;
            PosX = (int)Math.Round(Width * (PercentX / 100), 0);
            PosY = posY;
        }

        public void ChangeFocusedContent (int sum)
        { 
            SelectedIndex += sum;
            MouseY += sum;

            if (MouseY == Height - 5)
            {
                MouseY -= sum;
                
                if (Columns[0].DeserializedContent.Count() - Offset > Height - 5)
                    Offset++;
            }
            if (MouseY == -1)
            {
                MouseY -= sum;
                Offset--;
            }
            
            if (SelectedIndex == Columns[0].DeserializedContent.Count())
            {
                SelectedIndex = 0;
                MouseY = 0;
                Offset = 0;
            }
            else if (SelectedIndex < 0)
            {
                SelectedIndex = Columns[0].DeserializedContent.Count() - 1;
                MouseY = Height - 5;
                Offset = Columns[0].DeserializedContent.Count() - (Height - 5);
            }
            
            EmptyComponent();
            CreateBody();
            CreateBorder();
            Window.Render();
        }
        
        
        public override void OnResize(object sender, WindowResizedEventArgs e)
        {
            if (MouseY > Window.Height)
            {
                SelectedIndex = 0;
                MouseY = 0;
                Offset = 0;
            }
            
            Height = (int)Math.Round(e.Height * (PercentHeight / 100));
            Width = (int)Math.Round(e.Width * (PercentWidth / 100));
            
            PosX = (int)Math.Round(e.Width * (PercentX / 100), 0);
            
            CreateBorder();
            CreateBody();
        }

        public override void CreateBody()
        {
            int columnStartX = PosX;
            foreach (var column in Columns)
            {
                column.DeserializeContent(SelectedIndex);
                int columnWidth = 0;
                if (column.Portion == -1)
                {
                    columnWidth = (Width + PosX) - columnStartX - 1;
                }
                else
                {
                    columnWidth = (int)Math.Floor(Width * ((double)column.Portion / 100));
                }
                int columnMiddle = (int)Math.Ceiling((double)columnWidth / 2);

                for (int x = (columnMiddle - column.Header.Length / 2) + columnStartX; x < (columnMiddle + column.Header.Length / 2) + columnStartX; x++)
                {
                    if (x < 0)
                        continue;
                    Window.Buffer[x, PosY + 1] = new Pixel(column.Header[x - ((columnMiddle - column.Header.Length / 2) + columnStartX)],
                        BgColor, FgColor);
                }
                Window.Buffer[columnWidth + columnStartX, PosY + 1] = new Pixel('│', BgColor, FgColor);
                
                var offsetContent = column.DeserializedContent.Skip(Offset).Take(column.DeserializedContent.Count - Offset);

                int y = PosY + 2;
                foreach (var content in offsetContent)
                {
                    if (y > Height - 3)
                    {
                        break;
                    }

                    Window.Buffer[columnWidth + columnStartX, y] = new Pixel('│', BgColor, FgColor);

                    var bgColor = BgColor;
                    
                    if (content.IsSelected)
                    {
                        bgColor = ConsoleColor.Cyan;
                    }

                    for (int x = columnStartX; x < columnWidth + columnStartX - 1; x++)
                    {
                        if (x - columnStartX > content.Value.Length - 1)
                            break;
                        Window.Buffer[x + 1, y] = new Pixel(content.Value[x - columnStartX], bgColor, FgColor);
                    }

                    y++;
                }

                columnStartX = columnWidth + columnStartX;
            }
        }
    }
}