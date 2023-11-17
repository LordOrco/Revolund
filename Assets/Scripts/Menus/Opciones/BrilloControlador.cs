using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class BrilloControlador : MonoBehaviour
{
    public Volume postProcessVolume;

    public void ChangeBrightness(float value)
    {
        ColorAdjustments colorAdjustments;
        if (postProcessVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.postExposure.Override(value);

            // Puedes imprimir el valor actual para verificar en la consola
            //Debug.Log("Nuevo Brillo: " + value);
        }
    }
}
