using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.WebSocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            WebSocket socket = new WebSocket();
            socket.start(10020);
            Console.ReadKey();

        }

    }
}
