using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Util
{
    public class SimpleMath
    {

        public static float Dist(Vector2 a, Vector2 b)
        {
            return (float) Math.Sqrt((b.Y - a.Y) * (b.Y - a.Y) + (b.X - a.X) * (b.X - a.X));
        }
    }
}
