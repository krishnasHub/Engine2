using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Actor
{
    public interface IBlockPhysics
    {

        bool CheckCollision(GameActor actor, Vector2 blockPosition, Vector2 blockSize);

        void HandleCollission(GameActor actor);

        bool CanStepIntoMe();
    }
}
