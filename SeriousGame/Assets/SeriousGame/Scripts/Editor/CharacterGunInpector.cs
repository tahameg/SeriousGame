using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SeriousGame.Gameplay;
using SeriousGame.Utils;

[CustomEditor(typeof(CharacterGun))]
public class CharacterGunInspector : GunBaseInspector
{
    public override void OnSceneGUI()
    {
        base.OnSceneGUI();
    }
}
