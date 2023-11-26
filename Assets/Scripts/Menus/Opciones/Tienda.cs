using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Tienda : MonoBehaviour
{
    [SerializeField]  List<PlantillaInformacionItem> informacionItems;
    [SerializeField]  GameObject plantillaObjetoTienda;
    [SerializeField] private TextMeshProUGUI textoMonedaTotales;
    void Start()
    {
        if (!PlayerPrefs.HasKey("monedasTotales"))
        {
            PlayerPrefs.SetInt("monedasTotales",900);
        }
        

        var plantillaItem = plantillaObjetoTienda.GetComponent<PlantillaItemTienda>();

        foreach (var item in informacionItems)
        {
            plantillaItem.imagen.sprite = item.image;
            plantillaItem.titulo.text = item.titulo;
            plantillaItem.textoPrecio.text = item.precio.ToString();

            Instantiate(plantillaItem, transform);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        textoMonedaTotales.text = PlayerPrefs.GetInt("monedasTotales").ToString();
    }
}
