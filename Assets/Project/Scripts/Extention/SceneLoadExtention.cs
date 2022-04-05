using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadExtention 
{
    public static IEnumerator LoadSceneAsync(string sceneName,SceneLoadBar sceneLoadBar, float fakeTime = 1){
        sceneLoadBar.OnStartLoad();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        float loadTime = fakeTime;
        float timeStep = Time.deltaTime;
        WaitForSeconds wait = new WaitForSeconds(timeStep);
        operation.allowSceneActivation = false;
        while(loadTime > 0){
            loadTime -= timeStep;
            loadTime = Mathf.Clamp(loadTime,0,1);
            int progress = Mathf.CeilToInt((1- loadTime)*100);
            sceneLoadBar.SetProgressBar(progress);
            yield return wait;
        }
        operation.allowSceneActivation = true;
            
    }
}
