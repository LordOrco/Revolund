using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolumen : MonoBehaviour
{

    public Slider controlVolumen;

    public GameObject[] audios;
    // Start is called before the first frame update
    void Start()
    {
        //Me pondra todos los objetos que compartan el tag de audio;
        audios = GameObject.FindGameObjectsWithTag("audio");
        controlVolumen.value = PlayerPrefs.GetFloat("volumenSave",1f);//carga la informacion guardada de los ajustes

    }

    private void Update()
    {
        foreach (GameObject au in audios)
        {
            au.GetComponent<AudioSource>().volume = controlVolumen.value;
        }
    }
    

    public void guardarVolumen()
    {
        PlayerPrefs.SetFloat("volumenSave",controlVolumen.value);
    }

}
