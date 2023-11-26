using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    private Camera camara;
    private GridManager grid;   
    private UnitManager unitManager;
    private SettingsManager settings;
    [SerializeField] private MenuPausa pauseMenu;

    private float panSpeed = 10f;
    private float panBorderThickness = 10f;
    private Vector2 panLimit;
    private Vector2 gridCenter;

    private float zoomSpeed = 5f;
    private float maxZoom = 5f;
    private float minZoom = 1f;

    private int heroIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        camara = GameObject.FindObjectOfType<Camera>();
        grid = GameObject.FindObjectOfType<GridManager>();
        unitManager = GameObject.FindObjectOfType<UnitManager>();
        settings = GameObject.FindObjectOfType<SettingsManager>();

        gridCenter = grid.GetGridCenter();
        panLimit = new Vector2(gridCenter.x + (float) grid.GetWidth()/2, gridCenter.y + (float) grid.GetHeight()/2);
        Debug.Log(gridCenter.y);

        //SaveControlsArray("controls", controls);

        // Cargar el array desde PlayerPrefs
        //controls = LoadControlsArray("controls");

        // Imprimir el array cargado
        foreach (KeyCode keyCode in settings.GetControls())
        {
            Debug.Log(keyCode);
        }
    }

    // Update is called once per frame
    void Update()
    {
        cameraControls();
        PauseMenu();
    }

    public void cameraControls()
    {
        /////////////////////////////////////////////////////////////////////////////
        //MOVIMIENTO DE LA CÁMARA
        /////////////////////////////////////////////////////////////////////////////

        Vector3 pos = camara.transform.position;

        if(grid.GetWidth() > 2 * camara.orthographicSize * 16 / 9)
        {
            // Mover la cámara hacia la izquierda
            if (Input.GetKey(settings.GetControls()[1]) || Input.mousePosition.x <= panBorderThickness)
            {
                pos.x -= panSpeed * Time.deltaTime;
            }
            // Mover la cámara hacia la derecha
            if (Input.GetKey(settings.GetControls()[3]) || Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                pos.x += panSpeed * Time.deltaTime;
            }
            pos.x = Mathf.Clamp(pos.x, -0.5f + camara.orthographicSize * 16 / 9, panLimit.x - camara.orthographicSize * 16 / 9);
        }

        if (grid.GetHeight() > 2 * camara.orthographicSize)
        {
            // Mover la cámara hacia la arriba
            if (Input.GetKey(settings.GetControls()[0]) || Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                pos.y += panSpeed * Time.deltaTime;
            }
            // Mover la cámara hacia abajo
            if (Input.GetKey(settings.GetControls()[2]) || Input.mousePosition.y <= panBorderThickness)
            {
                pos.y -= panSpeed * Time.deltaTime;
            }
            pos.y = Mathf.Clamp(pos.y, -0.5f + camara.orthographicSize, panLimit.y - camara.orthographicSize);

        }
        camara.transform.position = pos;

        /////////////////////////////////////////////////////////////////////////////
        //ZOOM DE  LA CÁMARA
        /////////////////////////////////////////////////////////////////////////////

        float currentZoom = camara.orthographicSize;

        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        camara.orthographicSize = currentZoom;

        /////////////////////////////////////////////////////////////////////////////
        //SELECCIÓN DE TROPAS
        /////////////////////////////////////////////////////////////////////////////

        if (Input.GetKeyDown(settings.GetControls()[4]))
        {
            //Busca los heroes
            BaseHero[] unidades = GameObject.FindObjectsOfType<BaseHero>();
            int maxIndex = unidades.Length - 1;

            //Si el heroe no está seleccionado ya, lo selecciona
            unitManager.SetSelectedHero(unidades[heroIndex]);

            //Mueve la cámara al héroe con cuidado de no salirse
            if (grid.GetWidth() > 2 * camara.orthographicSize * 16 / 9)
            {
                pos.x = Mathf.Clamp(unidades[heroIndex].transform.position.x, -0.5f + camara.orthographicSize * 16 / 9, panLimit.x - camara.orthographicSize * 16 / 9);
            }
            if (grid.GetHeight() > 2 * camara.orthographicSize)
            {
                pos.y = Mathf.Clamp(unidades[heroIndex].transform.position.y, -0.5f + camara.orthographicSize, panLimit.y - camara.orthographicSize);
            }

            //Para cambiar al siguiente heroe si le da otra vez
            if(heroIndex < maxIndex)
            {
                heroIndex++;
            }
            else { heroIndex = 0;}
            
            //Se mueve la cámara
            camara.transform.position = pos;
        }
    }

    public void PauseMenu()
    {
        if (Input.GetKeyDown(settings.GetControls()[5]))
        {
            pauseMenu.TogglePause();
        }
    }
}
