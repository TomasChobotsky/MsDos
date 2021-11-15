using System.Collections.Generic;

namespace MsDos
{
    public record Column(int Portion, string Header, List<string> Content) { }
}