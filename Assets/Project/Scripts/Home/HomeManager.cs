using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Home{
    public class HomeManager : Singleton<HomeManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public void LoadBallGameplay(){
            StartCoroutine(SceneLoadExtention.LoadSceneAsync("TruckGameplayScene",UIManager.Instance.SceneLoadBar,2));
        }
    }    
}
