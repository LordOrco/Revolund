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
    [SerializeField] private GameObject BtCoord1, BtCoord2;
    public delegate void SoundTienda(int sonido);
    public static event SoundTienda OnSoundTienda;
    void Start()
    {
        desplegado = true;
        gameObject.GetComponent<Image>().sprite = Cerrar;
    }

    public void ToggleDespliegue()
    {

        if (desplegado)
        {
            /*
            Vector3 pos = new Vector3(tienda.transform.position.x, tienda.transform.position .y - 100, tienda.transform.position.z);
            tienda.transform.position = pos;
            */
            //tienda.SetActive(false);
            foreach (Transform child in tienda.transform)
            {
                if(child != gameObject.transform)
                    child.gameObject.SetActive(false);
            }
            gameObject.GetComponent<Image>().sprite = Abrir;
            SoundManager.Instance.PlaySoundShop();//Invocacion del sonido tienda 
            this.transform.position = BtCoord1.transform.position;
            
        }
        else
        {
            /*
            Vector3 pos = new Vector3(tienda.transform.position.x, tienda.transform.position.y + 100, tienda.transform.position.z);
            tienda.transform.position = pos;
            */
            //tienda.SetActive(true);
            foreach (Transform child in tienda.transform)
            {
                if (child != gameObject.transform)
                    child.gameObject.SetActive(true);
            }
            gameObject.GetComponent<Image>().sprite = Cerrar;
            SoundManager.Instance.PlaySoundShop();
            this.transform.position = BtCoord2.transform.position;
        }
        desplegado = !desplegado;
    }
    
}
