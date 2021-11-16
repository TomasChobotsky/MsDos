using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsDos.Style
{
    class Border
    {
        public BorderStyle style { get; set; } = new BorderStyle();
        public BorderStyle.Type BorderType { get; set; }

        public Border()
        {
            BorderType = BorderStyle.Type.None;
        }
    }
}
