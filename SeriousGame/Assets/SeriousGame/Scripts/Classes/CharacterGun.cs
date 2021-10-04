using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Gameplay
{
    public enum InvalidShootType 
    {
        BatteryOver,
        OverHeated
    }

    public class CharacterGun : GunBase, IHeatable
    {
        private bool _isOverHeated;
        private bool _isCooling;
        private float _maxChargeCapacity;
        private float _maxEnergyLoadingDuration = 5.0f;
        private float _chargeCapacity;
        private float _maxTemperature;
        private float _minTemperature;
        private float _temperature;
        
        private bool _isShootingStarted;
        private float _timeEllapsedInShooting;

        private float _heatUpStep
        {
            get
            {
                return (MaxTemperature - MinTemperature) / _maxEnergyLoadingDuration;
            }
        }
        public void Start()
        {
         
            _maxChargeCapacity = 1000f;
            _chargeCapacity = 1000f;
            _temperature = 20f;
            _maxTemperature = 70f;
            _minTemperature = 20f;
        }
        public float MaxChargeCapacity 
        {
            get
            {
                return _maxChargeCapacity;
            }
        }
        
        public float ChargeCapacity
        {
            get
            {
                return _chargeCapacity;
            }
        }

        public float MaxTemperature
        {
            get
            {
                return _maxTemperature;
            }
        }

        public float MinTemperature
        {
            get
            {
                return _minTemperature;
            }
        }

        public float Temperature
        {
            get
            {
                return _temperature;
            }
        }

        public bool isOverHeated
        {
            get
            {
                return _isOverHeated;
            }
        }

        public bool isBatteryOver
        {
            get
            {
                return ChargeCapacity <= 0.0f;
            }
        }

        public delegate void HeatingHandler();

        public delegate void ShootingRoutineHandler();
        public delegate void InvalidShootingHandler(InvalidShootType type);
        public delegate void BatteryHandler();

        public event HeatingHandler OverHeated;
        public event HeatingHandler CooledDown;

        public event BatteryHandler BatteryOver;

        public event ShootingRoutineHandler ShootingRoutineStarted;
        public event InvalidShootingHandler InvalidShoot;

        public delegate void ShootingResultHandler(float energy, bool didItHit, RaycastHit hitResult);
        public event ShootingResultHandler ShootingFinalized;


        public override void Initialize(Vector3 rootConnectionPoint, Vector3 firePoint, Vector3 fireAxis, float shootingRange)
        {
            base.Initialize(rootConnectionPoint, firePoint, fireAxis, shootingRange);
            _maxChargeCapacity = 1000f;
            _chargeCapacity = 1000f;
            _temperature = 20f;
            _maxTemperature = 70f;
            _minTemperature = 20f;
        }
        private void LoseCharge(float amount)
        {
            if(amount >= 0f) { 
                if(_chargeCapacity - amount < 0f)
                {
                    _chargeCapacity = 0;
                    OnBatteryOver();
                }
                else
                {
                    _chargeCapacity -= amount;
                }
            }
        }

        private float GetCoolDownDuration(float temperature)
        {
            return Mathf.Pow(2, temperature * 0.1f ) / Mathf.Pow(2, temperature * 0.05f);
        }

        private float GetLoadedEnergyInDelta(float t1, float t2)
        {
            double point1 = Mathf.Pow(2, t1);
            double point2 = Mathf.Pow(2, t2);
            return (float)((point2 - point1)* 332.19281);
        }
        public void CoolDown(float amount)
        {
            if (amount >= 0f)
            {
                if (_temperature - amount < MinTemperature)
                {
                    _temperature = MinTemperature;
                    OnCooledDown();
                    _isOverHeated = false;
                }
                else
                {
                    _temperature -= amount;
                }
            }
        }

        public void HeatUp(float amount)
        {
            if (amount >= 0f)
            {
                if (_temperature + amount > MaxTemperature)
                {
                    _temperature = MaxTemperature;
                    OnOverHeated();
                    _isOverHeated = true;
                }
                else
                {
                    _temperature += amount;
                }
            }
        }

        private void OnCooledDown()
        {

        }
        private void OnOverHeated()
        {

        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartShooting();
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                EndShooting();
            }
            if (_isOverHeated)
            {
                Debug.Log("Overheated");
            }
        }

        public void StartShooting()
        {
            if(_isShootingStarted == false)
            {
                _isShootingStarted = true;
                StartCoroutine("ShootingRoutine");
            }
        }

        public void EndShooting()
        {
            _isShootingStarted = false;
        }
        private void OnInvalidShoot(InvalidShootType type)
        {

        }
        private void OnShootingStarted()
        {
            ShootingRoutineStarted?.Invoke();
        }

        private void OnBatteryOver()
        {
            BatteryOver?.Invoke();
        }
        private void OnShootingEnded(float loadedEnergy)
        {
            StartCoroutine("ShootLaser", loadedEnergy);
            if(!_isShootingStarted && Temperature > MinTemperature && !_isCooling)
            {
                StartCoroutine("CoolingRoutine", GetCoolDownDuration(Temperature));
            }
        }

        IEnumerator ShootLaser(float energy)
        {
            RaycastHit hit;
            bool shootResult = Shoot(energy, out hit);
            Vector3 creationPoint = transform.TransformPoint(FirePoint);
            Vector3 fireAxisNormalized = transform.TransformDirection(FireAxis).normalized;
            Quaternion creationRotation = Quaternion.FromToRotation(Vector3.up, fireAxisNormalized);
            GameObject instantiatedLaserBeam = Instantiate(LaserBeamObject, creationPoint, creationRotation);
            instantiatedLaserBeam.transform.localScale = Vector3.one * Mathf.Sqrt(energy) * 0.01f;
            float timeEllapsed = 0.0f;
            float timeToTravel;
            if (shootResult)
            {
                timeToTravel = Vector3.Distance(transform.TransformPoint(FirePoint), hit.point) / LaserBeamSpeed;

            }
            else 
            {
                timeToTravel = MaxLaserLifespan;
            }
            while (timeEllapsed < timeToTravel)
            {
                instantiatedLaserBeam.transform.position += fireAxisNormalized * LaserBeamSpeed * Time.deltaTime;
                timeEllapsed += Time.deltaTime;
                yield return null;
            }
            timeEllapsed = 0.0f;
            if (shootResult)
            {
                GameObject instantiatedLaserShot = Instantiate(LaserShotPointObject, hit.point, Quaternion.identity);
                while (timeEllapsed <= 0.1f)
                {
                    timeEllapsed += Time.deltaTime;
                    instantiatedLaserShot.transform.localScale += Vector3.one * energy * Time.deltaTime * 0.01f;
                    yield return null;
                }
                Destroy(instantiatedLaserShot);
            }
            Destroy(instantiatedLaserBeam);
            ShootingFinalized?.Invoke(energy, shootResult, hit);
        }

        IEnumerator ShootingRoutine()
        {
            if (!_isShootingStarted)
            {
                yield break;
            }
            if(isOverHeated)
            {
                _isShootingStarted = false;
                OnInvalidShoot(InvalidShootType.OverHeated);
            }
            else if(isBatteryOver)
            {
                _isShootingStarted = false;
                OnInvalidShoot(InvalidShootType.BatteryOver);
            }
            else
            {
                float timeEllapsed = 0.0f;
                float previousTimeEllapsed = 0.0f;
                float deltaTime = 0.0f;
                float loadedEnergy = 0.0f;
                OnShootingStarted();
                while (_isShootingStarted)
                {
                    deltaTime = Time.deltaTime;
                    previousTimeEllapsed = timeEllapsed;
                    if (timeEllapsed + deltaTime >= _maxEnergyLoadingDuration)
                    {
                        timeEllapsed = _maxEnergyLoadingDuration;
                    }
                    else
                    {
                        timeEllapsed += deltaTime;
                    }
                    HeatUp(_heatUpStep*deltaTime);
                    float energyLoadedInDelta = GetLoadedEnergyInDelta(previousTimeEllapsed, timeEllapsed);
                    loadedEnergy += energyLoadedInDelta;
                    LoseCharge(energyLoadedInDelta);
                    bool shootingEndCondition = isBatteryOver || isOverHeated || timeEllapsed >= _maxEnergyLoadingDuration;
                    if(shootingEndCondition)
                    {
                        _isShootingStarted = false;
                    }
                    yield return null;
                }
                OnShootingEnded(loadedEnergy);
            }
        }

        IEnumerator CoolingRoutine(float timeToCool)
        {
            if (_isCooling)
            {
                yield break;
            }
            _isCooling = true;
            float subSteps = 50f;
            float temperatureStep = (Temperature - MinTemperature) / (subSteps * timeToCool);
            while(Temperature > MinTemperature)
            {
                if (_isShootingStarted)
                {
                    _isCooling = false;
                    yield break;
                }
                CoolDown(temperatureStep);
                yield return new WaitForSeconds(1f / subSteps);
            }
            _isCooling = false;
        }
    }
}
