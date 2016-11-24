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

        public void SetLevel(GameLevel gameLevel)
        {
            level = gameLevel;
        }

        public Game(int width, int height, float zoom = 1f, float rotation = 0f) : base(width, height)
        {
            GL.Enable(EnableCap.Texture2D);
            view = new View(new Vector2(width / 2, height / 2), new Vector2(width, height), rotation, zoom);
            WorldSettings.View = view;
            WorldSettings.Height = this.Height;
            WorldSettings.Width = this.Width;
            GameInput.Initialize(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            level.Init();

            // Set the View to center on the level
            if(level.BoundActor != null)
            {
                view.SetPosition(view.ToWorld(level.BoundActor.Position), TweenType.QuarticOut, Constants.TWEEN_SPEED);
            }
            
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if(GameInput.MouseButtonPress(OpenTK.Input.MouseButton.Left))
            {
                var pos = WorldSettings.TranslateFromScreenToGameCoords(new Vector2(Mouse.X, Mouse.Y));
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
