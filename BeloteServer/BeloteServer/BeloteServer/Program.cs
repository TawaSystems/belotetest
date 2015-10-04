using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace BeloteServer
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.Listeners.Add(new TextWriterTraceListener("Log.txt"));
            Debug.AutoFlush = true;
            Debug.IndentSize = 4;
            Debug.WriteLine(DateTime.Now.ToString() + " Start Server");
#endif
            Game belote = new Game();
        }
    }
}
