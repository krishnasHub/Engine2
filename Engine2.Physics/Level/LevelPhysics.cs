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
    /// <summary>
    /// Level Physics is concerned with physics concenring the entire level.
    /// As of now, it has logic to add gravity to the entite level, all actors in it.
    /// </summary>
    public class LevelPhysics : ILevelPhysics
    {
        private Vector2 gravity = new Vector2(0f, 0.1f);

        public void AddGravity(List<GameActor> actors)
        {
            foreach (var actor in actors)
            {
                if(actor.IsAffectedByGravity)
                    AddGravity(actor);
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
