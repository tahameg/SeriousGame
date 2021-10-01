using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SeriousGame.Utils
{
    public static class Geometry
    {
        public static void RotateAround(Transform transform, Vector3 pivotPoint, Vector3 axis, float angle)
        {
            Quaternion rot = Quaternion.AngleAxis(angle, axis);
            transform.position = rot * (transform.position - pivotPoint) + pivotPoint;
            transform.rotation = rot * transform.rotation;
        }

        public static float GetAngleOnPlane(Vector3 from, Vector3 to, Vector3 planeNormal)
        {
            Vector3 fromProjected = Vector3.ProjectOnPlane(from, planeNormal);
            Vector3 toProjected = Vector3.ProjectOnPlane(to, planeNormal);
            return Vector3.SignedAngle(fromProjected, toProjected, planeNormal);
        }

        public static float GetAngleOnPlaneOfTransform(Vector3 from, Vector3 to, Vector3 planeNormal, Transform transform)
        {
            Vector3 fromOnTransform = transform.InverseTransformDirection(from);
            Vector3 toOnTransform = transform.InverseTransformDirection(to);
            Vector3 fromProjected = Vector3.ProjectOnPlane(fromOnTransform, planeNormal);
            Vector3 toProjected = Vector3.ProjectOnPlane(toOnTransform, planeNormal);
            return Vector3.SignedAngle(fromProjected, toProjected, planeNormal);
        }
    }

    public static class EditorHelper
    {
        public static void DrawCapQuick(Vector3 position, Transform handleTransform)
        {
            DrawCapQuick(position, handleTransform, 0.01f);
        }

        public static void DrawCapQuick(Vector3 position, Transform handleTransform, float size)
        {
            Handles.DotHandleCap(0, position, handleTransform.rotation * Quaternion.LookRotation(Vector3.up), size, EventType.Repaint);
        }
    }
}

