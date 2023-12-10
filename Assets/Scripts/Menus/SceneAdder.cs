using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAdder : MonoBehaviour
{
    public int targetScene = 0;
    public int sceneIndex = 0;

   //public void sound1()
   //{
   //    //SoundManager.Instance.PlayClickInGame();
   //    Invoke("AddScene", 0.0f);
   //}
   //public void sound2()
   //{
   //    //PlayClick.InstanceS.PlayClickInGame();
   //    Invoke("UnloadScene", 0.0f);
   //}

    public void AddScene()
    {
       SceneManager.LoadScene(targetScene, LoadSceneMode.Additive);
    }

    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(sceneIndex);
    }
}
