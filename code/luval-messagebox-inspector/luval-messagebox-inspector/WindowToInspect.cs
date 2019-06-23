using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval_messagebox_inspector
{
    public class WindowToInspect
    {
        public WindowToInspect()
        {
            KeysToSend = new List<string>();
        }
        public string Title { get; set; }
        public List<string> KeysToSend { get; set; }
    }
}
