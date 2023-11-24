using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    private RawImage darkOverlay;
    static private Color brightness;

    private AudioSource[] audios;
    static private float volume;

    static private Text[] texts;
    static public int[] fontSizes = new int[3] {14,20,25};
    static private int fontSize;


    private void Start()
    {
        brightness = new Color(0, 0, 0, PlayerPrefs.GetFloat("brightness", 0.0f));
        volume = PlayerPrefs.GetFloat("volume", 1f);
        fontSize = PlayerPrefs.GetInt("fontSize", 0);
        ApplyOptions();

    }

    public void ApplyOptions()
    {
        darkOverlay = GetComponent<RawImage>();
        darkOverlay.color = brightness;

        audios = GameObject.FindObjectsOfType<AudioSource>();
        foreach (AudioSource au in audios)
        {
            au.volume = volume;
        }

        texts = GameObject.FindObjectsOfType<Text>();
        foreach (Text txt in texts)
        {
            txt.fontSize = fontSizes[fontSize];
            txt.horizontalOverflow = HorizontalWrapMode.Overflow;
            txt.verticalOverflow = VerticalWrapMode.Overflow;
        }
    }

    public void SetBrightness(float value)
    {
        //Debug.Log(value);
        brightness.a = value;
        PlayerPrefs.SetFloat("brightness", value);
        darkOverlay.color = brightness;
    }

    public float GetBrightness()
    {
        return brightness.a;
    }

    public void SetVolume(float value)
    {
        volume = value;
        PlayerPrefs.SetFloat("volume", value);

        audios = GameObject.FindObjectsOfType<AudioSource>();
        foreach (AudioSource au in audios)
        {
            au.volume = volume;
        }
    }
    public float GetVolume()
    {
        return volume;
    }

    public void SetFontSize(int value)
    {
        fontSize = value;
        PlayerPrefs.SetFloat("fontSize", value);
        texts = GameObject.FindObjectsOfType<Text>();
        foreach (Text txt in texts)
        {
            txt.fontSize = fontSizes[fontSize];
            txt.horizontalOverflow = HorizontalWrapMode.Overflow;
            txt.verticalOverflow = VerticalWrapMode.Overflow;
        }
    }

    public int GetFontSize()
    {
        return fontSize;
    }
}
