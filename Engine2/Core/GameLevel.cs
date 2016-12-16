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
    /// <summary>
    /// This is the basic Level class that is used to get a Level ready on screen. You can subclass this to define your own Levels.
    /// This is not an abstract class, so you can directly create an object of it, give it a tmx file to load the level and watch it rendered.
    /// </summary>
    public abstract class GameLevel
    {
        protected Block[,] grid;
        protected string fileName;
        public Vector2 ViewCenterPos;
        public GameActor BoundActor;
        protected Texture2D tileSet;
        protected static List<GameActor> actors = new List<GameActor>();
        protected Vector2 tileSize;
        protected int blocksInRow;
        protected Dictionary<int, IBlockPhysics> gridPhysicsMap;

        public ILevelPhysics LevelPhysics;

        public GameBackground GameBackground;

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


        public void SetBlockPhysics(int blockType, IBlockPhysics blockPhysics)
        {
            if (gridPhysicsMap == null)
                gridPhysicsMap = new Dictionary<int, IBlockPhysics>();

            gridPhysicsMap[blockType] = blockPhysics;
        }


        public static List<GameActor> GetAllActors()
        {
            var l = new List<GameActor>();

            actors.ForEach(a => l.Add(a));

            return l;
        }

        public void AddActor(GameActor a)
        {
            if (a == null)
                return;

            if (actors == null)
                actors = new List<GameActor>();

            a.ParentLevel = this;

            if (!actors.Contains(a))
                actors.Add(a);
        }

        public bool CanActorMoveTo(GameActor actor, Vector2 pos)
        {
            if (gridPhysicsMap == null)
                return true;

            // We need the GRID co-ordinates of the actor's boundaries.
            // Get the top left block where the actor intersects.
            // Get the bottom right block where the actor intersects.
            var pos1 = pos / Constants.GRID_SIZE;
            // dx and dy are the width and height of the actor.
            var dx = (actor.BoundingShape == BoundingShape.BoundingBox ? actor.BoundingBox.Right : actor.BoundingRadius * 2f);
            var dy = (actor.BoundingShape == BoundingShape.BoundingBox ? actor.BoundingBox.Bottom : actor.BoundingRadius * 2f); 
            // The new position would be pos + the width and height of the actor.
            var pos2 = pos + new Vector2(dx, dy);
            pos2 /= Constants.GRID_SIZE;
            // I feel shitty about adding this line, but turns out, adding this makes motion smoother.
            pos2 -= new Vector2(0.05f, 0.05f);

            // Now loop through all the blocks from pos1 through pos2.
            // If the actor cannot collide with any of these set of blocks, return false.
            // If the actor can collide with all of these blocks, return true.
            for (int x = (int) pos1.X; x <= (int) pos2.X; ++x)
                for (int y = (int)pos1.Y; y <= (int)pos2.Y; ++y)
                {
                    // Get the type of this block
                    var type = this[x, y].Type;

                    // If we have no Physics set to this type of block, move on.
                    if (!gridPhysicsMap.ContainsKey(type))
                        continue;

                    // Get the block's physical position
                    var blockPos = new Vector2(this[x, y].X * Constants.GRID_SIZE, this[x, y].Y * Constants.GRID_SIZE);

                    // If the actor is colliding with this block, then return false.
                    if (gridPhysicsMap[type].CheckCollision(actor, blockPos, new Vector2(Constants.GRID_SIZE, Constants.GRID_SIZE)))
                    {
                        var value = gridPhysicsMap[type].CanStepIntoMe();
                        if (value)
                            continue;
                        return false;
                    }
                        
                }

            return true;
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


                XmlNode tileLayer = doc.DocumentElement.SelectSingleNode("layer[@name='Tile Layer']");
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
            if (GameBackground != null)
                GameBackground.Render();

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

        private void removeDestroyedActors()
        {
            var deletedActorCount = actors.RemoveAll(a => a.ReadyToBeDestroyed);

            if(deletedActorCount > 0)
                Console.WriteLine("Deleted " + deletedActorCount + " actors.");
        }

        public virtual void Init()
        {
            foreach (var a in actors)
            {
                if(a.CanInit)
                    a.Init();
            }

            var bindActor = actors.FirstOrDefault(a => a.BindToView);
            if(bindActor != null)
            {
                BoundActor = bindActor;
                ViewCenterPos = BoundActor.Position;
            }

            if (GameBackground != null)
            {
                GameBackground.WindowWidth = Width;
                GameBackground.WindowHeight = Height;
                GameBackground.Init();
            }
        }

        private void repositionView()
        {
            if (BoundActor != null)
            {
                var pos = BoundActor.Position;

                if (!WorldSettings.View.IsInbounds(pos))
                    WorldSettings.View.SetPosition(pos, TweenType.QuarticOut, Constants.TWEEN_SPEED);
            }
        }


        private void CheckcollissionWithBlocks(GameActor a)
        {
            var l = a.Position / Constants.GRID_SIZE;
            var xl = (int)l.X - 5;
            var yl = (int)l.Y - 5;
            var xr = ((int)(a.Position.X + a.BoundingBox.Right) / Constants.GRID_SIZE) + 5;
            var yr = ((int)(a.Position.Y + a.BoundingBox.Bottom) / Constants.GRID_SIZE) + 5;

            for (int x = xl; x <= xr; x++)
                for (int y = yl; y <= yr; y++)
                {
                    if (x >= Width || y >= Height || x < 0 || y < 0)
                        continue;

                    if (!gridPhysicsMap.ContainsKey(this[x, y].Type))
                        continue;

                    var p = gridPhysicsMap[this[x, y].Type];

                    if (p != null)
                    {
                        if (p.CheckCollision(a, new Vector2(x * Constants.GRID_SIZE, y * Constants.GRID_SIZE),
                            new Vector2(Constants.GRID_SIZE, Constants.GRID_SIZE)))
                        {
                            p.HandleCollission(a);
                        }

                    }
                }

            // Once collission is done for this actor, call the same function for all the children it has.
            a.GetChildActors().ForEach(ca =>
            {
                // If the child actor is collidable, 
                if (ca != null && ca.IsCollidable)
                {
                    // Check collission for it's children recursively.
                    CheckcollissionWithBlocks(ca);
                }
            });
        }

        private void CheckcollissionWithBlocks()
        {
            // Loop for every actor
            foreach (var a in actors)
            {
                // If it is not collidable, then move on..
                if (!a.IsCollidable)
                    continue;

                if (gridPhysicsMap != null)
                {
                    // Check collission for that actor
                    CheckcollissionWithBlocks(a);                    
                }
            }
        }

        private void checkCollissionsWithOtherActors(GameActor a)
        {
            if (a.PhysicsComponent != null)

                // Get all colidable objects
                actors.Where(c => c!= a && c.IsCollidable).ToList().ForEach(b => 
                {
                    // If they have the same root actors, then don't colide.
                    bool checkCollision = false;

                    // If either of them don't have a root actor, then check for collision
                    if(a.RootActor == null || b.RootActor == null)
                    {
                        checkCollision = true;
                    } 
                    // If they both have root actors and these root actors are different, then check for collision
                    else if (a.RootActor != b.RootActor)
                    {
                        checkCollision = true;
                    }

                    // If we can check collision, then check it!
                    if(checkCollision)
                    {
                        if (a.PhysicsComponent.CheckCollission(b))
                        {
                            a.onHit(b);
                            b.onHit(a);
                        }
                    }

                });

                

            // Once done colliding it, check with other actors as well.
            a.GetChildActors().ForEach(ca => 
            {
                if (ca != null && ca.IsCollidable)
                    checkCollissionsWithOtherActors(ca);
            });
        }

        private void checkCollissionsWithOtherActors()
        {
            actors.Where(ac => ac.IsCollidable).ToList().ForEach(a => checkCollissionsWithOtherActors(a));
        }

        private void tickActors()
        {
            foreach (var a in actors)
            {
                a.Tick();
            }
        }

        private void applyLevelPhysics()
        {
            if (LevelPhysics != null)
            {
                actors.Where(ac => ac.IsCollidable).ToList().ForEach(a =>
                {
                    if (this.CanActorMoveTo(a, a.Position + a.Velocity + LevelPhysics.GetGravityVector()))
                    {
                        if(a.IsAffectedByGravity)
                            LevelPhysics.AddGravity(a);
                        //a.InAir = true;
                    }
                    else
                    {
                        a.InAir = false;
                        //a.Velocity = Vector2.Zero;
                    }
                });
            }
        }

        public virtual void Tick()
        {
            // Reposition the camera if the bound actor has moved out of frame
            repositionView();

            // Apply Level Physics, if something is configured.
            applyLevelPhysics();

            // Tick every actor
            tickActors();

            // Check collissions for every actor with the surrounding blocks in the levels that it is colliding with
            CheckcollissionWithBlocks();

            // Check for collissions between each actor
            checkCollissionsWithOtherActors();

            // Delete actors that are not active as of now..
            removeDestroyedActors();

            // Update the game background
            updateBackground();


        }

        private void updateBackground()
        {
            if (GameBackground != null)
            {
                GameBackground.Tick();
                GameBackground.WindowWidth = Width;
                GameBackground.WindowHeight = Height;
            }
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
