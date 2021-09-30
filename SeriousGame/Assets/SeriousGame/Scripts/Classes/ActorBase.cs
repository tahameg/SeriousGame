using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Gameplay
{
    public abstract class ActorBase : MonoBehaviour, IVincible
    {
        private float _maxHealth;
        private float _health;

        protected ActorMeta meta;
        public float MaxHealth
        {
            get
            {
                return _maxHealth;
            }
            set
            {
                _maxHealth = value;
            }
        }

        public float Health
        {
            get
            {
                return _health;
            }
            set
            {
                if (value >= 0)
                {
                    _health = value;
                }
                else if (value >= MaxHealth)
                {
                    _health = MaxHealth;
                }
                else
                {
                    _health = 0;
                }
            }
        }

        public float HealthPercentage
        {
            get
            {
                if( MaxHealth == 0f)
                {
                    return 0f;
                }
                else
                {
                    return (Health * 100f) / MaxHealth;
                }
            }
            set
            {
                if (value >= 100)
                {
                    Health = MaxHealth;
                }
                else if (value < 0)
                {
                    Health = 0;
                }
                else
                {
                    Health = MaxHealth * value / 100;
                }
            }
        }
        public bool isAlive
        {
            get
            {
                return _health > 0;
            }
        }
        protected virtual void Start()
        {
            Initialize();
        }

        protected virtual void Update()
        {
            if (!isAlive)
            {
                Die();
            }
        }

        protected virtual void Initialize() {
        }
        public virtual void Die()
        {
            Health = 0f;
        }
        public virtual void AddHealth(float amount)
        {
            Health -= amount;
        }
        public virtual void Heal()
        {
            Health = MaxHealth;
        }
    }

    public class ActorMeta
    {
        private string _name;
        public string Name 
        { 
            get
            {
                return _name;
            }
        }

        public ActorMeta(string name)
        {
            _name = name;
        }
    }
}

