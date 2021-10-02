using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SeriousGame.Gameplay
{
    public enum CharacterTypeIndex
    {
        Invalid,
        CharacterTypeA,
        CharacterTypeB,
        CharacterTypeC
    }

    [CreateAssetMenu(fileName = "NewCharacterType", menuName = "SeriousGame/Gameplay/CharacterType")]
    public class CharacterTypeInfo : ScriptableObject
    {
        public string TypeName;
        public CharacterTypeIndex Index;
        public string CharacterBasePrefabPath;
        public string TurretPrefabPath;
        [Header("Structural Parameters")]
        /// <summary>
        /// Where the turret will be connected(Respect to the character base local coordinates)
        /// </summary>
        public Vector3 TurretConnectionPointOnBase;
        /// <summary>
        /// Where the turret will be connected from(Respect to the turret local coordinates)
        /// </summary>
        public Vector3 RootConnectionPointOnTurret;
        /// <summary>
        /// Where the Gun will connected to(Respect to the turret local coordinates)
        /// </summary>
        public Vector3 GunConnectionPointOnTurret;

        public float ElevationMax;
        public float ElevationMin;

        [Header("Dynamical Parameters")]
        public float MaxHealth;

    }
}
    

