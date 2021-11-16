using System;
using System.IO;

namespace MsDos.Data
{
    //Tady jsem chtěl record struct ale není tu c# 10 peeposad
    public record TableContent(bool IsSelected, string Value) {}
}