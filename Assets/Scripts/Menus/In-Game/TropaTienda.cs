using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TropaTienda : MonoBehaviour
{
    [SerializeField] public int indice;
    private ShopManager shopManager;
    void Start()
    {
        shopManager = GameObject.FindFirstObjectByType<ShopManager>();
    }

    public void Comprar()
    {
        shopManager.comprarTropa(indice);
    }
}
