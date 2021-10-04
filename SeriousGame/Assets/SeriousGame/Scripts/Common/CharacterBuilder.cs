using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeriousGame.Common;
using SeriousGame.Gameplay;

namespace SeriousGame.Management
{
    public class CharacterBuilder : Service
    {
        [SerializeField]
        public List<CharacterTypeInfo> AvailableCharacterTypes;

        [SerializeField]
        public List<GunTypeInfo> AvailableGunTypes;

        private Dictionary<string, GameObject> _loadedAssets;
        public GameObject BuildCharacter(CharacterTypeIndex characterIndex, GunTypeIndex gunIndex, Vector3 spawnPosition, Camera gameCamera, float cameraSensitivity, Vector3 cameraInitialOffset, string name)
        {
            CharacterTypeInfo characterInfo = GetCharacterInfo(characterIndex);
            if (characterInfo == null)
            {
                Debug.LogError("Invalid character type selected!");
            }
            GunTypeInfo gunInfo = GetGunInfo(gunIndex);
            if (gunInfo == null)
            {
                Debug.LogError("Invalid gun type selected!");
            }
            GameObject characterBaseInstance = RunAssetLoadingProcedure(characterInfo.CharacterBasePrefabPath);
            GameObject turretInstance = RunAssetLoadingProcedure(characterInfo.TurretPrefabPath);
            GameObject gunInstance = RunAssetLoadingProcedure(gunInfo.GunPrefabPath);

            if (characterBaseInstance == null || turretInstance == null || gunInstance == null)
            {
                Debug.LogError("One of the assets could not be loaded!");
                return null;
            }

            GameObject characterObject = Instantiate(characterBaseInstance, spawnPosition, Quaternion.identity);
            GameObject turretObject = Instantiate(turretInstance, spawnPosition + Vector3.one, Quaternion.identity);
            GameObject gunObject = Instantiate(gunInstance, spawnPosition + Vector3.one, Quaternion.identity);
            CharacterGun gun = InitializeGun(gunObject, gunInfo);
            CharacterTurret turret = InitializeTurretOnObject(turretObject, characterObject, characterInfo);
            Character character = InitializeCharacter(characterObject, characterInfo, name, gameCamera);

            turret.AttachGun(gun);
            character.AttachTurret(turret);
            character.ResetCamera(cameraSensitivity, cameraInitialOffset);
            character.gameObject.layer = Character.Layer;
            return character.gameObject;
        }

        public CharacterTypeInfo GetCharacterInfo(CharacterTypeIndex index)
        {
            if (index == CharacterTypeIndex.Invalid)
            {
                return null;
            }
            foreach (CharacterTypeInfo info in AvailableCharacterTypes)
            {
                if (info.Index == index)
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
            if (index == GunTypeIndex.Invalid)
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

        public Character InitializeCharacter(GameObject characterObject, CharacterTypeInfo info, string name, Camera camera)
        {
            characterObject.name = name;
            Character instance = characterObject.GetComponent<Character>();
            if (instance == null)
            {
                instance = characterObject.AddComponent<Character>();
            }
            Gameplay.CharacterController controller = characterObject.GetComponent<Gameplay.CharacterController>();
            if (controller == null)
            {
                characterObject.AddComponent<Gameplay.CharacterController>();
            }
            GameCamera gameCamera = camera.transform.GetComponent<GameCamera>();
            if (gameCamera == null)
            {
                gameCamera = camera.gameObject.AddComponent<GameCamera>();
            }
            ActorMeta actorInfo = new ActorMeta(name);
            instance.Initialize(info.MaxHealth, actorInfo, info.TurretConnectionPointOnBase, gameCamera);
            return instance;
        }

        public CharacterGun InitializeGun(GameObject gunObject, GunTypeInfo info)
        {
            CharacterGun instance = gunObject.GetComponent<CharacterGun>();
            if (instance == null)
            {
                instance = gunObject.AddComponent<CharacterGun>();
            }
            instance.Initialize(info.RootConnectionPointOnGun, info.GunFirePoint, info.GunFireAxis, info.ShootingRange);
            return instance;
        }

        public GameObject RunAssetLoadingProcedure(string path)
        {
            GameObject returnObject = getAssetIfLoaded(path);
            if (returnObject != null)
            {
                return returnObject;
            }
            returnObject = LoadAsset(path);
            return returnObject;
        }

        private GameObject getAssetIfLoaded(string path)
        {
            if (_loadedAssets == null)
            {
                _loadedAssets = new Dictionary<string, GameObject>();
                return null;
            }
            if (_loadedAssets.ContainsKey(path))
            {
                return _loadedAssets[path];
            }
            return null;
        }

        private CharacterTurret InitializeTurretOnObject(GameObject turretObject, GameObject characterBaseObject, CharacterTypeInfo info)
        {
            CharacterTurret instance = turretObject.GetComponent<CharacterTurret>();
            if (instance == null)
            {
                instance = turretObject.AddComponent<CharacterTurret>();
            }
            AxisLimits limits = new AxisLimits(info.ElevationMin, info.ElevationMax);
            instance.Initialize(characterBaseObject.transform, limits, info.RootConnectionPointOnTurret, info.GunConnectionPointOnTurret);
            return instance;
        }

        private GameObject LoadAsset(string path)
        {
            GameObject returnObject = Resources.Load<GameObject>(path);
            if (returnObject != null && !_loadedAssets.ContainsKey(path))
            {
                _loadedAssets.Add(path, returnObject);
            }
            return returnObject;
        }

        void Start()
        {
            ServiceLocator.Instance.RegisterService(this);
        }

        void Update()
        {
        }
    }
}

