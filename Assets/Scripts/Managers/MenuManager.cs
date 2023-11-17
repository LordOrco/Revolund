using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Manager del menu, gestiona la UI
public class MenuManager : MonoBehaviour
{
    //Patron singleton, solo hay un MenuManager y se instancia a si mismo en Awake
    public static MenuManager Instance;

    //Cajas de texto de UI
    [SerializeField] private GameObject selectedHeroObject, tileObject, tileUnitObject;
    private void Awake()
    {
        Instance = this; 
    }

    //Enseña la info de la tile
    public void ShowTileInfo(Tile tile)
    {
        //Si no hay tile desactiva las informaciones de tile y sale
        if (tile == null)
        {
           tileObject.SetActive(false);
           tileUnitObject.SetActive(false);
           return;
        }
        //Activa la caja de texto de tile y pone el nombre.
        tileObject.GetComponentInChildren<TextMeshProUGUI>().text = tile.TileName;
        tileObject.SetActive(true);

        //Si hay una unidad en la tile, muestra su información
        if (tile.OccupiedUnit)
        {
            tileUnitObject.GetComponentInChildren<TextMeshProUGUI>().text = tile.OccupiedUnit.unitName;
            tileUnitObject.SetActive(true);
        }
        Debug.Log(tile.TileName);
    }
    //Muestra informacion del Heroe seleccionado.
    public void ShowSelectedHero(BaseHero hero)
    {
        if(hero == null) 
        {
            selectedHeroObject.SetActive(false);
            return;
        }
        selectedHeroObject.GetComponentInChildren<TextMeshProUGUI>().text = hero.unitName;
        selectedHeroObject.SetActive(true);
    }
}

