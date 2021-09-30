using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Gameplay
{
    public interface IVincible
    {
        bool isAlive { get; }
        float MaxHealth { get; set; }
        float Health { get; set; }
        float HealthPercentage { get; set; }
        void AddHealth(float amount);
        void Heal();
        void Die();

    }
}

