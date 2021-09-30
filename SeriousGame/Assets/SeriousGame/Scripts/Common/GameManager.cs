using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SeriousGame.Common;

namespace SeriousGame.Management
{
    public enum GameLevel
    {
        StartScene,
        GameScene,
        FinalScene
    }
    public class GameManager : Service
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            ServiceLocator.Instance.RegisterService<GameManager>(this);
        }

        public void LoadLevel(GameLevel level)
        {
            string levelName = Enum.GetName(typeof(GameLevel), level);
            try
            {
                SceneManager.LoadSceneAsync(levelName);
            }
            catch(Exception)
            {
                Debug.LogError("No level named " + levelName + "was found.");
            }
        }
    }
}

