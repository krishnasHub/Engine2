using Engine2.Actor;
using Engine2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Physics.Actor
{
    /// <summary>
    /// This class contains the collision detection code for Actors. It's kept as simple as bounding box collision between:
    /// 1. Box and Box
    /// 2. Box and Circle
    /// 3. Circle and Circle
    /// </summary>
    public class ActorPhysics : IActorPhysics
    {
        private GameActor actor;

        public void SetActor(GameActor a)
        {
            actor = a;
        }

        public bool CheckCollission(GameActor b)
        {
            if (actor.BoundingShape == BoundingShape.BoundingBox && b.BoundingShape == BoundingShape.BoundingBox)
                return checkBoxOverBox(actor, b);
            else if (actor.BoundingShape == BoundingShape.BoundingBox && b.BoundingShape == BoundingShape.BoundingCircle)
                return checkBoxOverCircle(actor, b);
            else if (actor.BoundingShape == BoundingShape.BoundingCircle && b.BoundingShape == BoundingShape.BoundingBox)
                return checkBoxOverCircle(b, actor);
            else if (actor.BoundingShape == BoundingShape.BoundingCircle && b.BoundingShape == BoundingShape.BoundingCircle)
                return checkCircleOverCircle(actor, b);

            // Default.. should never really be here!
            return false;
        }

        private bool checkBoxOverBox(GameActor a, GameActor b)
        {
            if ((a.Position.X + a.BoundingBox.Right >= b.Position.X && b.Position.X + b.BoundingBox.Right >= a.Position.X) &&
                   (a.Position.Y + a.BoundingBox.Bottom >= b.Position.Y && b.Position.Y + b.BoundingBox.Bottom >= a.Position.Y))
                return true;

            return false;
        }

        private bool checkCircleOverCircle(GameActor a, GameActor b)
        {
            return SimpleMath.Dist(a.Center, b.Center) <= (a.BoundingRadius + b.BoundingRadius);
        }

        private bool checkBoxOverCircle(GameActor a, GameActor b)
        {
            // Check the perpendicular distance between circle of bto every edge over a
            // If any of this distance is less than or equal to the b's radius, then we have collission.
            return false;
        }
    }
}
