using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SeriousGame.Utils;

namespace SeriousGame.Gameplay
{
    public struct ClampLimits
    {
        float minAngle;
        float maxAngle;
    }
    public class TurretBase : MonoBehaviour
    {
        [Header("Structural")]
        public Vector3 RootConnectionPoint;
        public Vector3 GunConnectionPoint;
        public GunBase Gun;
        public ClampLimits Limits;


        [Header("Temporary")]

        public float AzimuthSpeed;
        public float ElevationSpeed;

        private void Start()
        {
            AttachGun();
        }
        public void AttachGun()
        {
            Vector3 gunFireAxis = Gun.transform.TransformDirection(Gun.FireAxis);
            Gun.transform.rotation *= Quaternion.FromToRotation(gunFireAxis, transform.forward);
                
            Vector3 connectionPointOnGun = Gun.transform.TransformPoint(Gun.ConnectionPoint);
            Vector3 connectionPointOnTurret = transform.TransformPoint(GunConnectionPoint);
            Gun.transform.Translate(connectionPointOnTurret - connectionPointOnGun, Space.World);
        }

        public void RotateInElevation(float angle)
        {
            Vector3 connectionPointOnGun = Gun.transform.TransformPoint(Gun.ConnectionPoint);
            Geometry.RotateAround(Gun.transform, connectionPointOnGun, -transform.right, angle);
        }
        public void RotateInAzimuth(float angle)
        {
            Vector3 connectionPoint = transform.TransformDirection(RootConnectionPoint);
            Geometry.RotateAround(transform, connectionPoint, -transform.up, angle);
        }
        void rotateToward(Vector3 targetPosition, float angleStep)
        {

        }

        void rotateToward(Transform targetTransform, float angleStep)
        {

        }

    }
}

