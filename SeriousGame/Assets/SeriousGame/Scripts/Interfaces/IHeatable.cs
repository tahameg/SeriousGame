using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Gameplay
{
    public interface IHeatable
    {
        void HeatUp(float amount);
        void CoolDown(float amount);
    }
}

