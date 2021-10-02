using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SeriousGame.Gameplay
{
    public enum GunTypeIndex
    {
        Invalid,
        GunTypeQ,
        GunTypeW,
        GunTypeE
    }

    [CreateAssetMenu(fileName = "NewGunType", menuName = "SeriousGame/Gameplay/GunType")]
    public class GunTypeInfo : ScriptableObject
    {
        public string TypeName;
        public GunTypeIndex Index;
        public string GunPrefabPath;
        [Header("Structural Parameters")]
        /// <summary>
        /// Where the laser rays will be originated.(Respect to the gun local coordinates)
        /// </summary>
        public Vector3 GunFirePoint;
        /// <summary>
        /// Direction of laser rays .(Respect to the gun local coordinates)
        /// </summary>
        public Vector3 GunFireAxis;
        /// <summary>
        /// Where the Gun will connected from(Respect to the gun local coordinates)
        /// </summary>
        public Vector3 RootConnectionPointOnGun;
        [Header("Dynamic Parameters")]
        public float GunChargeCapacity;
        public float ShootingRange;
    }
}


