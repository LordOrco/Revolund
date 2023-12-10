using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip[] sonidos;
  
    public List<AudioSource>fuentes;

    private void Awake()
    {
        Instance = this;
        DoNotDestroy.instance.GetComponent<AudioSource>().Pause();
        
    }

    public void PlaySoundShop()
    {
        if (Instance != null)
            PlaySound(0);
    }

    public void PlayComprarMp3()
    {
        if (Instance != null)
            PlaySound(1);
    }

    public void PlayYouWinMusic()
    {
        if (Instance != null)
            PlaySound(2);
    }

    public void PlayYouLooseMusic()
    {
        if (Instance != null)
            PlaySound(3);
    }

    public void PlayInvocartropa()
    {
        if (Instance != null)
            PlaySound(4);
    }

    public void PlayDeadunits()
    {
        if (Instance != null)
            PlaySound(5);
    }

    public void PlayWalkunits()
    {
        if (Instance != null)
            PlaySound(6);
    }

    public void PlayCyberShot()
    {
        if (Instance != null)
            PlaySound(7);
    }

    public void PlaySteampunkShot()
    {
        if (Instance != null)
            PlaySound(8);
    }

    public void PlayClickInGame()
    {
        if (Instance != null)
            PlaySound(9);
    }

    public void PlaySound(int sonido){
        fuentes[0].clip = sonidos[sonido];
        fuentes[0].Play();
        }
        
    }