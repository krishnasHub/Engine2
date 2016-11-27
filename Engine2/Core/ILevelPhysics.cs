using Engine2.Actor;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Core
{
    public interface ILevelPhysics
    {
        void AddGravity(List<GameActor> actors);
        void AddGravity(GameActor actor);
        Vector2 GetGravityVector();
    }
}
