using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Common
{
    public class ServiceLocator : Singleton<ServiceLocator>
    {
        private Dictionary<Type, Service> _services;
        public delegate void ServiceRegisterHandler(Type type);
        
        public event ServiceRegisterHandler ServiceRegistered;
        public event ServiceRegisterHandler ServiceUnregistered;

        private void Awake()
        {
            CreateSingleton( this );
            _services = new Dictionary<Type, Service>();
        }

        /// <summary>
        /// Retrieves a service of type T using the Service Locator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Instance of the MonoBehaviour-derived class T if it is found in the scene and it is registered.
        /// Otherwise returns null.
        /// </returns>
        public T GetService<T>() where T : Service, new()
        {
            RejectIfNotInitialized();
            if ( !_services.ContainsKey(typeof(T)) )
            {
                return null; //service was not registered
            }
            
            var service = (T)_services[typeof(T)];
            if( service == null )
            {
                return null; 
            }

            return service;
        }

        public bool IsServiceAvailable<T>() where T : Service, new()
        {
            RejectIfNotInitialized();
            return _services.ContainsKey(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newService"></param>
        public void RegisterService<T>(T newService) where T : Service, new()
        {
            RejectIfNotInitialized();
            bool isServiceLocated = _services.ContainsKey(typeof(T));
            if (!isServiceLocated)
            {
                _services.Add(typeof(T), FindObjectOfType<T>());
                ServiceRegistered?.Invoke(typeof(T));
            }
        }

        public Service[] GetServices()
        {
            Service [] services = new Service[_services.Count];
            int indexCounter = 0;
            foreach(KeyValuePair<Type, Service> item in _services)
            {
                services[indexCounter] = item.Value;
                indexCounter++;
            }
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void UnregisterService<T>() where T : MonoBehaviour, new()
        {
            RejectIfNotInitialized();
            var isServiceLocated = _services.ContainsKey(typeof(T));
            if ( !isServiceLocated )
            {
                return;
            }
            _services.Remove(typeof(T));
            ServiceUnregistered?.Invoke(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            RejectIfNotInitialized();
            _services.Clear();
        }
        private void RejectIfNotInitialized()
        {
            UnityEngine.Assertions.Assert.IsNotNull(_services, "Called before the Locator was initialized");
        }

    }
}

//TODO: Queue register requests in case they come before initialization