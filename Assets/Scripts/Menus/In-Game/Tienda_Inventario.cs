using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Tienda_Inventario : MonoBehaviour
{
    [SerializeField] private Sprite ToggledOn;
    [SerializeField] private Sprite ToggledOff;
    [SerializeField] private GameObject Toggled;
    // Start is called before the first frame update
    void Start()
    {
        Toggle();
    }

    public void Toggle()
    {
        if (GetComponent<UnityEngine.UI.Toggle>().isOn)
        {
            Toggled.SetActive(true);
            GetComponent<UnityEngine.UI.Image>().sprite = ToggledOn;
        }
        else
        {
            Toggled.SetActive(false);
            GetComponent<UnityEngine.UI.Image>().sprite = ToggledOff;
        }
    }

}
