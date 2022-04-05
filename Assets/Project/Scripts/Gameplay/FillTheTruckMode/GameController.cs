using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Gameplay.FillTheTruckMode{
    public enum GameState{
        Win,
        Lose,
    }
    public class GameController : Singleton<GameController>
    {
        public static System.Action<GameState> OnGameFinish  = delegate {};
        protected override void Awake()
        {
            base.Awake();
            
        }
        private void OnEnable()
        {
            OnGameFinish += _OnGameFinish;
        }
        private void OnDestroy()
        {
            OnGameFinish -= _OnGameFinish;
        }
        void Start()
        {
            LevelLoader.Instance.LoadLevel();
        }

        void Update()
        {
            
        }
        public void LoadNextLevel(){

        }
        public void ReturnHome(){
            StartCoroutine(SceneLoadExtention.LoadSceneAsync("HomeScene",MainCanvas.Instance.SceneLoadBar,2f));
        }
        public void _OnGameFinish(GameState gameState){
            
            if(gameState == GameState.Win){
                Debug.Log("win");
            }else{
                Debug.Log("Lose");
            }
        }
    }
}
