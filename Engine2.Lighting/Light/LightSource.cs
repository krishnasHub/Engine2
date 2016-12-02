using Engine2.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Engine2.Lighting.Light
{
    public class LightSource : GameActor
    {
        protected static int LightSourceCount = 0;

        protected LightName LightName;
        // Default value
        //protected float intensity = 0.9f;
        protected float zIndex = 50f;
        public float ZIndex
        {
            get { return zIndex; }

            set { if (value > 3) zIndex = value; }
        }
        private float getCalculatedintensity(float v)
        {
            return 1f - v / 1000;
        }
        /// <summary>
        /// Has to be between 0 and 1000
        /// </summary>
        protected float Intensity = 100f;
        public void SetIntensity(float value)
        {
            if (value > 0 && value <= 1000f)
                Intensity = value;
        }

        public float GetIntensity()
        {
            return Intensity;
        }
        
        private float[] colorArray = new float[] { 1f, 1f, 1f, 1f };

        public bool IsDiffused = true;
        public bool IsSpecular = false;
        public bool IsAmbient = false;

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

        public void SetColor(Color c)
        {
            colorArray[0] = c.R / (float)byte.MaxValue;
            colorArray[1] = c.G / (float)byte.MaxValue;
            colorArray[2] = c.B / (float)byte.MaxValue;
            colorArray[3] = c.A / (float)byte.MaxValue;
        }

        public void SetColor(float r = 1f, float g = 1f, float b = 1f, float a = 1f)
        {
            colorArray[0] = r;
            colorArray[1] = g;
            colorArray[0] = b;
            colorArray[1] = a;
        }

        protected LightSource()
        {
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

            GL.Light(LightName, LightParameter.Position, new float[] { xPos, yPos, zIndex, 1.0f });

            if(IsDiffused)
                GL.Light(LightName, LightParameter.Diffuse, colorArray);

            if(IsSpecular)
                GL.Light(LightName, LightParameter.Specular, colorArray);

            if(IsAmbient)
                GL.Light(LightName, LightParameter.Ambient, colorArray);

            //GL.Light(LightName.Light0, LightParameter.SpotDirection, new float[] { xPos, yPos, zPos });
            //GL.Light(LightName.Light0, LightParameter.SpotCutoff, new float[] { 60f });
            //GL.Light(LightName.Light0, LightParameter.SpotExponent, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            //GL.LightModel(LightModelParameter.LightModelAmbient, new float[] { 0.1f, 0.1f, 0.1f, 1.0f });
            //GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
            //GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);

            //GL.Light(LightName, LightParameter.SpotCutoff, new float[] { 180f });
            //GL.Light(LightName, LightParameter.SpotDirection, new float[] { 1f, 1f, 0f });
            //GL.Light(LightName, LightParameter.ConstantAttenuation, new float[] { 0.2f });

            var intensity = getCalculatedintensity(Intensity);
            GL.Light(LightName, LightParameter.ConstantAttenuation, intensity);

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
