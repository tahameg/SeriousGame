using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SeriousGame.Utils;
using SeriousGame.Gameplay;

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
        EditorHelper.DrawCapQuick(FirePoint, handleTransform);
        
        Handles.color = Color.white;
        Handles.DrawLine(FirePoint, FirePoint + FireAxis);
        
        Handles.color = Color.green;
        EditorHelper.DrawCapQuick(ConnectionPoint, handleTransform);
        Handles.DrawWireCube(ConnectionPoint, Vector3.one * 0.05f);
        
    }
}
