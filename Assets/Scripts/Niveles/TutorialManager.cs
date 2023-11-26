using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public string[] objetivos = new string[8];
    public int objetivoActual = 0;
    [SerializeField] private TextMeshProUGUI ObjetivoGUI;
    private SettingsManager a;
    private ShopManager b;

    // Start is called before the first frame update
    void Start()
    {
        a = GameObject.FindObjectOfType<SettingsManager>();
        b = GameObject.FindObjectOfType<ShopManager>();
        cargarObjetivos();
        ObjetivoGUI.text = objetivos[0];
    }

    // Update is called once per frame
    void Update()
    {
        switch (objetivoActual)
        {
            case 0:
                if (Input.GetKey(a.GetControls()[4]))
                {
                    objetivoActual++;
                    ObjetivoGUI.text = objetivos[objetivoActual];
                }
                break;
            case 1:
                if(GameManager.Instance.State == GameManager.GameState.EnemyTurn)
                {
                    objetivoActual++;
                    ObjetivoGUI.text = objetivos[objetivoActual];
                }
                break;
            case 2:
                if (b.dinero < b.dineroInicial)
                {
                    objetivoActual++;
                    ObjetivoGUI.text = objetivos[objetivoActual];
                }
                break;
            case 3:
                if (b.dinero < b.dineroInicial)
                {
                    objetivoActual++;
                    ObjetivoGUI.text = objetivos[objetivoActual];
                }
                break;
            case 4:
                if(!b.isActiveAndEnabled) 
                {
                    objetivoActual++;
                    ObjetivoGUI.text = objetivos[objetivoActual];
                }
                break;
            case 5:
                if(GameManager.Instance.State == GameManager.GameState.Victory)
                {
                    objetivoActual++;
                    ObjetivoGUI.text = objetivos[objetivoActual];
                }
                break;
        }
    }

    public void cargarObjetivos()
    {
        objetivos[0] = "Pulsa Espacio para encontrar tu tropa";
        objetivos[1] = "Mueve tu tropa";
        objetivos[2] = "Compra una tropa";
        objetivos[3] = "Abre tu inventario";
        objetivos[4] = "Coloca tu tropa en una torre de despliegue";
        objetivos[5] = "¡Acaba con los enemigos y conquista las torres!";
        objetivos[6] = "Ganaste!";
    }
}
