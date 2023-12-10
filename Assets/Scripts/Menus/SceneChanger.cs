using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public int targetScene = 0;
    //public void sound()
    //{
    //    //SoundManager.Instance.PlayClickInGame();
    //    Invoke("ChangeScene",0.0f);
    //}

    public void ChangeScene()
    {
        //SoundManager.Instance.PlayClickInGame();

        if (targetScene == -1)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(targetScene);
            //SoundManager.Instance.PlayClickInGame();
        }
    }
}
