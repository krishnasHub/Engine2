using Engine2.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Engine2.Lighting.Light
{
    public class LightSource : GameActor
    {

        private static int LightSourceCount = 0;

        protected LightName LightName;

        // Default value
        protected float intensity = 4f;

        /// <summary>
        /// Has to be anything greater than 4 in order ot be seen!
        /// </summary>
        public float Intensity
        {
            get { return intensity; }
            set
            {
                if (value <= 4)
                    intensity = 4f;
                else
                    intensity = value;
            }
        }

        ~LightSource()
        {
            LightSource.LightSourceCount--;
        }

        public static LightSource GetLightSource()
        {
            if (LightSourceCount == 8)
                return null;

            var l = new LightSource();
            l.LightName = LightName.Light0 + (LightSource.LightSourceCount - 1);

            return l;
        }

        protected LightSource()
        {
            // No Parent

            isCollidable = false;
            canInit = false;

            LightSource.LightSourceCount++;
        }

        public override void Init()
        {
            // Do not call base.Init();
        }

        public override void Render()
        {
            // Should have it's own render code, not the base class's rendering..

            float xPos = ParentActor != null ? ParentActor.Position.X : Position.X;
            float yPos = ParentActor != null ? ParentActor.Position.Y : Position.Y;

            GL.Light(LightName, LightParameter.Position, new float[] { xPos, yPos, intensity, 1.0f });
            //GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 1f, 0f, 0f, 1f });
            GL.Light(LightName, LightParameter.Diffuse, new float[] { 1.0f, 1f, 0f, 1f });
            //GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            //GL.Light(LightName.Light0, LightParameter.SpotDirection, new float[] { xPos, yPos, zPos });
            //GL.Light(LightName.Light0, LightParameter.SpotCutoff, new float[] { 60f });
            //GL.Light(LightName.Light0, LightParameter.SpotExponent, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.LightModel(LightModelParameter.LightModelAmbient, new float[] { 0.1f, 0.1f, 0.1f, 1.0f });
            //GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
            //GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);
            GL.Enable(EnableCap.Lighting);
            GL.Enable((EnableCap) LightName);
        }

        public override void Tick()
        {
            // Should have it's own Tick event, not the base class's Tick
        }

        public override void onHit(GameActor otherActor)
        {
            // Not much of a collission to be performed with a Light Source!
        }
    }
}
