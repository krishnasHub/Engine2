using Engine2.Core;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Util
{
    public class WorldSettings
    {
        public static View View = null;

        public static int Width;
        public static int Height;

        public static Vector2 TranslateFromScreenToGameCoords(Vector2 pos)
        {
            Vector2 ret = new Vector2(pos.X, pos.Y);
            ret -= new Vector2(Width, Height) / 2f;
            ret = View.ToWorld(ret);
            
            return ret;
        }
    }
}
