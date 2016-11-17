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
    class Game : GameWindow
    {
        

        View view;
        Texture2D t1, t2;
        Texture2D tileSet;
        Level level;

        public Game(int width, int height) : base(width, height)
        {
            GL.Enable(EnableCap.Texture2D);
            view = new View(Vector2.Zero, 0.0f, 2f);
            GameInput.Initialize(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            t1 = ContentLoader.LoadTexture("tile_wall.jpg");
            t2 = ContentLoader.LoadTexture("tile_grass.png");

            tileSet = ContentLoader.LoadTexture("tile_set1.png");
            level = ContentLoader.LoadLevel("Level1.tmx");
            view.SetPosition(new Vector2(level.PlayerStartPos.X, level.PlayerStartPos.Y), TweenType.QuarticOut, Constants.TWEEN_SPEED);
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

            for (int x = 0; x < level.Width; x++)
            {
                for (int y = 0; y < level.Height; y++)
                {
                    RectangleF source = new RectangleF(0, 0, 0, 0);

                    switch(level[x, y].Type)
                    {
                        case BlockType.Ladder:
                            source = new RectangleF(2 * Constants.TILE_SIZE, 0 * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE);
                            break;

                        case BlockType.LadderPlatform:
                            source = new RectangleF(3 * Constants.TILE_SIZE, 0 * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE);
                            break;

                        case BlockType.Solid:
                            source = new RectangleF(1 * Constants.TILE_SIZE, 0 * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE);
                            break;

                        case BlockType.Platform:
                            source = new RectangleF(0 * Constants.TILE_SIZE, 1 * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE);
                            break;
                    }

                    SpriteBatch.Draw(tileSet, new Vector2(x * Constants.GRID_SIZE, y * Constants.GRID_SIZE),
                        new Vector2((float)Constants.GRID_SIZE / Constants.TILE_SIZE), Color.White, Vector2.Zero, source);
                }
            }

                    this.SwapBuffers();
        }
    }
}
