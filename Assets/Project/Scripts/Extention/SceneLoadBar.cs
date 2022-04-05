using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class SceneLoadBar
{
    int progress;
    [SerializeField] Text textProgress;
    [SerializeField ]GameObject loadingPanel; 
    [SerializeField] Image imgProgress; 
    public void SetProgressBar(int progress){
        textProgress.text = progress.ToString()+"%";
    }
    public void OnStartLoad(){
        loadingPanel.SetActive(true);
    }
}
