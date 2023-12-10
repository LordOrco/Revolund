using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    
    [SerializeField] private GameObject LoseSign;
    [SerializeField] private GameObject VictorySign;
    [SerializeField] private GameObject GUIDesactivar;

    public static EndManager Instance;
    public delegate void SoundVictory(int sonido);
    public static event SoundVictory OnSoundVictory;
    public delegate void Soundefeat(int sonido);
    public static event Soundefeat OnSounDefeat;

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
    }
    public void Victory()
    {
        DesactivateGUI();
        {
            if (VictorySign != null)
            {
                VictorySign.SetActive(true);
                OnSoundVictory?.Invoke(2);
            }
        }
    }

    private void DesactivateGUI()
    {
        foreach(Transform child in GUIDesactivar.transform)
        {
            child.gameObject.SetActive(false);
            OnSounDefeat?.Invoke(3);
        }
    }
  }

