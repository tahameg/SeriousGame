using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SeriousGame.Common;
using SeriousGame.Gameplay;
using SeriousGame.Management;

public class HudManager : Service
{
    public RawImage ChargeCapacity;
    public Text ChargeText;
    public Text ChargeTextMax;

    public RawImage TemperatureCapacity;
    public Text TemperatureText;
    public Text TemperatureTextMax;

    public RawImage HealthCapacity;
    public Text HealthText;
    public Text HealthTextMax;

    private ServiceLocator _serviceLocator;
    private GameDirector _gameDirector;
    private float _healthMax;
    private float _temperatureMax;
    private float _temperatureMin;
    private float _chargeMax;

    public bool isInitialized = false;
    public bool areServicesInitialized
    {
        get
        {
            return _gameDirector != null;
        }
    }
    private void Start()
    {
        _serviceLocator = ServiceLocator.Instance;
        _serviceLocator.RegisterService(this);
        _gameDirector = _serviceLocator.GetService<GameDirector>();
        if (!areServicesInitialized)
        {
            _serviceLocator.ServiceRegistered += OnServiceRegistered;
        }
    }

    public void Initialize(float chargeMax, float temperatureMax, float temperatureMin, float healthMax)
    {
        _chargeMax = chargeMax;
        _temperatureMax = temperatureMax;
        _temperatureMin = temperatureMin;
        _healthMax = healthMax;

        TemperatureTextMax.text = _temperatureMax.ToString();
        HealthTextMax.text = _healthMax.ToString("#.##");
        ChargeTextMax.text = _chargeMax.ToString("#.##");
        isInitialized = true;
    }
    private void OnServiceRegistered(Type type)
    {
    }

    public void ShowTemperature(float temperature)
    {
        float ratio = (temperature - _temperatureMin) / (_temperatureMax - _temperatureMin);
        TemperatureText.text = temperature.ToString("#.##");
        TemperatureCapacity.rectTransform.localScale = new Vector3(ratio, 1.0f, 1.0f);
    }

    public void ShowCharge(float charge)
    {
        float ratio = charge / _chargeMax;
        ChargeText.text = charge.ToString("#.##");
        ChargeCapacity.rectTransform.localScale = new Vector3(ratio, 1.0f, 1.0f);
    }

    public void ShowHealth(float health)
    {
        float ratio = health / _healthMax;
        HealthText.text = health.ToString("#.##");
        HealthCapacity.rectTransform.localScale = new Vector3(ratio, 1.0f, 1.0f);
    }
}
