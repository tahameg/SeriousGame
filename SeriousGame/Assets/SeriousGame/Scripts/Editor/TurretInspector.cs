using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SeriousGame.Gameplay;
using SeriousGame.Utils;

[CustomEditor(typeof(TurretBase))]
public class TurretInspector : Editor
{
    private void OnSceneGUI()
    {
        TurretBase turret = target as TurretBase;
        Transform handleTransform = turret.transform;
        Vector3 gunConnectionPoint = handleTransform.TransformPoint(turret.GunConnectionPoint);
        Vector3 connectionPoint = handleTransform.TransformPoint(turret.RootConnectionPoint);

        Handles.color = Color.cyan;
        EditorHelper.DrawCapQuick(gunConnectionPoint, handleTransform);
        Handles.DrawLine(gunConnectionPoint, gunConnectionPoint - handleTransform.right * 0.1f);

        Handles.color = Color.red;
        EditorHelper.DrawCubeQuick(connectionPoint);
        Handles.DrawLine(connectionPoint, connectionPoint + handleTransform.up * 0.1f);
    }
}
