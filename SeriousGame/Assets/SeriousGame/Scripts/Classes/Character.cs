using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Gameplay
{
    public enum CharacterClassIndex 
    {
        Undefined,
        Sarp,
        SarpL,
        Alkar
    }
    public class Character : ActorBase
    {
        public string CharacterName;
        public CharacterClassIndex Index;
        public Vector3 TurretConnectionPoint;
        private TurretBase _turret;
        private LRFBase _lrf;

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
        [Header("Development")]
        public GunBase TestGun;
        public TurretBase TestTurret;
        public LRFBase TestLRF;
        public GameObject TestObject;
        public float maxSpeed;

        protected override void Initialize()
        {
            meta = new ActorMeta(CharacterName);
            UnityEngine.Assertions.Assert.IsTrue(Index != CharacterClassIndex.Undefined, "Index is not set!");
        }

        public void AttachTurret(TurretBase turret)
        {
            if (Turret != null)
            {
                RemoveTurret();
            }
            Turret = turret;
            Turret.Initialize(transform);
            PlaceTurret(Turret);
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

