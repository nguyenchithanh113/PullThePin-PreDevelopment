using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Gameplay.FillTheTruckMode{
    public class BallEntity : MonoBehaviour
    {
        EntityManager entityManager;
        [SerializeField] EntityType entityType;
        public EntityType EntityType => entityType;
        void Start()
        {
            
            
        }
        public void SetManager(EntityManager _entityManager){
            entityManager = _entityManager;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void Dispose(){
            entityManager.DisposeEntity(this);
            Destroy(gameObject);
        }
    }
}
