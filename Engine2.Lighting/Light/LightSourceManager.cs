using Engine2.Actor;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Lighting.Light
{
    public class LightSourceManager
    {
        public static List<LightSource> Lights = new List<LightSource>();

        public static LightSource GetLightSource(GameActor parent = null)
        {
            // Yes, this is kinda redundant, but it's easy to monitor them at LightSourceManager level than at the root LightSource level.
            if (Lights.Count == 8)
                return null;

            var light = LightSource.GetLightSource();

            if(light != null)
            {
                if(parent != null)
                    light.ParentActor = parent;

                Lights.Add(light);
            }

            return light;
        }

    }
}
