using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesplegableTienda : MonoBehaviour
{
    private bool desplegado;
    [SerializeField] private GameObject tienda;
    [SerializeField] private Sprite Abrir;
    [SerializeField] private Sprite Cerrar;
    void Start()
    {
        desplegado = true;
        gameObject.GetComponent<Image>().sprite = Cerrar;
    }

    public void ToggleDespliegue()
    {

        if (desplegado)
        {
            Vector3 pos = new Vector3(tienda.transform.position.x, tienda.transform.position .y - 100, tienda.transform.position.z);
            tienda.transform.position = pos;
            gameObject.GetComponent<Image>().sprite = Abrir;
        }
        else
        {
            Vector3 pos = new Vector3(tienda.transform.position.x, tienda.transform.position.y + 100, tienda.transform.position.z);
            tienda.transform.position = pos;
            gameObject.GetComponent<Image>().sprite = Cerrar;
        }
        desplegado = !desplegado;
    }
    
}
