using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAdder : MonoBehaviour
{
    public int targetScene = 0;
    public int sceneIndex = 0;

    public void AddScene()
    {
       SceneManager.LoadScene(targetScene, LoadSceneMode.Additive);
    }

    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(sceneIndex);
    }
}
