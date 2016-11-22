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
    public class Level : GameLevel
    {
        public Level(string levelFileName, string tileSetName) : base (levelFileName, tileSetName)
        {
            // Nothing as of now..
        } 
        
        public override void Render()
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


    
}
