using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using System.Drawing;
using Engine2.Util;
using Engine2.Texture;

namespace Engine2.Core
{
    public class Level
    {
        private Block[,] grid;
        private string fileName;
        public Point PlayerStartPos;

        public Block this[int x, int y]
        {
            get { return grid[x, y]; }
            set { grid[x, y] = value; }
        }
        public string FileName
        {
            get { return fileName; }
        }
        public int Width
        {
            get { return grid.GetLength(0); }
        }
        public int Height
        {
            get { return grid.GetLength(1); }
        }

        public Level(int w, int h, string fileName = "none")
        {
            grid = new Block[w, h];
            this.fileName = fileName;
            PlayerStartPos = new Point(1, 1);
        }

        private void LoadDefault(int w, int h)
        {
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (x == 0 || y == 0 || x == w - 1 || y == h - 1)
                        grid[x, y] = new Block(1, x, y);
                    else
                        grid[x, y] = new Block(0, x, y);
                }
            }
        }

        public void SetBlock(int x, int y, int type)
        {
            grid[x, y] = new Block(type, x, y);
        }

        public void Render(Texture2D tileSet)
        {
            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    RectangleF source;

                    switch (this[x, y].Type)
                    {
                        case 2:
                            source = new RectangleF(1 * Constants.TILE_SIZE, 0 * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE);
                            break;

                        case 3:
                            source = new RectangleF(2 * Constants.TILE_SIZE, 0 * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE);
                            break;

                        case 4:
                            source = new RectangleF(3 * Constants.TILE_SIZE, 0 * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE);
                            break;

                        case 1:
                            source = new RectangleF(0 * Constants.TILE_SIZE, 0 * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE);
                            break;

                        case 5:
                            source = new RectangleF(0 * Constants.TILE_SIZE, 1 * Constants.TILE_SIZE, Constants.TILE_SIZE, Constants.TILE_SIZE);
                            break;

                        default:
                            source = new RectangleF(0, 0, 0, 0);
                            break;
                    }

                    if(source != null)
                        SpriteBatch.Draw(tileSet, new Vector2(x * Constants.GRID_SIZE, y * Constants.GRID_SIZE),
                            new Vector2((float)Constants.GRID_SIZE / Constants.TILE_SIZE), Color.White, Vector2.Zero, source);
                }
            }
        }
    }


    public struct Block
    {
        private int type;
        private int posX, posY;

        public Block(int type, int x, int y)
        {
            this.type = type;
            this.posX = x;
            this.posY = y;
        }

        public int Type
        {
            get { return type; }
        }

        public int X
        {
            get { return posX; }
        }

        public int Y
        {
            get { return posY; }
        }
    }
}
