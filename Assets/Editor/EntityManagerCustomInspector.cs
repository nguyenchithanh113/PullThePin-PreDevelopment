using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Project.Scripts.Gameplay.FillTheTruckMode;

[CustomEditor(typeof(EntityManager))]
public class EntityManagerCustomInspector : Editor
{
    EntityManager entityManager;
    private void OnEnable()
    {
        entityManager = (EntityManager)target;
        
    }
    public override void OnInspectorGUI()
    {
        using(var check = new EditorGUI.ChangeCheckScope()){
            base.OnInspectorGUI();
            if(GUILayout.Button("Get Entity")){
                entityManager.GetEntity();
            }

            if(check.changed){
                entityManager.SetEntityInfo();
            }
        }
    }
}
