using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SeriousGame.Gameplay;
using SeriousGame.Utils;

[CustomEditor(typeof(Character))]
public class CharacterInspector : Editor
{
    private void OnSceneGUI()
    {
        Character character = target as Character;
        Transform handleTransform = character.transform;
        Vector3 turretConnectionPoint = character.TurretConnectionPoint;

        Handles.color = Color.red;
        EditorHelper.DrawCapQuick(turretConnectionPoint, handleTransform);
        Handles.DrawLine(turretConnectionPoint, turretConnectionPoint - handleTransform.up * 0.1f);

    }
}
