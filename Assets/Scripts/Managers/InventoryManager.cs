using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private ShopManager shopManager;
    public int[] inventarioTropas;
    public int ultimaTropa;
    [SerializeField] public TextMeshProUGUI[] numTropasGUI;
    [SerializeField] public List<Button> tropaSeleccionadaGUI = new List<Button>();
    [SerializeField] public List<Button> CancelarVenderGUI = new List<Button>();
    void Start()
    {
        shopManager = GameObject.FindObjectOfType<ShopManager>();
        foreach (Button b in CancelarVenderGUI)
        {
            b.gameObject.SetActive(false);
        }
    }

    public void colocarTropa(int indice)
    {
        if (inventarioTropas[indice] > 0)
        {
            ultimaTropa = indice;
            UnitManager.instance.SetSelectedHero(shopManager.tropas[indice]);       //Instantiate(shopManager.tropas[indice], new Vector3(0, 0, -20), Quaternion.identity));
            inventarioTropas[indice]--;
            numTropasGUI[indice].text = inventarioTropas[indice].ToString();

            Color temp = tropaSeleccionadaGUI[ultimaTropa].image.color;
            temp.a = 0.5f;
            tropaSeleccionadaGUI[ultimaTropa].image.color = temp;

            foreach (Button b in CancelarVenderGUI)
            {
                b.gameObject.SetActive(true);
            }
        }
    }

    public void cancelarColocacion()
    {
        if (UnitManager.instance.canInstance)
        {
            UnitManager.instance.cancelBuyUnit(null);
            inventarioTropas[ultimaTropa]++;
            numTropasGUI[ultimaTropa].text = inventarioTropas[ultimaTropa].ToString();

            Color temp = tropaSeleccionadaGUI[ultimaTropa].image.color;
            temp.a = 0.0f;
            tropaSeleccionadaGUI[ultimaTropa].image.color = temp;

            foreach (Button b in CancelarVenderGUI)
            {
                b.gameObject.SetActive(false);
            }
        }
    }

    public void venderColocacion()
    {
        if (UnitManager.instance.canInstance)
        {
            shopManager.añadirDinero(shopManager.precios[ultimaTropa]);
            UnitManager.instance.cancelBuyUnit(null);

            Color temp = tropaSeleccionadaGUI[ultimaTropa].image.color;
            temp.a = 0.0f;
            tropaSeleccionadaGUI[ultimaTropa].image.color = temp;

            foreach (Button b in CancelarVenderGUI)
            {
                b.gameObject.SetActive(false);
            }
        }
    }
}
