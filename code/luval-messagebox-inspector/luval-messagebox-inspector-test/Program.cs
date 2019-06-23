using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace luval_messagebox_inspector_test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var title = "OscarMarin";
            var message =
@"EWP: connection to partner '172.22.0.39:3200' broken

WSAECONNRESET: Connection reset by peer

Do you want to see the detailed error description?
";
            var wait = 5;
            while (true)
            {
                Console.WriteLine("Starting the process waiting {0}", wait);
                Thread.Sleep(wait * 1000);
                var res = MessageBox.Show(message, "SAP GUI for Windows 750", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                Console.WriteLine("Result from message: {0}", res);
                Console.Write("Press any key to continue");
                Console.ReadKey();
            }
        }
    }
}
