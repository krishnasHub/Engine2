using Engine2.Core;
using Engine2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Actor
{
    public class PawnSensing
    {
        private GameActor parentActor;

        public float SensingRadius;
        public Action<GameActor> PawnSensed;

        public PawnSensing(GameActor parentActor)
        {
            this.parentActor = parentActor;
            SensingRadius = 100.0f;
        }

        public void CheckPawn()
        {
            // If nothing is configured, don't do anything..
            if (PawnSensed == null)
                return;

            // Collect all the actors that are not part of this actor's chain.
            var actors = GameLevel.GetAllActors().Where(a => a != parentActor
                && (a.RootActor == null || parentActor.RootActor == null || a.RootActor != parentActor.RootActor)).ToList();

            // If there aren't any.. return.
            if (actors.Count == 0)
                return;

            // For every remaining actors, check their distance from the parent actor.
            foreach(var a in actors)
            {
                if(SimpleMath.Dist(a.Position, parentActor.Position) <= SensingRadius)
                {
                    PawnSensed.Invoke(a);
                }
            }

        }

    }
}
