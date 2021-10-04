using System;
using System.Collections;
using System.Collections.Generic;
using SeriousGame.Common;
using SeriousGame.Management;
using SeriousGame.Gameplay;
using UnityEngine;

namespace SeriousGame.Management
{
    public class GameDirector : Service
    {
        [Header("Character Creation")]
        public string CharacterName;
        public CharacterTypeIndex CharacterIndex;
        public GunTypeIndex GunIndex;
        public Vector3 SpawnPosition;
        [Header("CameraParameters")]
        public Camera MainCamera;
        public Vector3 CameraOffset;
        public float CameraSensitivity;

        ServiceLocator _serviceLocator;
        CharacterBuilder _builder;
        HudManager _hudManager;

        Character _character;

        public bool areServicesInitialized
        {
            get
            {
                return (_builder != null) && (_hudManager != null);
            }
        }
        public Character Character
        {
            get
            {
                if(_character == null)
                {
                    Character[] characters = FindObjectsOfType<Character>();
                    if (characters.Length > 1)
                    {
                        _character = characters[0];
                        for (int i = 0; i < characters.Length; i++)
                        {
                            Destroy(characters[i].gameObject);
                        }
                    }
                    else if (characters.Length == 0)
                    {
                        _character = null;
                    }
                    else
                    {
                        _character = characters[0];
                    }
                }
                return _character;
            }
            private set
            {
                _character = value;
            }
        }

        void Start()
        {
            _serviceLocator = ServiceLocator.Instance;
            _serviceLocator.RegisterService(this);
            _builder = _serviceLocator.GetService<CharacterBuilder>();
            _hudManager = _serviceLocator.GetService<HudManager>();

            if (!areServicesInitialized)
            {
                _serviceLocator.ServiceRegistered += OnServiceRegistered;
            }
            else
            {
                Initialize();
            }
        }

        private void OnServiceRegistered(Type type)
        {
            if(type == typeof(CharacterBuilder))
            {
                _builder = _serviceLocator.GetService<CharacterBuilder>();
            }
            else if(type == typeof(HudManager))
            {
                _hudManager = _serviceLocator.GetService<HudManager>();
            }
            if (areServicesInitialized)
            {
                Initialize();
            }
        }

        private void Initialize()
        {
            if(Character == null) 
            {
                GameObject characterObject = _builder.BuildCharacter(CharacterIndex, GunIndex, SpawnPosition, MainCamera, CameraSensitivity, CameraOffset, CharacterName);
                Character = characterObject.GetComponent<Character>();
            }
            CharacterGun gun = Character.Turret.Gun;
            _hudManager.Initialize(gun.MaxChargeCapacity, gun.MaxTemperature, gun.MinTemperature, Character.MaxHealth);
            Character.Unfreeze();
        }
        private void Update()
        {
            if (_hudManager.isInitialized)
            {
                _hudManager.ShowTemperature(Character.Turret.Gun.Temperature);
                _hudManager.ShowHealth(Character.Health);
                _hudManager.ShowCharge(Character.Turret.Gun.ChargeCapacity);
            }
        }
    }
}

