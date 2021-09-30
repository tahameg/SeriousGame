using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGame.Common
{
    public abstract class Singleton<T> : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if ( _instance == null )
                {
                    var instanceObject = FindObjectOfType<Singleton<T>>() as GameObject;
                    UnityEngine.Assertions.Assert.IsNotNull(instanceObject, "Could not initiate the instance");
                    _instance = instanceObject.GetComponent<T>();
                    return _instance;
                }
                else
                {
                    return _instance;
                }
            }
        }
        private static T _instance;

        protected void CreateSingleton( T newInstance )
        {
            var existingInstances = FindObjectsOfType<Singleton<T>>();
            if ( existingInstances.Length > 1 )
            {
                Destroy( gameObject );
                return;
            }

            if ( _instance == null )
            {
                _instance = newInstance;
            }
            else if ( _instance.Equals(newInstance) )
            {
                Destroy(gameObject);
            }
        }
    }
}

