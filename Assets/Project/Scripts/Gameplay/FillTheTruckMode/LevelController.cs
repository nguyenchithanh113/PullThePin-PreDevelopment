using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Project.Scripts.Gameplay.FillTheTruckMode{
    public class LevelController : MonoBehaviour
    {
        //[SerializeField] List<EntityManager> lstEntityManager = new List<EntityManager>();
        [SerializeField] List<Truck> lstTruck = new List<Truck>();

        void Start()
        {
            Initialize();
        }
        void Initialize(){

        }
        
        // Update is called once per frame
        void Update()
        {
            
        }
        public void CheckGameEnd(){
            bool isAllFinished = true;
            for(int i = 0; i < lstTruck.Count; i++){
                if(lstTruck[i].IsFinished == false){
                    isAllFinished = false;
                    break;
                }
            }
            if(isAllFinished){
                GameController.OnGameFinish(GameState.Win);
            }else{
                GameController.OnGameFinish(GameState.Lose);
            }
        }
    }
}
