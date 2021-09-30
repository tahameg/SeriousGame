using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}

