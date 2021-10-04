using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Gameplay
{
    public class CharacterTurret : TurretBase
    {
        public delegate void HeatingHandler();

        public delegate void ShootingRoutineHandler();
        public delegate void InvalidShootingHandler(InvalidShootType type);
        public delegate void BatteryHandler();

        public event HeatingHandler OverHeated;
        public event HeatingHandler CooledDown;

        public event BatteryHandler BatteryOver;

        public event ShootingRoutineHandler ShootingRoutineStarted;
        public event InvalidShootingHandler InvalidShoot;

        public delegate void ShootingResultHandler(float energy, bool didItHit, RaycastHit hitResult);
        public event ShootingResultHandler ShootingFinalized;
        public new CharacterGun Gun
        {
            get
            {
                return base.Gun as CharacterGun;
            }
        }
        public void AttachGun(CharacterGun gun)
        {
            base.AttachGun(gun);
            Gun.OverHeated += OnOverHeated;
            Gun.CooledDown += OnCooledDown;
            Gun.BatteryOver += OnBatteryOver;
            Gun.ShootingRoutineStarted += OnShootingStarted;
            Gun.InvalidShoot += OnInvalidShoot;
            Gun.ShootingFinalized += OnShootingFinalized;
        }

        public new void RemoveGun()
        {
            base.RemoveGun();
        }

        public void StartShooting()
        {
            Gun.StartShooting();
        }

        public void EndShooting()
        {
            Gun.EndShooting();
        }

        private void OnCooledDown()
        {
            CooledDown?.Invoke();
        }
        private void OnOverHeated()
        {
            OverHeated?.Invoke();
        }

        private void OnInvalidShoot(InvalidShootType type)
        {
            InvalidShoot?.Invoke(type);
        }
        private void OnShootingStarted()
        {
            ShootingRoutineStarted?.Invoke();
        }

        private void OnBatteryOver()
        {
            BatteryOver?.Invoke();
        }
        private void OnShootingFinalized(float energy, bool result, RaycastHit hitResult)
        {
            ShootingFinalized?.Invoke(energy, result, hitResult);
        }
        public void OnDestroy()
        {
            if(Gun != null)
            {
                RemoveGun();
            }
        }
    }
}

