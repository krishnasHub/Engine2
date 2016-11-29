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
    public class TileSheetManager
    {
        private Texture2D tileSheet;
        private string tileSheetName;

        public int StartRow = 0;
        public int StartColumn = 0;
        public int Rows;
        public int Columns;
        public float SpriteWidth;
        public float SpriteHeight;

        public int CurrentSprite = 1;

        public void Init()
        {
            tileSheet = ContentLoader.LoadTexture(tileSheetName);
        }

        public TileSheetManager(string tileSheetName)
        {
            this.tileSheetName = tileSheetName;
        }

        public void Render(Vector2 position, Vector2 scale)
        {
            int r = (CurrentSprite - 1) / Columns;
            int c = (CurrentSprite - 1) % Columns;

            float x = c * SpriteWidth;
            float y = r * SpriteHeight;

            RectangleF source = new RectangleF(x, y, SpriteWidth, SpriteHeight);

            SpriteBatch.Draw(tileSheet, position, scale, Color.White, Vector2.Zero, source);
        }
    }
}
