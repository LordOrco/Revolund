using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;
using System.Xml.Linq;
using UnityEditor;

public class SettingsManager : MonoBehaviour
{
    private RawImage darkOverlay;
    static private Color brightness;

    private AudioSource[] audios;
    static private float volume;

    static private Text[] texts;
    static public int[] fontSizes = new int[3] {14,20,25};
    static private int fontSize;

    private KeyCode[] defaultControls = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space, KeyCode.Escape };
    public KeyCode[] controls = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space, KeyCode.Escape };


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
            txt.verticalOverflow = VerticalWrapMode.Truncate;
            txt.horizontalOverflow = HorizontalWrapMode.Overflow;
        }

        controls = LoadControlsArray();
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
            txt.verticalOverflow = VerticalWrapMode.Truncate;
            txt.horizontalOverflow = HorizontalWrapMode.Overflow;
        }
    }

    public int GetFontSize()
    {
        return fontSize;
    }

    //GUARDADO Y CARGADO DE CONTROLES
    public void SaveControlsArray()
    {
        // Convertir el array de KeyCode a una cadena de texto
        string arrayString = KeyCodeArrayToString(controls);
        Debug.Log(arrayString + "1");
        // Guardar la cadena de texto en PlayerPrefs
        PlayerPrefs.SetString("controls", arrayString);
        PlayerPrefs.Save();
    }

    public KeyCode[] LoadControlsArray()
    {
        // Recuperar la cadena de texto desde PlayerPrefs
        string loadedArrayString = PlayerPrefs.GetString("controls", "W,A,S,D,Space,Escape");

        // Convertir la cadena de texto a un array de KeyCode
        return StringToKeyCodeArray(loadedArrayString);
    }

    public string KeyCodeArrayToString(KeyCode[] array)
    {
        // Convertir el array de KeyCode a una cadena de texto separada por comas
        string[] stringValues = new string[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            stringValues[i] = array[i].ToString();
        }
        Debug.Log(stringValues);
        return string.Join(",", stringValues);
    }

    public KeyCode[] StringToKeyCodeArray(string arrayString)
    {
        // Convertir la cadena de texto a un array de KeyCode
        string[] stringValues = arrayString.Split(',');
        KeyCode[] array = new KeyCode[stringValues.Length];

        for (int i = 0; i < stringValues.Length; i++)
        {
            Enum.TryParse(stringValues[i], out array[i]);
        }

        return array;
    }

    public void SetControl(int index, KeyCode control)
    {
        for(int i = 0; i < controls.Length; i++)
        {
            if (controls[i] == control)
            {
                controls[i] = KeyCode.None;
            }
        }
        controls[index] = control;
    }

    public void ResetControls()
    {
        for(int i = 0; i<controls.Length; i++)
        {
            controls[i] = defaultControls[i];
        }
    }

    public KeyCode[] GetControls()
    {
        return controls;
    }
}
