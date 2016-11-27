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


        /*
        #region Shaders

        int pgmID;

        int vsID;
        int fsID;

        Vector3[] vertdata;
        Vector3[] coldata;
        Matrix4[] mviewdata;


        int attribute_vcol;
        int attribute_vpos;
        int uniform_mview;

        int vbo_position;
        int vbo_color;
        int vbo_mview;

        private void initProgram()
        {
            pgmID = GL.CreateProgram();

            loadShader("Content/Shaders/vs.glsl", ShaderType.VertexShader, pgmID, out vsID);
            loadShader("Content/Shaders/fs.glsl", ShaderType.FragmentShader, pgmID, out fsID);


            GL.LinkProgram(pgmID);
            Console.WriteLine(GL.GetProgramInfoLog(pgmID));

            attribute_vpos = GL.GetAttribLocation(pgmID, "vPosition");
            attribute_vcol = GL.GetAttribLocation(pgmID, "vColor");
            uniform_mview = GL.GetUniformLocation(pgmID, "modelview");

            if (attribute_vpos == -1 || attribute_vcol == -1 || uniform_mview == -1)
            {
                Console.WriteLine("Error binding attributes");
            }

            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_color);
            GL.GenBuffers(1, out vbo_mview);
        }

        private void loadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);

            using (var sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }

            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));

        }

        private void onShaderLoad()
        {
            initProgram();

            vertdata = new Vector3[] { new Vector3(-0.8f, -0.8f, 0f),
                new Vector3( 0.8f, -0.8f, 0f),
                new Vector3( 0f,  0.8f, 0f)};


            coldata = new Vector3[] { new Vector3(1f, 0f, 0f),
                new Vector3( 0f, 0f, 1f),
                new Vector3( 0f,  1f, 0f)};


            mviewdata = new Matrix4[]{
                Matrix4.Identity
            };
        }

        private void UpdateShaderFrame()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vpos, 3, VertexAttribPointerType.Float, false, 0, 0);


            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attribute_vcol, 3, VertexAttribPointerType.Float, true, 0, 0);

            GL.UniformMatrix4(uniform_mview, false, ref mviewdata[0]);

            GL.UseProgram(pgmID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.EnableVertexAttribArray(attribute_vpos);
            GL.EnableVertexAttribArray(attribute_vcol);
        }

        private void RenderShaderFrame()
        {
            GL.Enable(EnableCap.DepthTest);


            GL.EnableVertexAttribArray(attribute_vpos);
            GL.EnableVertexAttribArray(attribute_vcol);


            GL.DrawArrays(BeginMode.Triangles, 0, 3);


            GL.DisableVertexAttribArray(attribute_vpos);
            GL.DisableVertexAttribArray(attribute_vcol);


            GL.Flush();
        }

        #endregion
        */

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

            

            this.SwapBuffers();
        }
    }
}
