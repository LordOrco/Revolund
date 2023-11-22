using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MantenerValoresOpciones : MonoBehaviour
{
    public Slider brightness;
    public Slider volume;
    //public ToggleGroup fontSize;
    public SettingsManager settingsManager;

    private void Start()
    {
        brightness.value = settingsManager.GetBrightness();
        volume.value = settingsManager.GetVolume();
        LoadFontSizeToggleState();
    }

    private void LoadFontSizeToggleState()
    {
        switch (settingsManager.GetFontSize())
        {
            case 0:
                //Debug.Log(settingsManager.GetFontSize());
                GameObject.Find("FuentePequeña").GetComponent<Toggle>().isOn = true;
                break;
            case 1:
                GameObject.Find("FuenteMediana").GetComponent<Toggle>().isOn = true;
                break;
            case 2:
                GameObject.Find("FuenteGrande").GetComponent<Toggle>().isOn = true;
                break;
            default:
                Debug.Log("Algo va mal");
                break;
        }       
    }
}
