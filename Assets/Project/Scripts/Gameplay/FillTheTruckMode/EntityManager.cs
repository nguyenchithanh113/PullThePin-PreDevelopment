using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Gameplay.FillTheTruckMode{
    public enum EntityType{
        Yellow,
        White,
    }
    public class EntityManager : MonoBehaviour
    {
        
        [SerializeField] List<BallEntity> lstEntity = new List<BallEntity>();
        
        public List<BallEntity> LstEntity => lstEntity;
        [TextArea,SerializeField] string EntityInfo;
        LevelController levelController;
        private void Start()
        {
            Initialize();
        }
        void Initialize(){
            GetComponent();
            SetEntityManager();
        }
        void GetComponent(){
            levelController = GetComponentInParent<LevelController>();
        }
        void SetEntityManager(){
            foreach(BallEntity entity in lstEntity){
                entity.SetManager(this);
            }
        }
        public void SetEntityInfo(){
            EntityInfo = string.Empty;
            Dictionary<EntityType, int> entityDic = new Dictionary<EntityType, int>();
            for(int i = 0; i < lstEntity.Count; i++){
                if(!entityDic.ContainsKey(lstEntity[i].EntityType)){
                    entityDic.Add(lstEntity[i].EntityType,0);
                }
                entityDic[lstEntity[i].EntityType]++;
            }
            foreach(KeyValuePair<EntityType,int> keyValue in entityDic){
                EntityInfo += keyValue.Key.ToString() + " Count " + keyValue.Value + "\n";
            }
        }
        public void GetEntity(){
            lstEntity.Clear();
            foreach(Transform tr in transform){
                BallEntity _entity = tr.GetComponent<BallEntity>();
                if(_entity!=null) lstEntity.Add(_entity);
            }
        }
        public void DisposeEntity(BallEntity entity){
            lstEntity.Remove(entity);
            CheckEnd();
        }
        public void AddEntity(BallEntity entity){
            lstEntity.Add(entity);
        }
        public void CheckEnd(){
            if(lstEntity.Count==0){
                levelController.CheckGameEnd();
            }
        }
    }
}
