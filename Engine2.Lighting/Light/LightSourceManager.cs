using Engine2.Actor;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Lighting.Light
{
    public class LightSourceManager
    {
        public static List<LightSource> Lights = new List<LightSource>();

        public static void SetAmientLight(Color c)
        {
            float[] colorArray = new float[4];

            colorArray[0] = c.R / (float)byte.MaxValue;
            colorArray[1] = c.G / (float)byte.MaxValue;
            colorArray[2] = c.B / (float)byte.MaxValue;
            colorArray[3] = c.A / (float)byte.MaxValue;

            SetAmbientLight(colorArray);
        }

        public static void SetAmientLight(float r = 1f, float g = 1f, float b = 1f, float a = 1f)
        {
            float[] array = { r, g, b, a };
            SetAmbientLight(array);
        }

        private static void SetAmbientLight(float[] colorArray)
        {
            GL.LightModel(LightModelParameter.LightModelAmbient, colorArray);

            if(Lights.Count == 0)            
                GL.Enable(EnableCap.Lighting);
            
        }

        public static LightSource GetLightSource(GameActor parent = null)
        {
            // Yes, this is kinda redundant, but it's easy to monitor them at LightSourceManager level than at the root LightSource level.
            if (Lights.Count == 8)
                return null;

            var light = LightSource.GetLightSource();

            if(light != null)
            {
                if(parent != null)
                {
                    light.ParentActor = parent;
                    parent.ChildActors.Add(light);
                }

                light.IsCollidable = false;
                Lights.Add(light);
            }

            return light;
        }

        public static DirectedLightSource GetDirectedLightSource(GameActor parent = null)
        {
            // Yes, this is kinda redundant, but it's easy to monitor them at LightSourceManager level than at the root LightSource level.
            if (Lights.Count == 8)
                return null;

            var light = DirectedLightSource.GetDirectedLightSource();

            if (light != null)
            {
                if (parent != null)
                {
                    light.ParentActor = parent;
                    parent.ChildActors.Add(light);
                }

                light.IsCollidable = false;
                Lights.Add(light);
            }

            return light;
        }

    }
}
