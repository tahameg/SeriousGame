using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeriousGame.Common;
using SeriousGame.Gameplay;

namespace SeriousGame.Management
{
    public class CharacterBuilder : Service
    {
        private Dictionary<string, GameObject> _loadedAssets;
        [SerializeField]
        public List<CharacterTypeInfo> AvailableCharacterTypes;
        [SerializeField]
        public List<GunTypeInfo> AvailableGunTypes;
        [Header("Character Creation")]
        public string CharacterName;
        public CharacterTypeIndex Character;
        public GunTypeIndex Gun;
        public Vector3 SpawnPosition;
        void Start()
        {
            ServiceLocator.Instance.RegisterService(this);
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.G))
            {
                BuildCharacter(Character, Gun, SpawnPosition);
            }
        }
        public GameObject BuildCharacter(CharacterTypeIndex characterIndex, GunTypeIndex gunIndex, Vector3 spawnPosition)
        {
            CharacterTypeInfo characterInfo = GetCharacterInfo(characterIndex);
            if (characterInfo == null)
            {
                Debug.LogError("Invalid character type selected!");
            }
            GunTypeInfo gunInfo = GetGunInfo(gunIndex);
            if(gunInfo == null)
            {
                Debug.LogError("Invalid gun type selected!");
            }
            GameObject characterBaseInstance = RunAssetLoadingProcedure(characterInfo.CharacterBasePrefabPath);
            GameObject turretInstance = RunAssetLoadingProcedure(characterInfo.TurretPrefabPath);
            GameObject gunInstance = RunAssetLoadingProcedure(gunInfo.GunPrefabPath);

            if(characterBaseInstance == null || turretInstance == null || gunInstance == null)
            {
                Debug.LogError("One of the assets could not be loaded!");
                return null;
            }

            GameObject characterObject = Instantiate(characterBaseInstance, spawnPosition, Quaternion.identity);
            GameObject turretObject = Instantiate(turretInstance, spawnPosition + Vector3.one, Quaternion.identity);
            GameObject gunObject = Instantiate(gunInstance, spawnPosition + Vector3.one, Quaternion.identity);

            Character character = InitializeCharacter(characterObject, characterInfo, CharacterName);
            GunBase gun = InitializeGun(gunObject, gunInfo);
            TurretBase turret = InitializeTurretOnObject(turretObject, characterObject, characterInfo);

            turret.AttachGun(gun);
            character.AttachTurret(turret);
            return character.gameObject;
        }

        public Character InitializeCharacter(GameObject characterObject, CharacterTypeInfo info, string name)
        {
            Character instance = characterObject.GetComponent<Character>();
            if(instance == null)
            {
                instance = characterObject.AddComponent<Character>();
            }
            ActorMeta actorInfo = new ActorMeta(name);
            instance.Initialize(info.MaxHealth, actorInfo, info.TurretConnectionPointOnBase);
            return instance;
        }
        public GunBase InitializeGun(GameObject gunObject, GunTypeInfo info)
        {
            GunBase instance = gunObject.GetComponent<GunBase>();
            if(instance == null)
            {
                instance = gunObject.AddComponent<GunBase>();
            }
            instance.Initialize(info.RootConnectionPointOnGun, info.GunFirePoint, info.GunFireAxis, info.ShootingRange, info.GunChargeCapacity);
            return instance;
        }

        private TurretBase InitializeTurretOnObject(GameObject turretObject, GameObject characterBaseObject, CharacterTypeInfo info)
        {
            TurretBase instance = turretObject.GetComponent<TurretBase>();
            if(instance == null)
            {
                instance = turretObject.AddComponent<TurretBase>();
            }
            AxisLimits limits = new AxisLimits(info.ElevationMin, info.ElevationMax);
            instance.Initialize(characterBaseObject.transform, limits, info.RootConnectionPointOnTurret, info.GunConnectionPointOnTurret);
            return instance;
        }

        private GameObject getAssetIfLoaded(string path)
        {
            if(_loadedAssets == null)
            {
                _loadedAssets = new Dictionary<string, GameObject>();
                return null;
            }
            if(_loadedAssets.ContainsKey(path))
            {
                return _loadedAssets[path];
            }
            return null;
        }
        private GameObject LoadAsset(string path)
        {
            GameObject returnObject = Resources.Load<GameObject>(path);
            if(returnObject != null && !_loadedAssets.ContainsKey(path))
            {
                _loadedAssets.Add(path, returnObject);
            }
            return returnObject;
        }

        public GameObject RunAssetLoadingProcedure(string path)
        {
            GameObject returnObject = getAssetIfLoaded(path);
            if(returnObject != null)
            {
                return returnObject;
            }
            returnObject = LoadAsset(path);
            return returnObject;
        }

        public CharacterTypeInfo GetCharacterInfo(CharacterTypeIndex index)
        {
            if(index == CharacterTypeIndex.Invalid)
            {
                return null;
            }
            foreach(CharacterTypeInfo info in AvailableCharacterTypes)
            {
                if(info.Index == index)
                {
                    return info;
                }
            }
            return null;
        }

        public CharacterTypeInfo GetCharacterInfo(string name)
        {
            foreach (CharacterTypeInfo info in AvailableCharacterTypes)
            {
                if (info.TypeName == name)
                {
                    return info;
                }
            }
            return null;
        }

        public GunTypeInfo GetGunInfo(GunTypeIndex index)
        {
            if(index == GunTypeIndex.Invalid)
            {
                return null;
            }
            foreach (GunTypeInfo info in AvailableGunTypes)
            {
                if (info.Index == index)
                {
                    return info;
                }
            }
            return null;
        }

        public GunTypeInfo GetGunInfo(string name)
        {
            foreach (GunTypeInfo info in AvailableGunTypes)
            {
                if (info.TypeName == name)
                {
                    return info;
                }
            }
            return null;
        }
    }
}

