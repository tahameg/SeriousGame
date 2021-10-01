using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Gameplay
{
    public interface IFireable
    {
        public bool Shoot(float poweri, out IVincible vincible);

    }
}

