using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Engine2.Core
{
    public enum TweenType
    {
        Instant,
        Linear,
        QuadraticInOut,
        CubicInOut,
        QuarticOut

    }

    public class View
    {
        private Vector2 position;

        /// <summary>
        /// radians.
        /// + value => clockwise
        /// </summary>
        public float rotation;
        public float zoom;

        private Vector2 positionGoTo;
        private Vector2 positionFrom;
        private TweenType tweenType;

        private int currentStep, tweenStep;

        public Vector2 Position
        {
            get { return this.position; }
        }
        public Vector2 PositionGoTo
        {
            get { return this.positionGoTo; }
        }

        public Vector2 ToWorld(Vector2 input)
        {
            input /= zoom;
            Vector2 dx = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            Vector2 dy = new Vector2((float)Math.Cos(rotation + MathHelper.PiOver2), (float)Math.Sin(rotation + MathHelper.PiOver2));

            return (this.position + (dx * input.X) + (dy * input.Y));
        }

        public View(Vector2 startPosition, float startRotation = 0.0f, float startZoom = 1f)
        {
            position = startPosition;
            rotation = startRotation;
            zoom = startZoom;
        }

        public void Update()
        {
            if(currentStep < tweenStep)
            {
                currentStep++;

                switch(tweenType)
                {
                    case TweenType.Linear:
                        this.position = positionFrom + (positionGoTo - positionFrom) * GetLinear((float) currentStep / tweenStep);
                        break;

                    case TweenType.QuadraticInOut:
                        this.position = positionFrom + (positionGoTo - positionFrom) * GetQuadraticInOut((float)currentStep / tweenStep);
                        break;

                    case TweenType.CubicInOut:
                        this.position = positionFrom + (positionGoTo - positionFrom) * GetCubicInOut((float)currentStep / tweenStep);
                        break;

                    case TweenType.QuarticOut:
                        this.position = positionFrom + (positionGoTo - positionFrom) * GetQuarticOut((float)currentStep / tweenStep);
                        break;
                }
            }
            else
            {
                position = positionGoTo;
            }
        }

        private float GetQuarticOut(float v)
        {
            return -((v - 1) * (v - 1) * (v - 1) * (v - 1)) + 1;
        }

        private float GetCubicInOut(float v)
        {
            return (v * v * v) / ((3 * v * v) - (3 * v) + 1);
        }

        private float GetQuadraticInOut(float v)
        {
            return (v * v) / ((2 * v * v) - (2 * v) + 1);
        }

        private float GetLinear(float v)
        {
            return v;
        }

        public void SetPosition(Vector2 newPosition)
        {
            this.position = newPosition;
            this.positionFrom = newPosition;
            this.positionGoTo = newPosition;

            tweenType = TweenType.Instant;
            currentStep = 0;
            tweenStep = 0;
        }

        public void SetPosition(Vector2 newPosition, TweenType type, int numSteps)
        {
            this.positionFrom = this.position;
            this.position = newPosition;
            this.positionGoTo = newPosition;

            tweenType = type;
            currentStep = 0;
            tweenStep = numSteps;
        }

        public void ApplyTransform()
        {
            Matrix4 transform = Matrix4.Identity;

            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-position.X, -position.Y, 0));
            transform = Matrix4.Mult(transform, Matrix4.CreateRotationZ(-rotation));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(zoom, zoom, 1.0f));

            GL.MultMatrix(ref transform);
        }

    }
}
