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
using OpenTK.Input;
using System.IO;

namespace Engine2.Core
{
    public class Game : GameWindow
    {
        View view;
        private GameLevel level;

        private Dictionary<Key, List<Action<InputEvent>>> keyEventMap;
        private Dictionary<MouseButton, List<Action<InputEvent>>> mouseEventMap;


        #region Lighting Region

        float xPos = 0f;
        float yPos = 0f;
        float zPos = 50f;

        protected void OnColorLoad(Vector2 position)
        {
            //GL.Enable(EnableCap.DepthTest);
            
            xPos = position.X;
            yPos = position.Y;

            GL.Light(LightName.Light0, LightParameter.Position, new float[] { xPos, yPos, zPos, 1.0f });
            //GL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 1f, 0f, 0f, 1f });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 1.0f, 1f, 0f, 1f });
            //GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            //GL.Light(LightName.Light0, LightParameter.SpotDirection, new float[] { xPos, yPos, zPos });
            //GL.Light(LightName.Light0, LightParameter.SpotCutoff, new float[] { 60f });
            //GL.Light(LightName.Light0, LightParameter.SpotExponent, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.LightModel(LightModelParameter.LightModelAmbient, new float[] { 0.1f, 0.1f, 0.1f, 1.0f });
            //GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
            //GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
        }

        #endregion

        public void AddKeyEvent(Key key, Action<InputEvent> ev)
        {
            if (keyEventMap == null)
                keyEventMap = new Dictionary<Key, List<Action<InputEvent>>>();

            if (!keyEventMap.ContainsKey(key))
                keyEventMap[key] = new List<Action<InputEvent>>();

            keyEventMap[key].Add(ev);
        }

        public void AddMouseEvent(MouseButton mouse, Action<InputEvent> ev)
        {
            if (mouseEventMap == null)
                mouseEventMap = new Dictionary<MouseButton, List<Action<InputEvent>>>();

            if (!mouseEventMap.ContainsKey(mouse))
                mouseEventMap[mouse] = new List<Action<InputEvent>>();

            mouseEventMap[mouse].Add(ev);
        }

        public void SetLevel(GameLevel gameLevel)
        {
            level = gameLevel;
        }

        public Game(int width, int height, float zoom = 1f, float rotation = 0f) : base(width, height)
        {
            Title = "New Game";

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            
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

            //onShaderLoad();
        }


        private void checkKeyEvents()
        {
            if (keyEventMap != null)
                foreach (var key in keyEventMap.Keys)
                {
                    keyEventMap[key].ForEach(a =>
                    {
                        var inputEvent = new InputEvent(key);
                        inputEvent.IsPressed = GameInput.KeyPress(key);
                        inputEvent.IsReleased = GameInput.KeyRelease(key);
                        inputEvent.IsDown = GameInput.KeyDown(key);

                        if(inputEvent.IsPressed || inputEvent.IsReleased || inputEvent.IsDown)
                            a.Invoke(inputEvent);
                    });
                }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            //UpdateShaderFrame();

            view.Size = new Vector2(this.Width, this.Height);

            if(GameInput.MouseButtonPress(OpenTK.Input.MouseButton.Left))
            {
                var pos = WorldSettings.TranslateFromScreenToGameCoords(new Vector2(Mouse.X, Mouse.Y));
                view.SetPosition(pos, TweenType.QuarticOut, Constants.TWEEN_SPEED);
            }


            checkKeyEvents();

            view.Update();
            GameInput.Update();
            level.Tick();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.CornflowerBlue);

            //RenderShaderFrame();

            SpriteBatch.Begin(this.Width, this.Height);
            view.ApplyTransform();

            level.Render();

            OnColorLoad(level.Actors[0].Position);

            this.SwapBuffers();
        }
    }
}
