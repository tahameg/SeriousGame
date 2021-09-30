using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SeriousGame.Gameplay;

[CustomEditor(typeof(TurretBase))]
public class TurretBaseInspector : Editor
{
    private void OnSceneGUI()
    {
        TurretBase turret = target as TurretBase;
        Transform handleTransform = turret.transform;
        Vector3 gunConnectionPoint = handleTransform.TransformPoint(turret.GunConnectionPoint);

        Handles.color = Color.red;
        Handles.DrawWireCube(gunConnectionPoint, Vector3.one * 0.05f);
    }
}
