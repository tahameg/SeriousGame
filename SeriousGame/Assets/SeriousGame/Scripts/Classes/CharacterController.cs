using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeriousGame.Common;
using SeriousGame.Gameplay;
using SeriousGame.Management;

namespace SeriousGame.Gameplay
{
    public class CharacterController : MonoBehaviour
    {
        Character _character;
        void Start()
        {
            _character = GetComponent<Character>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_character.isInitialized && _character.isReadyForControl)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _character.StartShooting();
                }
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    _character.EndShooting();
                }

                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                _character.Camera.RotateBy(mouseX, mouseY);
                Vector3 aimPosition = _character.Camera.CalculateTargetPosition(_character.Turret.Gun.ShootingRange);
                _character.AimToward(aimPosition, 0.05f);
            }
        }

    }
}

