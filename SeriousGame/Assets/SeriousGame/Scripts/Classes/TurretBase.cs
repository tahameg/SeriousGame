using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SeriousGame.Utils;

namespace SeriousGame.Gameplay
{   
    public struct AxisLimits
    {
        public float max;
        public float min;
        public AxisLimits(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public static AxisLimits identity
        {
            get
            {
                return new AxisLimits(0.0f, 0.180f);
            }
        }

        public static AxisLimits zero
        {
            get
            {
                return new AxisLimits(0.0f, 0.0f);
            }
        }
    }
    public abstract class TurretBase : MonoBehaviour
    {
        [Header("Structural")]
        public Vector3 GunConnectionPoint;
        public Vector3 RootConnectionPoint;
        private AxisLimits _elevationLimits;
        private GunBase _gun;
        private bool _isInitialized;

        public float Azimuth
        {
            get
            {
                Vector3 worldFireAxis = Gun.transform.TransformDirection(Gun.FireAxis);
                return Geometry.GetAngleOnPlane(worldFireAxis, AzimuthZeroAxis, _azimuthPlaneNormal);
            }
            set
            {
                Vector3 connectionPoint = transform.TransformDirection(RootConnectionPoint);
                float angle = Azimuth - value;
                transform.RotateAround(connectionPoint, _azimuthPlaneNormal, angle);
            }
        }

        public Vector3 AzimuthZeroAxis
        {
            get
            {
                return transform.forward;
            }
        }

        public float Elevation
        {
            get
            { 
                Vector3 worldFireAxis = Gun.transform.TransformDirection(Gun.FireAxis);
                return Geometry.GetAngleOnPlane(worldFireAxis, ElevationZeroAxis, _elevationPlaneNormal);
            }
            set
            {
                Vector3 connectionPoint = transform.TransformPoint(GunConnectionPoint);
                float angle =  Elevation - value;
                Gun.transform.RotateAround(connectionPoint, _elevationPlaneNormal, angle);
            }
        }

        public AxisLimits ElevationLimits
        {
            get
            {
                return _elevationLimits.Equals(AxisLimits.zero) ? AxisLimits.identity : _elevationLimits;
            }
            set
            {
                if (value.min > value.max)
                {
                    _elevationLimits = new AxisLimits(value.max, value.max);
                }
                else
                {
                    _elevationLimits = value;
                }
            }
        }

        public Vector3 ElevationZeroAxis
        {
            get
            {
                return transform.forward;
            }
        }

        public GunBase Gun
        {
            get
            {
                return _gun;
            }
            private set
            {
                _gun = value;
            }
        }

        public bool isInitialized
        {
            get
            {
                bool isGunInitialized = false;
                if( Gun != null)
                {
                    isGunInitialized = Gun.isInitialized;
                }
                return _isInitialized && isGunInitialized;
            }
        }
        
        private Vector3 _azimuthPlaneNormal
        {
            get
            {
                return transform.transform.up;
            }
        }

        private Vector3 _elevationPlaneNormal
        {
            get
            {
                return  transform.right;
            }
        }
       
        public void AttachGun(GunBase gun)
        {
            if (Gun != null)
            {
                RemoveGun();
            }
            Gun = gun;
            gun.transform.parent = transform;
            PlaceGun(Gun);
            Elevation = ElevationLimits.min > 0f ? ElevationLimits.min : 0.0f;
        }

        /// <summary>
        /// Initializes the rotation axis according to the parrent transform;
        /// </summary>
        /// <param name="parent">The parrent transform that this turret is attached to.</param>
        public virtual void Initialize(Transform parent, AxisLimits axisLimits, Vector3 rootConnectionPoint, Vector3 gunConnectionPoint)
        {
            transform.parent = parent;
            transform.rotation = transform.parent.rotation;
            RootConnectionPoint = rootConnectionPoint;
            GunConnectionPoint = gunConnectionPoint;
            ElevationLimits = axisLimits;
            _isInitialized = true;
            
        }

        public void RemoveGun()
        {
            if (Gun != null)
            {
                Destroy(Gun.gameObject);
                Gun = null;
            }
        }
        
        public void RotateInAzimuth(float angle)
        {
            Azimuth += angle;
        }

        public void RotateInElevation(float angle)
        {
            Elevation = Mathf.Clamp(Elevation + angle, ElevationLimits.min, ElevationLimits.max);
        }

        /// <summary>
        /// Rotates turret to the target
        /// </summary>
        /// <param name="targetPosition"> Target position for turret to rotate to.</param>
        /// <param name="step"> An value between 0 and 1</param>
        public void RotateToward(Vector3 targetPosition, float step)
        {
            float fixedStep = step > 1f ? 1 : step < 0f ? 0 : step;
            float[] targetAzimuthElevation = GetAzimuthElevation(targetPosition);
            float[] difference = new float[] { targetAzimuthElevation[0] - Azimuth, targetAzimuthElevation[1] - Elevation };
            RotateInAzimuth(difference[0] * step);
            RotateInElevation(difference[1] * step);
        }

        /// <summary>
        /// Rotates turret to the target.
        /// </summary>
        /// <param name="targetTransform"> Target transform for turret to rotate to.</param>
        /// <param name="step"> An value between 0 and 1</param>
        
        public void RotateToward(Transform targetTransform, float step)
        {
            RotateToward(targetTransform.position, step);
        }

        private float CalculateAzimuth(Vector3 worldPosition)
        {
            Vector3 objectDirection = (worldPosition - transform.TransformPoint(RootConnectionPoint)).normalized;
            return Geometry.GetAngleOnPlane(objectDirection, AzimuthZeroAxis, _azimuthPlaneNormal);
        }

        private float CalculateElevation(Vector3 worldPosition)
        {
            Vector3 objectDirection = (worldPosition - transform.TransformPoint(GunConnectionPoint)).normalized;
            return Geometry.GetAngleOnPlane(objectDirection, ElevationZeroAxis, _elevationPlaneNormal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldPosition"> Position of the target object in world space</param>
        /// <returns> An array of 2 consisting of azimuth and Elevation respectively</returns>
        private float[] GetAzimuthElevation(Vector3 worldPosition)
        {
            return new float[] { CalculateAzimuth(worldPosition), CalculateElevation(worldPosition) };
        }

        private void PlaceGun(GunBase gun)
        {
            Vector3 gunFireAxis = gun.transform.TransformDirection(gun.FireAxis);
            gun.transform.rotation = Quaternion.FromToRotation(gunFireAxis, transform.forward)*gun.transform.rotation;
            gun.transform.rotation = Quaternion.FromToRotation(gun.transform.up, transform.up) * gun.transform.rotation;
            
            Vector3 connectionPointOnGun = gun.transform.TransformPoint(gun.ConnectionPoint);
            Vector3 connectionPointOnTurret = transform.TransformPoint(GunConnectionPoint);
            gun.transform.Translate(connectionPointOnTurret - connectionPointOnGun, Space.World);
        }

        public bool Shoot(float energy, out RaycastHit hitResult)
        {
            return Gun.Shoot(energy, out hitResult);
        }
    }
}

