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
        View view;
        Texture2D t1, t2;

        public Game(int width, int height) : base(width, height)
        {
            GL.Enable(EnableCap.Texture2D);
            view = new View(Vector2.Zero, 0.0f, 0.5f);
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

            view.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.CornflowerBlue);

            SpriteBatch.Begin(this.Width, this.Height);
            view.ApplyTransform();

            SpriteBatch.Draw(t1, Vector2.Zero, new Vector2(1, 1), Color.Wheat, Vector2.Zero);
            SpriteBatch.Draw(t2, new Vector2(-800f, -500f), new Vector2(1, 1), Color.WhiteSmoke, Vector2.Zero);
            

            this.SwapBuffers();
        }
    }
}
