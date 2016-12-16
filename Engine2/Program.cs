using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using Engine2.Core;


// https://www.youtube.com/watch?v=SZuPxXAkfyA

// Next tutorial: https://www.youtube.com/watch?v=cZuJ67px5xE&t=385s
// Lightings: http://www.glprogramming.com/red/chapter05.html

namespace Engine2
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = new Game("New Game", 1280, 960);
            // tile_wall.jpg

            g.Run();
        }
    }
}
