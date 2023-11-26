using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private ShopManager shopManager;
    public int[] inventarioTropas;
    public int ultimaTropa;
    [SerializeField] public TextMeshProUGUI[] numTropas;
    void Start()
    {
        shopManager = GameObject.FindObjectOfType<ShopManager>();
    }

    public void colocarTropa(int indice)
    {
        if (inventarioTropas[indice] > 0)
        {
            ultimaTropa = indice;
            UnitManager.instance.SetSelectedHero(Instantiate(shopManager.tropas[indice], new Vector3(0, 0, -20), Quaternion.identity));
            inventarioTropas[indice]--;
            numTropas[indice].text = inventarioTropas[indice].ToString();
        }
    }

    public void cancelarColocacion()
    {
        if (UnitManager.instance.canInstance)
        {
            UnitManager.instance.SetSelectedHero(null);
            UnitManager.instance.canInstance = false;
            inventarioTropas[ultimaTropa]++;
        }
    }
}
