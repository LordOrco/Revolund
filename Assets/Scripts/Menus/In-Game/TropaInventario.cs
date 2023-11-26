using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TropaInventario : MonoBehaviour
{
    [SerializeField] public int indice;
    private InventoryManager inventoryManager;
    void Start()
    {
        inventoryManager = GameObject.FindFirstObjectByType<InventoryManager>();
    }

    public void Colocar()
    {
        inventoryManager.colocarTropa(indice);
    }
}
