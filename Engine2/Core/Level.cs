using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using System.Drawing;
using System.IO;
using System.Xml;

namespace Engine2.Core
{
    public struct Level
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

        public Level(int w, int h)
        {
            grid = new Block[w, h];
            fileName = "none";
            PlayerStartPos = new Point(1, 1);

            LoadDefault(w, h);
        }

        private void LoadDefault(int w, int h)
        {
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (x == 0 || y == 0 || x == w - 1 || y == h - 1)
                        grid[x, y] = new Block(BlockType.Solid, x, y);
                    else
                        grid[x, y] = new Block(BlockType.Empty, x, y);
                }
            }
        }

        public Level(string filePath)
        {
            try
            {
                fileName = filePath;


                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fs);

                    int w = int.Parse(doc.DocumentElement.GetAttribute("width"));
                    int h = int.Parse(doc.DocumentElement.GetAttribute("height"));

                    grid = new Block[w, h];
                    PlayerStartPos = new Point(1, 1);

                    XmlNode tileLayer = doc.DocumentElement.SelectSingleNode("layer[@name='Tile Layer 1']");
                    var tiles = tileLayer.SelectSingleNode("data").InnerText.Trim().Split(',');

                    {
                        int i = 0;
                        for (int y = 0; y < h; ++y)
                            for (int x = 0; x < w; x++)                            
                            {
                                int gid = int.Parse(tiles[i]);

                                switch(gid)
                                {
                                    case 2:
                                        grid[x, y] = new Block(BlockType.Solid, x, y);
                                        break;
                                    case 3:
                                        grid[x, y] = new Block(BlockType.Ladder, x, y);
                                        break;
                                    case 4:
                                        grid[x, y] = new Block(BlockType.LadderPlatform, x, y);
                                        break;
                                    case 5:
                                        grid[x, y] = new Block(BlockType.Platform, x, y);
                                        break;

                                    default:
                                        grid[x, y] = new Block(BlockType.Empty, x, y);
                                        break;
                                }
                                i++;
                            }
                    }
                }
            }
            catch (Exception e)
            {
                grid = new Block[100, 100];
                fileName = "none";
                PlayerStartPos = new Point(1, 1);

                LoadDefault(100, 100);
            }
        }
    }


    public enum BlockType
    {
        Solid,
        Empty,
        Platform,
        Ladder,
        LadderPlatform
    }

    public struct Block
    {
        private BlockType type;
        private int posX, posY;
        private bool solid, platform, ladder;

        public Block(BlockType type, int x, int y)
        {
            this.type = type;
            this.posX = x;
            this.posY = y;

            solid = false;
            platform = false;
            ladder = false;

            switch(type)
            {
                case BlockType.Solid:
                    solid = true;
                    break;

                case BlockType.Platform:
                    platform = true;
                    break;

                case BlockType.Ladder:
                    ladder = true;
                    break;

                case BlockType.LadderPlatform:
                    platform = true;
                    ladder = true;
                    break;
            }
        }

        public BlockType Type
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

        public bool IsSolid
        {
            get { return solid; }
        }

        public bool IsPlatform
        {
            get { return platform; }
        }

        public bool IsLadder
        {
            get { return ladder; }
        }
    }
}
