using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantillaItemTienda : MonoBehaviour
{
    public Image imagen;
    public TextMeshProUGUI textoPrecio;
    public TextMeshProUGUI titulo;
    public Button botonComprar;
    int precio;
    private int monedasTotales;
    void Start()
    {
        precio = int.Parse(textoPrecio.text);
        //monedasTotales = PlayerPrefs.GetInt("monedasTotales");
        monedasTotales = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        if (precio > monedasTotales)
        {
            botonComprar.interactable = false;
            botonComprar.image.color = new Color(0, 0, 0, 0.5f);
        }
    }

    public void Comprar()
    {
        monedasTotales -= precio;
        PlayerPrefs.SetInt("monedasTotales",monedasTotales);
    }
}
