using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Gameplay
{
    public struct CameraParameters
    {
        public Vector3 OffsetFromTarget;
        public float Sensitivity;
        public Transform TargetTransform;
        public CameraParameters(Transform targetTransform, float sensitivity, Vector3 offsetFromTarget)
        {
           TargetTransform = targetTransform;
           Sensitivity = sensitivity;
           OffsetFromTarget = offsetFromTarget;
        }
    }
    public class GameCamera : MonoBehaviour
    {
        public Vector3 CurrentlyLookedPosition;
        public GameObject CrosshairObject;
        private Camera _cameraComponent;
        private bool _isInitialized;
        private Vector3 _offsetFromTarget;

        private float _sensitivity;

        private Transform _targetTransform;

        private float _YAngle;
        public bool isInitialized
        {
            get
            {
                return _isInitialized;
            }
        }
        public Vector3 CalculateTargetPosition(float range)
        {
            float transformedRange = Vector3.Distance(transform.position, _targetTransform.position) + range;
            Vector3 cameraSpace = _cameraComponent.WorldToScreenPoint(CrosshairObject.transform.position);
            Vector3 unitWorldPosition = _cameraComponent.ScreenToWorldPoint(cameraSpace);
            Vector3 direction = (unitWorldPosition - transform.position).normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, transformedRange))
            {
                CurrentlyLookedPosition = hit.point;
                return CurrentlyLookedPosition;
            }
            CurrentlyLookedPosition = transform.position + direction * transformedRange;
            return CurrentlyLookedPosition;
        }

        public void Initialize(CameraParameters cameraParameters)
        {
            _targetTransform = cameraParameters.TargetTransform;
            _sensitivity = cameraParameters.Sensitivity;
            _offsetFromTarget = cameraParameters.OffsetFromTarget;
            transform.position = _targetTransform.transform.position + _offsetFromTarget;
            transform.rotation = _targetTransform.rotation;
            transform.rotation = Quaternion.LookRotation(_targetTransform.position+_targetTransform.forward*2 - transform.position);
            _cameraComponent = transform.GetComponent<Camera>();
            _isInitialized = true;
        }

        public void RotateBy(float rotationX, float rotationY)
        {
            float rotX = rotationX * _sensitivity * Time.deltaTime;
            float rotY = -rotationY * _sensitivity * Time.deltaTime;
            transform.RotateAround(_targetTransform.position, _targetTransform.up, rotX);
            transform.RotateAround(_targetTransform.position, transform.right, rotY);
        }

        public void RotateDirection(float step)
        {
            transform.RotateAround(_targetTransform.position, Vector3.up, step);
        }

    }
}

