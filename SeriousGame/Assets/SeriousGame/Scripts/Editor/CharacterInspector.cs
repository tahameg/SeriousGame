using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SeriousGame.Gameplay;

[CustomEditor(typeof(Character))]
public class CharacterInspector : Editor
{
    private void OnSceneGUI()
    {
        Character gun = target as Character;

    }
}
