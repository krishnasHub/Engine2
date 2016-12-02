using Engine2.Actor;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Lighting.Light
{
    public class DirectedLightSource : LightSource
    {

        public Vector2 Direction
        {
            set { directionValues[0] = value.X; directionValues[1] = value.Y; }
            get { return new Vector2(directionValues[0], directionValues[1]); }
        }

        private float[] directionValues = new float[3];

        public float DirectionZValue = -1f;
        
        public float Angle = 20f;

        protected DirectedLightSource() :base()
        {
            // Do nothing extra
            isCollidable = false;
            canInit = false;

            directionValues[2] = DirectionZValue;
        }

        public static DirectedLightSource GetDirectedLightSource()
        {
            if (LightSourceCount == 8)
                return null;

            var l = new DirectedLightSource();
            l.LightName = LightName.Light0 + (LightSource.LightSourceCount - 1);

            return l;
        }

        public override void Render()
        {
            base.Render();

            directionValues[2] = DirectionZValue;

            //GL.Light(LightName, LightParameter.ConstantAttenuation, new float[] { 0.5f });

            GL.Light(LightName, LightParameter.SpotExponent, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });

            GL.Light(LightName, LightParameter.SpotCutoff, Angle);
            GL.Light(LightName, LightParameter.SpotDirection, directionValues);

        }

        public override void Tick()
        {
            // Should have it's own Tick event, not the base class's Tick
        }

        public override void onHit(GameActor otherActor)
        {
            // Not much of a collission to be performed with a Light Source!
        }

        public override void Init()
        {
            // Do not call base.Init();
        }

    }
}
