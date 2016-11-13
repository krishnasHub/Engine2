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
    public class View
    {
        public Vector2 position;

        /// <summary>
        /// radians.
        /// + value => clockwise
        /// </summary>
        public float rotation;
        public float zoom;

        public View(Vector2 startPosition, float startRotation = 0.0f, float startZoom = 0.0f)
        {
            position = startPosition;
            rotation = startRotation;
            zoom = startZoom;
        }

        public void Update()
        {

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
