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
        public List<Component> Components { get; set; } = new List<Component>();

        public int SelectedComponentIndex
        {
            get { return selectedComponentIndex;}
            set
            {
                selectedComponentIndex = value;
                
                if (Components.Count > 0)
                    SelectedComponent = Components[SelectedComponentIndex];
            }
            
        }
        
        public Component SelectedComponent { get; set; }
        
        private int selectedComponentIndex;

        public void RecreateComponents()
        {
            foreach (var component in Components)
            {
                component.EmptyComponent();
                component.CreateBody();
                component.CreateBorder();
            }
            Components[0].Window.CreateWindow();
        }
    }
}
