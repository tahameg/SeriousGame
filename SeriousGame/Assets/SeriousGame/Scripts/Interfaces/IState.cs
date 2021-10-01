using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Common
{
    public interface IState
    {
        public void Enter();
        public void Tick();
        public void LateTick();
        public void FixedTick();
        public void Exit();
    }
}

