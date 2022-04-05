using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Gameplay.FillTheTruckMode{
    public class Truck : MonoBehaviour
    {
        [SerializeField] EntityType truckType;
        [SerializeField] int totalNeedEntity;
        bool isFinished;
        public bool IsFinished=>isFinished;
        private void OnTriggerEnter2D(Collider2D other)
        {
            BallEntity entity = other.GetComponent<BallEntity>();
    
            if(entity!=null){
                if(entity.EntityType == truckType){
                    totalNeedEntity--;
                    totalNeedEntity = Mathf.Max(0,totalNeedEntity);
                    if(totalNeedEntity==0){
                        isFinished = true;
                    }
                }
                entity.Dispose();
            }
        }
    }
}
