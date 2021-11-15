using System;
using System.IO;

namespace MsDos
{
    //Tady jsem chtěl record struct ale není tu c# 10 peeposad
    public record DirFile(bool IsSelected, string Name, string Date, string Size) {}
}