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
using Engine2.Input;

namespace Engine2.Core
{
    public class Game : GameWindow
    {
        View view;
        private GameLevel level;
        private bool isLoaded = false;

        public void SetLevel(GameLevel gameLevel)
        {
            level = gameLevel;

            if(isLoaded)
                view.SetPosition(view.ToWorld(new Vector2(level.PlayerStartPos.X + Width / 2, level.PlayerStartPos.Y + Height / 2)), 
                    TweenType.QuarticOut, Constants.TWEEN_SPEED);
        }

        public Game(int width, int height) : base(width, height)
        {
            GL.Enable(EnableCap.Texture2D);
            view = new View(Vector2.Zero, 0.0f, 0.75f);
            WorldSettings.View = view;
            GameInput.Initialize(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            view.SetPosition(view.ToWorld(new Vector2(level.PlayerStartPos.X + Width / 2, level.PlayerStartPos.Y + Height / 2)), 
                TweenType.QuarticOut, Constants.TWEEN_SPEED);

            isLoaded = true;
            level.Init();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if(GameInput.MouseButtonPress(OpenTK.Input.MouseButton.Left))
            {
                Vector2 pos = new Vector2(Mouse.X, Mouse.Y);
                pos -= new Vector2(this.Width, this.Height) / 2f;
                pos = view.ToWorld(pos);

                view.SetPosition(pos, TweenType.QuarticOut, Constants.TWEEN_SPEED);
            }

            if(GameInput.KeyPress(OpenTK.Input.Key.Right))            
                view.SetPosition(view.PositionGoTo + new Vector2(5, 0), TweenType.QuarticOut, Constants.TWEEN_SPEED);

            if (GameInput.KeyPress(OpenTK.Input.Key.Left))
                view.SetPosition(view.PositionGoTo + new Vector2(-5, 0), TweenType.QuarticOut, Constants.TWEEN_SPEED);

            if (GameInput.KeyPress(OpenTK.Input.Key.Up))
                view.SetPosition(view.PositionGoTo + new Vector2(0, -5), TweenType.QuarticOut, Constants.TWEEN_SPEED);

            if (GameInput.KeyPress(OpenTK.Input.Key.Down))
                view.SetPosition(view.PositionGoTo + new Vector2(0, 5), TweenType.QuarticOut, Constants.TWEEN_SPEED);


            view.Update();
            GameInput.Update();
            level.Tick();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.CornflowerBlue);

            SpriteBatch.Begin(this.Width, this.Height);
            view.ApplyTransform();

            level.Render();

            this.SwapBuffers();
        }
    }
}
