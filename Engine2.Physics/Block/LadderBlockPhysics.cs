using Engine2.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Engine2.Physics.Block
{
    public class LadderBlockPhysics : IBlockPhysics
    {
        public bool CheckCollision(GameActor actor, Vector2 blockPosition, Vector2 blockSize)
        {
            if (actor.BoundingShape == BoundingShape.BoundingBox)
            {
                float x = blockPosition.X;
                float y = blockPosition.Y;
                float xx = blockSize.X * 0.7f;
                float yy = blockSize.Y * 0.7f;


                if ((actor.Position.X + actor.BoundingBox.Right >= x && x + xx >= actor.Position.X) &&
                    (actor.Position.Y + actor.BoundingBox.Bottom >= y && y + yy >= actor.Position.Y))
                    return true;
            }

            else if (actor.BoundingShape == BoundingShape.BoundingCircle)
            {
                // Find the perpendicular distance from the center of the actor's scircle to all 4 sides
                // of the block.
                // If either of these distances are less than or equal to the circle's radius, there is a collission.
            }

            return false;
        }

        public bool CanStepIntoMe()
        {
            return true;
        }



        public void HandleCollission(GameActor actor)
        {
            actor.Position -= actor.Velocity;
            actor.Velocity = Vector2.Zero;
            actor.InAir = false;
        }
    }
}
