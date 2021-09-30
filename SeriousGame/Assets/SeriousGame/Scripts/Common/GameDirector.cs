using System.Collections;
using System.Collections.Generic;
using SeriousGame.Common;
using SeriousGame.Management;
using UnityEngine;

namespace SeriousGame.Management
{
    public class GameDirector : Service
    {
        // Start is called before the first frame update
        void Start()
        {
            ServiceLocator.Instance.RegisterService<GameDirector>(this);
        }

    }
}

