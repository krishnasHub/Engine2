using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace Engine2
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = new GameWindow(800, 600);

            g.Run();
        }
    }
}
