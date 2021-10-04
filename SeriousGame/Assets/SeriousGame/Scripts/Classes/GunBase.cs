using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SeriousGame.Common;

namespace SeriousGame.Gameplay
{
    public abstract class GunBase : MonoBehaviour, IFireable
    {
        // Start is called before the first frame update

        [Header("Structural")]
        public Vector3 ConnectionPoint;
        public Vector3 FirePoint;
        public Vector3 FireAxis;
        public GameObject LaserBeamObject;
        public float LaserBeamSpeed = 15f;
        public float MaxLaserLifespan = 6f;
        public GameObject LaserShotPointObject;
        [Header("Operational")]
        public float ShootingRange;

        private bool _isInitialized;

        public bool isInitialized
        {
            get
            {
                return _isInitialized;
            }
        }

        public virtual bool Shoot(float energy, out RaycastHit hitResult)
        {
            Ray ray = new Ray(transform.TransformPoint(FirePoint), transform.TransformDirection(FireAxis));
            return Physics.Raycast(ray, out hitResult, ShootingRange);
        }

        public virtual void Initialize(Vector3 rootConnectionPoint, Vector3 firePoint, Vector3 fireAxis, float shootingRange)
        {
            ConnectionPoint = rootConnectionPoint;
            FirePoint = firePoint;
            FireAxis = fireAxis;
            ShootingRange = shootingRange;
            if (LaserBeamObject == null)
            {
                LaserBeamObject = Resources.Load<GameObject>("Disposables/LaserBeam");
            }
            if (LaserShotPointObject == null)
            {
                LaserShotPointObject = Resources.Load<GameObject>("Disposables/LaserShotPoint");
            }
            _isInitialized = true;
        }

    }
}

