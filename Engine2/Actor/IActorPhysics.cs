﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Actor
{
    public interface IActorPhysics
    {
        bool CheckCollission(GameActor a, GameActor b);
    }
}