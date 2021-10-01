using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Gameplay
{
    public interface IHeatable
    {
        public void HeatUp(float amount);
        public void CoolDown(float amount);
    }
}

