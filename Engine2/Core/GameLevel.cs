using Engine2.Actor;
using Engine2.Texture;
using Engine2.Util;
using OpenTK;
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
        public Vector2 ViewCenterPos;
        public GameActor BoundActor;
        protected Texture2D tileSet;
        protected List<GameActor> actors;

        protected Vector2 tileSize;
        protected int blocksInRow;

        protected Dictionary<int, IBlockPhysics> gridPhysicsMap;

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
        public List<GameActor> Actors
        {
            get { return actors; }
        }

        public void SetBlockPhysics(int blockType, IBlockPhysics blockPhysics)
        {
            if (gridPhysicsMap == null)
                gridPhysicsMap = new Dictionary<int, IBlockPhysics>();

            gridPhysicsMap[blockType] = blockPhysics;
        }

        public void AddActor(GameActor a)
        {
            if (actors == null)
                actors = new List<GameActor>();

            actors.Add(a);
        }

        public GameLevel(string levelFileName)
        {
            fileName = Constants.RootFolder + "/" + levelFileName;
            ViewCenterPos = new Vector2(0f, 0f);

            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fs);

                int w = int.Parse(doc.DocumentElement.GetAttribute("width"));
                int h = int.Parse(doc.DocumentElement.GetAttribute("height"));


                XmlNode tileLayer = doc.DocumentElement.SelectSingleNode("layer[@name='Tile Layer 1']");
                var tiles = tileLayer.SelectSingleNode("data").InnerText.Trim().Split(',');

                var imgNode = doc.DocumentElement.SelectSingleNode("tileset").SelectSingleNode("image");
                var tileSetPath = imgNode.Attributes["source"].InnerText;

                var tWidth = doc.DocumentElement.SelectSingleNode("tileset").Attributes["tilewidth"].InnerText;
                var tHeight = doc.DocumentElement.SelectSingleNode("tileset").Attributes["tileheight"].InnerText;
                tileSize = new Vector2(int.Parse(tWidth), int.Parse(tHeight));

                var columns = doc.DocumentElement.SelectSingleNode("tileset").Attributes["columns"].InnerText;
                blocksInRow = int.Parse(columns);

                tileSet = ContentLoader.LoadTexture(tileSetPath);

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

        protected RectangleF GetSourceRectangle(int row, int column)
        {
            return new RectangleF(column * tileSize.X, row * tileSize.Y, tileSize.X, tileSize.Y);
        }

        public virtual void Render()
        {
            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    RectangleF source;

                    if (this[x, y].Type == 0)
                        source = new RectangleF(0, 0, 0, 0);
                    else
                    {
                        int r = (this[x, y].Type - 1) / blocksInRow;
                        int c = (this[x, y].Type - 1) % blocksInRow;

                        source = GetSourceRectangle(r, c);
                    }                    

                    DrawSprite(source, x, y);
                }
            }

            foreach (var a in actors)
            {
                a.Render();
            }
        }

        public virtual void Init()
        {
            foreach (var a in actors)
            {
                a.Init();
            }

            var bindActor = actors.FirstOrDefault(a => a.BindToView);
            if(bindActor != null)
            {
                BoundActor = bindActor;
                ViewCenterPos = BoundActor.Position;
            }
        }

        public virtual void Tick()
        {
            // Tick every actor
            foreach(var a in actors)
            {
                a.Tick();
            }

            // Reposition the camera if the bound actor has moved out of frame
            if (BoundActor != null)
            {
                var pos = BoundActor.Position;

                if (!WorldSettings.View.IsInbounds(pos))
                    WorldSettings.View.SetPosition(pos, TweenType.QuarticOut, Constants.TWEEN_SPEED);
            }

            // Check collissions for every actor with the surrounding blocks in the levels that it is colliding with
            foreach(var a in actors)
            {
                if (gridPhysicsMap != null)
                {
                    var l = a.Position / Constants.GRID_SIZE;
                    var xl = (int)l.X - 5;
                    var yl = (int)l.Y - 5;
                    var xr = ((int)(a.Position.X + a.BoundingBox.Right) / Constants.GRID_SIZE) + 5;
                    var yr = ((int)(a.Position.Y + a.BoundingBox.Bottom) / Constants.GRID_SIZE) + 5;

                    for (int x = xl; x <= xr; x++)
                        for (int y = yl; y <= yr; y++)
                        {
                            if (!gridPhysicsMap.ContainsKey(this[x, y].Type))
                                continue;

                            var p = gridPhysicsMap[this[x, y].Type];

                            if(p != null)
                            {
                                if(p.CheckCollision(a, new Vector2(x * Constants.GRID_SIZE, y * Constants.GRID_SIZE),
                                    new Vector2(Constants.GRID_SIZE, Constants.GRID_SIZE)))
                                {
                                    p.HandleCollission(a);
                                }
                            }
                        }
                }
            }

            // Check for collissions between each actor
            actors.ForEach(a =>
            {
                if (a.PhysicsComponent != null)
                    actors.Where(c => a != c).ToList().ForEach(b =>
                    {
                        if (a.PhysicsComponent.CheckCollission(a, b))
                        {
                            a.onHit(b);
                            b.onHit(a);
                        }
                    });
            });

        }

        protected void DrawSprite(RectangleF source, int x, int y)
        {
            if (source != null)
                SpriteBatch.Draw(tileSet, new Vector2(x * Constants.GRID_SIZE, y * Constants.GRID_SIZE),
                    new Vector2((float)Constants.GRID_SIZE / Constants.TILE_SIZE), Color.White, Vector2.Zero, source);
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
