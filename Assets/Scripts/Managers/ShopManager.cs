using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] public List<BaseHero> tropas = new List<BaseHero>();
    [SerializeField] private TextMeshProUGUI dineroGUI;
    private InventoryManager inventoryManager;
    private int dinero = 0;
    private int dineroInicial = 1000;
    [SerializeField] public List<Button> poderComprarGUI = new List<Button>();
    public List<int> precios = new List<int>();
    void Start()
    {
        inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
        cargarTienda();
    }

    private void cargarTienda()
    {
        dinero = dineroInicial;
        dineroGUI.text = dinero.ToString();
        precios.Add(250);
    }

    public void comprarTropa(int indice)
    {
        if(dinero >= precios[indice])
        {
            inventoryManager.inventarioTropas[indice]++;
            inventoryManager.numTropasGUI[indice].text = inventoryManager.inventarioTropas[indice].ToString();

            dinero -= precios[indice];
            dineroGUI.text = dinero.ToString();
            actualizarPoderComprar();
        }
    }

    public void a�adirDinero(int dineroExtra)
    {
        dinero += dineroExtra;
        dineroGUI.text = dinero.ToString();
        actualizarPoderComprar();
    }

    public void actualizarPoderComprar()
    {
        foreach(Button b in poderComprarGUI)
        {
            if(precios[b.GetComponent<TropaTienda>().indice] > dinero)
            {
                Color temp = b.image.color;
                temp.a = 0.5f;
                b.image.color = temp;
            }
            else
            {
                Color temp = b.image.color;
                temp.a = 0.0f;
                b.image.color = temp;
            }
        }
    }

}