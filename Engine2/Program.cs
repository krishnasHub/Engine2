﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using Engine2.Core;


// https://www.youtube.com/watch?v=SZuPxXAkfyA

namespace Engine2
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = new Game(1280, 960);
            // tile_wall.jpg

            g.Run();
        }
    }
}