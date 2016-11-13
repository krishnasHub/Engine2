using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Engine2.Util;
using Engine2.Texture;

namespace Engine2.Core
{
    class Game : GameWindow
    {

        Texture2D t1, t2;

        public Game(int width, int height) : base(width, height)
        {
            GL.Enable(EnableCap.Texture2D);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            t1 = ContentLoader.LoadTexture("tile_wall.jpg");
            t2 = ContentLoader.LoadTexture("tile_grass.png");

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.CornflowerBlue);

            GL.BindTexture(TextureTarget.Texture2D, t1.Id);
            GL.Begin(PrimitiveType.Quads);

            GL.Color3(Color.Red);
            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);

            GL.Color3(Color.Black);
            GL.TexCoord2(1, 0);
            GL.Vertex2(1, 0);

            GL.Color3(Color.Green);
            GL.TexCoord2(1, 1);
            GL.Vertex2(1, -1);

            GL.Color3(Color.Yellow);
            GL.TexCoord2(0, 1);
            GL.Vertex2(0, -1);

            GL.End();






            GL.BindTexture(TextureTarget.Texture2D, t2.Id);
            GL.Begin(PrimitiveType.Quads);

            GL.Color3(Color.Red);
            GL.TexCoord2(0, 0);
            GL.Vertex2(-0.5f, 0.5f);

            GL.Color3(Color.Black);
            GL.TexCoord2(1, 0);
            GL.Vertex2(0.5f, 0.5f);

            GL.Color3(Color.Green);
            GL.TexCoord2(1, 1);
            GL.Vertex2(0.5f, -0.5f);

            GL.Color3(Color.Yellow);
            GL.TexCoord2(0, 1);
            GL.Vertex2(-0.5f, -0.5f);

            GL.End();

            this.SwapBuffers();
        }
    }
}
