using System.Collections.Generic;
using MsDos.Data;

namespace MsDos
{
    public record Column(int Portion, string Header, List<string> Content, List<TableContent> DeserializedContent)
    {
        
    }
}