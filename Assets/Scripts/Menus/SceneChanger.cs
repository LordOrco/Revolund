using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public int targetScene = 0;

    public void ChangeScene()
    {
        if(targetScene == -1)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(targetScene);
        }
    }
}
