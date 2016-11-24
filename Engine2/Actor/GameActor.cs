using Engine2.Core;
using Engine2.Texture;
using Engine2.Util;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Actor
{
    public class GameActor
    {
        private string textureName;
        Texture2D Texture;

        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Scale;
        public RectangleF BoundingBox;

        public bool BindToView = false;

        public GameActor()
        {
            textureName = "";
        }

        public GameActor(string textureName)
        {
            this.textureName = textureName;
        }

        public void SetScale(float sx, float sy)
        {
            Scale = new Vector2(sx, sy);
        }

        public void SetScale(float value)
        {
            Scale = new Vector2(value, value);
        }

        public virtual void Init()
        {
            Texture = ContentLoader.LoadTexture(this.textureName);

            if (Scale == null)
                Scale = new Vector2(1f, 1f);

            if (BoundingBox == null)
                BoundingBox = new RectangleF(0, 0, Texture.Width * Scale.X, Texture.Height * Scale.Y);

            if (Velocity == null)
                Velocity = new Vector2(0f, 0f);

            if (Position == null)
                Position = new Vector2(0f, 0f);
        }

        public virtual void Tick()
        {
            Position += Velocity;
        }

        public virtual void Render()
        {
            SpriteBatch.Draw(Texture, Position, Scale, Color.White, Vector2.Zero);
        }

    }
}
