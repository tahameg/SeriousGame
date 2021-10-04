using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Gameplay
{

    public class Character : ActorBase
    {
        public static int Layer;
        public Vector3 TurretConnectionPoint;

        [SerializeField]
        private float _cameraMinClamp;
        [SerializeField]
        private float _cameraMaxClamp;
        private GameCamera _camera;
        private bool _isInitialized;
        private bool _isReadyForControl;
        private LRFBase _lrf;
        private CharacterTurret _turret;
        public delegate void ShootStatReadyHandler(ShootResult shootResult);
        public event ShootStatReadyHandler ShootStatReady;
        
        public GameCamera Camera
        {
            get
            {
                return _camera;
            }
        }
        public bool isInitialized
        {
            get
            {
                bool isTurretInitialized = false;
                if (Turret != null)
                {
                    isTurretInitialized = Turret.isInitialized;
                }
                bool isCameraInitialized = false;
                if (Camera != null)
                {
                    isCameraInitialized = Camera.isInitialized;
                }
                return _isInitialized && isTurretInitialized && isCameraInitialized;
            }
        }

        public bool isReadyForControl
        {
            get
            {
                return _isReadyForControl;
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

        public CharacterTurret Turret
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
        public void AimToward(Vector3 point, float step)
        {
            if (isInitialized)
            {
                Turret.RotateToward(point, step);
            }
        }

        public void AttachTurret(CharacterTurret turret)
        {
            if (Turret != null)
            {
                RemoveTurret();
            }
            Turret = turret;
            PlaceTurret(turret);
            Turret.ShootingFinalized += OnShootingFinalized;
        }

        public void EndShooting()
        {
            if (isInitialized)
            {
                Turret.EndShooting();
            }
        }

        public void Freeze()
        {
            _isReadyForControl = false;
        }

        public void Initialize(float maxHealth, ActorMeta meta, Vector3 turretConnectionPoint, GameCamera camera)
        {
            base.Initialize(maxHealth, meta);
            TurretConnectionPoint = turretConnectionPoint;
            name = name;
            _camera = camera;
            _camera.transform.parent = transform;
            _isInitialized = true;
        }
        public void OnDestroy()
        {
            if (Turret != null)
            {
                Turret.ShootingFinalized -= OnShootingFinalized;
            }
        } 
        public override void OnDied()
        {
            //dostuff
        }

        private float CalculateActualEnergy(float energy, Transform target)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            float usedEnergyInRange = energy * Mathf.Log(distance, 20);
            return energy - usedEnergyInRange >= 0f ? energy - usedEnergyInRange : 0f;
        }

        public void OnShootingFinalized(float energy, bool result, RaycastHit hit)
        {
            if (result)
            {
                GameObject hitObject = hit.transform.gameObject;
                Enemy hitEnemy = hit.transform.GetComponent<Enemy>();
                if (hitEnemy != null)
                {
                    if (hitEnemy.isAlive)
                    {
                        float realEnergy = CalculateActualEnergy(energy, hit.transform);
                        bool isKillShot;
                        hitEnemy.HandleShot(realEnergy, out isKillShot);
                        ShootResult shootResult = new ShootResult(true, realEnergy, isKillShot);
                        ShootStatReady?.Invoke(shootResult);
                    }
                    else
                    {
                        ShootStatReady?.Invoke(new ShootResult());
                    }
                }
                else
                {
                    ShootStatReady?.Invoke(new ShootResult());
                }
            }
            else
            {
                ShootStatReady?.Invoke(new ShootResult());
            }
        }

        public void RemoveTurret()
        {
            if (Turret != null)
            {
                Destroy(Turret.gameObject);
                Turret = null;
            }
        }

        public void ResetCamera(float sensitivity, Vector3 initialOffset)
        {
            if (Turret != null)
            {
                CameraParameters parameters = new CameraParameters(Turret.transform, sensitivity, initialOffset);
                Camera.Initialize(parameters);
            }
        }

        public void StartShooting()
        {
            if (isInitialized)
            {
                Turret.StartShooting();
            }
        }

        public void Unfreeze()
        {
            _isReadyForControl = true;
        }
        private void PlaceTurret(CharacterTurret turret)
        {
            Vector3 connectionPointOnTurret = turret.transform.TransformPoint(Turret.RootConnectionPoint);
            Vector3 connectionPointOnCharacter = transform.TransformPoint(TurretConnectionPoint);
            turret.transform.Translate(connectionPointOnCharacter - connectionPointOnTurret, Space.World);
        }
    }

    public struct ShootResult
    {
        public bool didItHit;
        public float DemageGiven;
        public bool isDeathShoot;

        public ShootResult(bool didItHit, float demageGiven, bool isDeathShoot)
        {
            this.didItHit = didItHit;
            DemageGiven = demageGiven;
            this.isDeathShoot = isDeathShoot;
        }
    }
}

