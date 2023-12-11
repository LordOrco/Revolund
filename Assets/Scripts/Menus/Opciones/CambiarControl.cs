using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class CambiarControl : MonoBehaviour
{
    [SerializeField] private int index;
    private bool waitingForKey = false;
    private SettingsManager settings;
    private TextMeshProUGUI boton;
    private Button[] demasBotones;

    void Start()
    {
        settings = GameObject.FindGameObjectWithTag("OpcionesManager").GetComponent<SettingsManager>();
        demasBotones = GameObject.FindObjectsOfType<Button>();
        boton = GetComponent<TextMeshProUGUI>();
    }

    public void clicked()
    {
        waitingForKey = !waitingForKey;
        if(waitingForKey)
        {
            foreach (Button button in demasBotones)
            {
                button.interactable = false;
            }
        }
        else
        {
            updateAllTexts();
        }
    }

    void Update()
    {
        if (waitingForKey)
        {
            boton.text = "...";
            foreach (KeyCode keycode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if(Input.GetKeyDown(keycode)) {
                    if(keycode == KeyCode.Escape || keycode == KeyCode.Mouse0 || keycode == KeyCode.Mouse1)
                    {
                        clicked();
                    }
                    else
                    {
                        settings.SetControl(index, keycode);
                        //Debug.Log("#", settings);
                        clicked();
                    }
                }
            }
        }
        else
        {
            boton.text = settings.GetControls()[index].ToString();
        }
    }

    public void updateText()
    {
        boton.text = settings.GetControls()[index].ToString();
    }

    public void updateAllTexts()
    {
        foreach (Button button in demasBotones)
        {
            button.interactable = true;
            if (button.GetComponentInChildren<CambiarControl>() != null)
            {
                button.GetComponentInChildren<CambiarControl>().updateText();
            }
        }
    }

    public void defaultControls()
    {
        settings.ResetControls();
        updateAllTexts();
    }
}
