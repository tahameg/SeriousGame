using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SeriousGame.Gameplay;
using SeriousGame.Utils;

[CustomEditor(typeof(GunBase))]
public class GunBaseInspector : Editor
{
    public virtual void OnSceneGUI()
    {
        GunBase gun = target as GunBase;
        Transform handleTransform = gun.transform;
        Vector3 ConnectionPoint = handleTransform.TransformPoint(gun.ConnectionPoint);
        Vector3 FireAxis = handleTransform.TransformDirection(gun.FireAxis);
        Vector3 FirePoint = handleTransform.TransformPoint(gun.FirePoint);

        Handles.color = Color.red;
        EditorHelper.DrawCapQuick(FirePoint, handleTransform);
        EditorHelper.DrawArrowQuick(FirePoint, FireAxis, handleTransform, FireAxis.magnitude);

        Handles.color = Color.cyan;
        EditorHelper.DrawCubeQuick(ConnectionPoint);
        
    }
}
