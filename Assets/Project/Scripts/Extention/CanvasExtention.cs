using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasExtention : Singleton<CanvasExtention>
{
    
    [SerializeField] protected SceneLoadBar sceneLoadBar;
    public SceneLoadBar SceneLoadBar => sceneLoadBar;
}
