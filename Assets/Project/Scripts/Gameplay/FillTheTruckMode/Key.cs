using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts.Gameplay.FillTheTruckMode{
    public class Key : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 10;
        [SerializeField] float moveDistance = 10;

        Vector2 originalPos;
        bool isMove;
        void Start()
        {
            originalPos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if(isMove){
                Move();
            }
        }
        /// <summary>
        /// OnMouseDown is called when the user has pressed the mouse button while
        /// over the GUIElement or Collider.
        /// </summary>
        void OnMouseDown()
        {
            if(!isMove){
                isMove = true;
            }
        }
        void Move(){
            if(((Vector2)transform.position - originalPos).sqrMagnitude >= moveDistance * moveDistance ){
                isMove = false;
            }
            transform.position += transform.right * moveSpeed*Time.deltaTime;
        }
    }
}
