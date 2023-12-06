using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioClip[] sonidos;
  
    public List<AudioSource>fuentes;
    private void Awake()
    {
        //Los distintos eventos que estan subcritos al Soundmanager 
        //OpenShop.OnSoundVolar += PlaySound;
       // Goal.onSoundspoints+= PlaySound;
       // GameManager.onSoundChoque+= PlaySound;
       // GameManager.onSounMenu += PlaySound;
       DesplegableTienda.OnSoundTienda += PlaySound;
        AudioSource fuente1 =gameObject.GetComponent<AudioSource>();
    
        fuentes.Add(fuente1);
       
        //Nota para añadir un sonido puedo ir añadiendo poco a poco 
        //Pero si tengo varios sonido hare un bucle for para ahorrar tiempo
        //Tambien para tener menos linea de codigo 
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
//Metodo para invocar el sonido en concreto se neceita un entero para invocar el sonido en especifico 
    public void PlaySound(int sonido){
        fuentes[0].clip = sonidos[sonido];
        fuentes[0].Play();
        }
        
    }