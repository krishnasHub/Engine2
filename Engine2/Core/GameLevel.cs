using Engine2.Texture;
using Engine2.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Engine2.Core
{
    public abstract class GameLevel
    {
        protected Block[,] grid;
        protected string fileName;
        public Point PlayerStartPos;
        protected Texture2D tileSet;

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
        public Texture2D TileSet
        {
            get { return tileSet; }
        }

        public GameLevel(string levelFileName, string tileSetPath)
        {
            fileName = Constants.RootFolder + "/" + levelFileName;

            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fs);

                int w = int.Parse(doc.DocumentElement.GetAttribute("width"));
                int h = int.Parse(doc.DocumentElement.GetAttribute("height"));                
                tileSet = ContentLoader.LoadTexture(tileSetPath);

                XmlNode tileLayer = doc.DocumentElement.SelectSingleNode("layer[@name='Tile Layer 1']");
                var tiles = tileLayer.SelectSingleNode("data").InnerText.Trim().Split(',');

                // Just added this block.. nothing to it. Please ignore that I did..
                {
                    grid = new Block[w, h];
                    int i = 0;
                    for (int y = 0; y < h; ++y)
                        for (int x = 0; x < w; x++)
                        {
                            int gid = int.Parse(tiles[i]);
                            grid[x, y] = new Block(gid, x, y);
                            i++;
                        }
                }

                XmlNode objLayer = doc.DocumentElement.SelectSingleNode("objectgroup[@name='Object Layer 1']");
                var objects = objLayer.SelectNodes("object");

                for (int i = 0; i < objects.Count; ++i)
                {
                    int xPos = (int)float.Parse(objects[i].Attributes["x"].Value);
                    int yPos = (int)float.Parse(objects[i].Attributes["y"].Value);
                    string objName = objects[i].Attributes["name"].Value;

                    switch (objName)
                    {
                        case "playerStartPos":
                            PlayerStartPos = new Point((int)(xPos / (float)Constants.TILE_SIZE), (int)(yPos / (float)Constants.TILE_SIZE));
                            break;

                        default:
                            break;
                    }
                }

            }
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

        public virtual void Render()
        {

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
