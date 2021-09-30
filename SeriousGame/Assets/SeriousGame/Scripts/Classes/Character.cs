using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Gameplay
{
    public enum CharacterClassIndex 
    {
        Undefined,
        Sarp,
        SarpL,
        Alkar
    }
    public class Character : ActorBase
    {
        public string CharacterName;
        private CharacterClassIndex Index;
        public TurretBase Turret;
        public LRFBase LRF;
        public Vector3 RootSocketPosition;
        protected override void Initialize()
        {
            base.Initialize();
            meta = new ActorMeta(CharacterName);
            Health = 100f;
            MaxHealth = 120f;
            //UnityEngine.Assertions.Assert.IsTrue(Index != CharacterClassIndex.Undefined, "Index is not set!");
            //UnityEngine.Assertions.Assert.IsNotNull(Turret, "There is no turret attached!");
            //UnityEngine.Assertions.Assert.IsNotNull(LRF, "There is no LRF attached!");
        }

        protected override void Update()
        {
            base.Update();
            if ( Input.GetKey(KeyCode.LeftArrow))
            {
                Turret.RotateInAzimuth(Turret.AzimuthSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                Turret.RotateInAzimuth(-Turret.AzimuthSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                Turret.RotateInElevation(Turret.ElevationSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                Turret.RotateInElevation(-Turret.ElevationSpeed * Time.deltaTime);
            }
        }

    }
}

