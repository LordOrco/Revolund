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
        DesplegableTienda.OnSoundTienda += PlaySound;//Posicion 0
        ShopManager.OnSoundcomprar += PlaySound;//Posicion 1
        EndManager.OnSoundVictory += PlaySound;//Posicion2
        EndManager.OnSoundVictory += PlaySound;//Posicion 3
        UnitManager.OnSoundcolocarTropa += PlaySound;
        BaseUnit.OnSounKill += PlaySound;
        BaseUnit.OnSoundWalk += PlaySound;
        BaseUnit.OnSoundAttackHero += PlaySound;
        BaseHero.OnSoundAttackEnemy += PlaySound;
        AudioSource fuente1 =gameObject.GetComponent<AudioSource>();
        
    
        fuentes.Add(fuente1);
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