using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GunBase))]
public class GunBaseInspector : Editor
{
    private void OnSceneGUI()
    {
        GunBase gun = target as GunBase;
        Transform handleTransform = gun.transform;
        Vector3 ConnectionPoint = handleTransform.TransformPoint(gun.ConnectionPoint);
        Vector3 FireAxis = handleTransform.TransformDirection(gun.FireAxis);
        Vector3 FirePoint = handleTransform.TransformPoint(gun.FirePoint);

        Handles.color = Color.red;
        Handles.DrawWireCube(FirePoint, Vector3.one * 0.1f);
        Handles.color = Color.white;
        Handles.DrawLine(FirePoint, FirePoint + FireAxis);
        Handles.color = Color.green;
        Handles.DrawWireCube(ConnectionPoint, Vector3.one * 0.05f);

    }
}
