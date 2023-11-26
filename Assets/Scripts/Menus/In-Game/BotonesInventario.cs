using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonesInventario : MonoBehaviour
{
    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
    }

    public void Vender()
    {
        inventoryManager.venderColocacion();
    }
    public void Cancelar()
    {
        inventoryManager.cancelarColocacion();
    }
}
