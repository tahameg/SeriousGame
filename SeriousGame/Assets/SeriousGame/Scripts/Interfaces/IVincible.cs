using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Gameplay
{
    public interface IVincible
    {
        public delegate void DeathHandler();
        public delegate void HealthChangeHandler(float healthBeforeChange, float healthAfterChange);

        event DeathHandler Died;
        event HealthChangeHandler HealthChanged;

        bool isAlive { get; }
        float MaxHealth { get; set; }
        float Health { get; set; }
        float HealthPercentage { get; set; }
        void AddHealth(float amount);
        void Heal();
        void Die();

        void GetHarmed(IVincible Harmer, float HarmAmount);

    }
}

