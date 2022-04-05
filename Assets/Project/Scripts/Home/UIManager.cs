using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Home{
    public class UIManager : CanvasExtention
    {
        protected override void Awake()
        {
            base.Awake();
        }
        void Start()
        {
        
        }
        void Initialize(){

        }
        public void OnTapToPlayClick(){
            HomeManager.Instance.LoadBallGameplay();
        }
       
    }
}
