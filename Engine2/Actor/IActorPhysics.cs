using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Actor
{
    public interface IActorPhysics
    {
        void SetActor(GameActor a);
        bool CheckCollission(GameActor otherActor);
    }
}
