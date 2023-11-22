using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class BrilloControlador : MonoBehaviour
{
    [SerializeField] private RawImage darkOverlay;
    public void ChangeBrightness(float value)
    {
        Debug.Log(value);
        var tempColor = darkOverlay.color;
        tempColor.a = value;
        darkOverlay.color = tempColor;
    }
}
