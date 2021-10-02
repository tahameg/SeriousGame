using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Gameplay
{

    public class Character : ActorBase
    {
        private bool _isInitialized;
        public Vector3 TurretConnectionPoint;
        private TurretBase _turret;
        private LRFBase _lrf;
        public bool isInitialized 
        {
            get
            {
                return _isInitialized;
            }
        }
        public TurretBase Turret 
        { 
            get
            {
                return _turret;
            }
            private set 
            {
                _turret = value;
            } 
        }
        public LRFBase LRF
        {
            get
            {
                return _lrf;
            }
            private set
            {
                _lrf = value;
            }
        }

        public void Initialize(float maxHealth, ActorMeta meta, Vector3 turretConnectionPoint)
        {
            base.Initialize(maxHealth, meta);
            TurretConnectionPoint = turretConnectionPoint;
            this.name = name;
            _isInitialized = true;
        }

        public void AttachTurret(TurretBase turret)
        {
            if (Turret != null)
            {
                RemoveTurret();
            }
            Turret = turret;
            PlaceTurret(turret);
        }

        public void RemoveTurret()
        {
            if (Turret != null)
            {
                Destroy(Turret.gameObject);
                Turret = null;
            }
        }
        
        private void PlaceTurret(TurretBase turret)
        {
            Vector3 connectionPointOnTurret = turret.transform.TransformPoint(Turret.RootConnectionPoint);
            Vector3 connectionPointOnCharacter = transform.TransformPoint(TurretConnectionPoint);
            turret.transform.Translate(connectionPointOnCharacter - connectionPointOnTurret, Space.World);
        }

    }
}

