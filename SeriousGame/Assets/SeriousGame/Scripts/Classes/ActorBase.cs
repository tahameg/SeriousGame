using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Gameplay
{
    public abstract class ActorBase : MonoBehaviour, IVincible
    {
        public ActorMeta ActorInfo;
        private float _health;
        private float _previousHealth;
        private float _maxHealth;
        public float Health
        {
            get
            {
                return _health;
            }
            set
            {
                _previousHealth = _health;
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
                if (MaxHealth == 0f)
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

        public event IVincible.DeathHandler Died;
        public event IVincible.HealthChangeHandler HealthChanged;

        
        public virtual void AddHealth(float amount)
        {
            Health += amount;
        }

        public virtual void Die()
        {
            Health = 0f;
        }

        public void GetHarmed(IVincible Harmer, float HarmAmount)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Heal()
        {
            Health = MaxHealth;
        }

        protected virtual void Initialize(float maxHealth, ActorMeta actorInfo)
        {
            _maxHealth = maxHealth;
            _health = maxHealth;
            ActorInfo = actorInfo;
        }

    }

    public class ActorMeta
    {
        private string _name;
        private float _birthTime;
        private bool _isCharacter;

        public float DeathTime;
        public ActorMeta(string name)
        {
            _name = name;
        }

        public ActorMeta(string name, float birthTime, bool isCharacter=false)
        {
            _name = name;
            _birthTime = birthTime;
            _isCharacter = isCharacter;
        }
        public string Name
        { 
            get
            {
                return _name;
            }
        }

        public float BirthTime
        {
            get
            {
                return _birthTime;
            }
        }

        public bool isCharacter
        {
            get
            {
                return _isCharacter;
            }
        }
    }
}

