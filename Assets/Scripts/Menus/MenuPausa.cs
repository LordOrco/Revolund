using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPausa : MonoBehaviour
{
    // Start is called before the first frame update
    public bool active = false;
    void Start()
    {
        gameObject.SetActive(false);
        active = false;
    }
    public void TogglePause()
    {
        active = !active;
        gameObject.SetActive(!gameObject.activeSelf);
        Time.timeScale = (gameObject.activeSelf) ? 0f : 1f;
    }

}
