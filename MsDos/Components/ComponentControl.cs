using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsDos.Components
{
    /// <summary>
    /// Controls interaction between different UI elements (for example: controls which element is currently selected or active
    /// </summary>
    public class ComponentControl
    {
        public List<Component> Components { get; set; }
        public int SelectedComponent { get; set; } = 0;
    }
}
