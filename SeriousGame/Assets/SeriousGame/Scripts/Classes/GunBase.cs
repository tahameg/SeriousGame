using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SeriousGame.Common;

namespace SeriousGame.Gameplay
{
    public abstract class GunBase : MonoBehaviour, IHeatable, IFireable
    {
        // Start is called before the first frame update

        [Header("Structural")]
        public Vector3 ConnectionPoint;
        public Vector3 FirePoint;
        public Vector3 FireAxis;
        [Header("Operational")]
        public float MaxTemperature;
        public float MinTemperature;
        public float CurrentTemperature;
        public bool isOverHeated;
        public float ShootingRange;

        public delegate void OverheatedHandler();
        public delegate void CooledDownHandler();

        public event OverheatedHandler OverHeated;
        public event CooledDownHandler CooledDown;


        public virtual void Start()
        {
            OverHeated += OnOverHeated;
            CooledDown += OnCooledDown;
        }

        private void OnDestroy()
        {
            OverHeated -= OnOverHeated;
            CooledDown -= OnCooledDown;
        }
        public void CoolDown(float amount)
        {
            if(amount >= 0f)
            {
                if(CurrentTemperature - amount < MinTemperature)
                {
                    CurrentTemperature = MinTemperature;
                    CooledDown?.Invoke();
                    isOverHeated = false;
                }
                else
                {
                    CurrentTemperature -= amount;
                }
            }
        }

        public void HeatUp(float amount)
        {
            if (amount >= 0f)
            {
                if (CurrentTemperature + amount > MinTemperature)
                {
                    CurrentTemperature = MinTemperature;
                    OverHeated?.Invoke();
                    isOverHeated = false;
                }
                else
                {
                    CurrentTemperature -= amount;
                }
            }
        }

        public bool Shoot(float power, out IVincible vincible)
        {
            if (!isOverHeated)
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.TransformPoint(FirePoint), transform.TransformDirection(FireAxis));
                if (Physics.Raycast(ray, out hit, ShootingRange))
                {
                    IVincible returnedVincible = hit.rigidbody.transform.GetComponent<IVincible>();
                    if ( returnedVincible != null)
                    {
                        vincible = returnedVincible;
                        return true;
                    }
                }
            }
            vincible = null;
            return false;
        }

        public abstract void OnCooledDown();
        public abstract void OnOverHeated();
    }

    public class ShootResult
    {
        public IVincible target;
        public bool didHit;
        public float hitRange;
        public bool isKillShot;
    }
}

