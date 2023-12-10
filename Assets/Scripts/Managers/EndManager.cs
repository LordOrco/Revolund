using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    
    [SerializeField] private GameObject LoseSign;
    [SerializeField] private GameObject VictorySign;
    [SerializeField] private GameObject GUIDesactivar;

    public static EndManager Instance;

    private void Awake()
    {
        Instance = this; 
    }
    public void Lose()
    {
        DesactivateGUI();
        if (LoseSign != null)
        {
            LoseSign.SetActive(true);

        }
        Invoke("ReturnJaja", 3f);
    }
    public void Victory()
    {
        DesactivateGUI();       
        if (VictorySign != null)
        {
            VictorySign.SetActive(true);
            SoundManager.Instance.PlayYouWinMusic();
        }
        Invoke("ReturnJaja", 3f);
    }

    private void DesactivateGUI()
    {
        foreach(Transform child in GUIDesactivar.transform)
        {
            child.gameObject.SetActive(false);
            SoundManager.Instance.PlayYouLooseMusic();
        }
    }

    private void ReturnJaja()
    {
        SceneManager.LoadScene(0);
    }
}

