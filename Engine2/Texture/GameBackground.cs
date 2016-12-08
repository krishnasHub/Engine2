using Engine2.Core;
using Engine2.Util;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Texture
{
    public class GameBackground
    {
        private Texture2D texture;
        private string textureName;
        private Vector2 position;
        private Vector2 scale;

        public int WindowWidth, WindowHeight; 
        public float ZoomFactor;
        public Vector2 Velocity;

        public GameBackground(string textureName)
        {
            this.textureName = textureName;
        }

        public void Init()
        {
            texture = ContentLoader.LoadTexture(textureName);

            if (Velocity == null)
                Velocity = new Vector2(0f, 0f);

            position = new Vector2(0f, 0f);

            if (ZoomFactor == 0f)
                ZoomFactor = 1f;

            if (WindowWidth == 0)
                WindowWidth = 1280;

            if (WindowHeight == 0)
                WindowHeight = 960;

            if (scale == null)
                scale = new Vector2(texture.Width * (float)WindowWidth, texture.Height * (float)WindowHeight);
        }

        public void Tick()
        {
            if (Velocity.X == 0f && Velocity.Y == 0f)
                return;

            position += Velocity;
        }

        public void Render()
        {
            //scale = new Vector2(1f, 1f);
            scale = new Vector2(Constants.GRID_SIZE * (float)WindowWidth / texture.Width, Constants.GRID_SIZE * (float)WindowHeight / texture.Height);
            SpriteBatch.Draw(texture, position, scale, Color.White, Vector2.Zero);
        }
    }
}
