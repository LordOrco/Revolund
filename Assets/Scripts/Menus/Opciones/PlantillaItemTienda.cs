using System.Collections;
using System.Collections.Generic;
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
    }

    // Update is called once per frame
    void Update()
    {
        monedasTotales = PlayerPrefs.GetInt("monedasTotales");
        if (precio > monedasTotales)
        {
            botonComprar.interactable = false;
        }
    }

    public void Comprar()
    {
        monedasTotales -= precio;
        PlayerPrefs.SetInt("monedasTotales",monedasTotales);
    }
}
