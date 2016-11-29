using Engine2.Actor;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Physics.Block
{
    public class SolidBlockPhysics : IBlockPhysics
    {


        public bool CheckCollision(GameActor actor, Vector2 blockPosition, Vector2 blockSize)
        {
            if (actor.BoundingShape == BoundingShape.BoundingBox)
            {
                if ( (actor.Position.X + actor.BoundingBox.Right >= blockPosition.X && blockPosition.X + blockSize.X >= actor.Position.X) &&
					(actor.Position.Y + actor.BoundingBox.Bottom >= blockPosition.Y && blockPosition.Y + blockSize.Y >= actor.Position.Y) )
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
            return false;
        }

        public void HandleCollission(GameActor actor)
        {
            // If it has no velocity, no point setting the position to anything.
            if (actor.Velocity.X == 0f && actor.Velocity.Y == 0f)
                return;

            // If the actor is currently colliding with the block but with it's velocity can move to a non-colliding location, then let it go.
            if (actor.ParentLevel != null && actor.ParentLevel.CanActorMoveTo(actor, actor.Position + 2 * actor.Velocity))
                return;

            // Bounce the actor, if bouncing is set
            if (actor.BounceFactor > 0f)
            {
                actor.Velocity *= actor.BounceFactor;
                actor.Position += actor.Velocity;
            }

            // Reverse the velocity and set the position to default values.
            // Set inAir to false since we've crashed onto something..
            actor.Position -= actor.Velocity;
            actor.Center -= actor.Velocity;
            actor.Velocity -= actor.Velocity;
            actor.InAir = false;
        }
    }
}
