using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoNotDestroy : MonoBehaviour
{
    public static DoNotDestroy instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    //private void Update()
    //{
    //    if (SceneManager.GetActiveScene().name == "EscenaPrincipal")
    //        instance.GetComponent<AudioSource>().Pause();
    //}
}
