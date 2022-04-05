using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Gameplay.FillTheTruckMode{
    public class LevelLoader : Singleton<LevelLoader>
    {
        public LevelController levelController;
        public void LoadLevel(){
            levelController = GetComponentInChildren<LevelController>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
