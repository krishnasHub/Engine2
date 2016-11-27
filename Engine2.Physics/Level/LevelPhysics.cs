using Engine2.Actor;
using Engine2.Core;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Physics.Level
{
    public class LevelPhysics : ILevelPhysics
    {
        private Vector2 gravity = new Vector2(0f, 0.1f);

        public void AddGravity(List<GameActor> actors)
        {
            foreach (var actor in actors)
            {
                actor.Velocity += gravity;
            }
        }

        public void AddGravity(GameActor actor)
        {
            actor.Velocity += gravity;
        }

        public Vector2 GetGravityVector()
        {
            return gravity;
        }
    }
}
