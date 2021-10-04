using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Gameplay
{
    public interface IFireable
    {
        bool Shoot(float energy, out RaycastHit hitResult);

    }
}

