using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseEnemy : BaseUnit
{

    public Slider barraVida;

    public void Awake()
    {
        Debug.Log("Awake");
        barraVida.maxValue = HP;
        barraVida.minValue= 0;

    }
    //Activa los highlights de las casillas
    public override void ShowPathingTiles()
    {
        if (!GetAreAccesibleTilesShown())
        {
            SetHighlightedTiles(GridManager.instance.a_Star.ObtainAccesibleTiles(this));
            //Activa los highlights de las casillas
            for (int i = 0; i < GetHighlightedTiles().Count; i++)
            {
                GetHighlightedTiles()[i].enemiesPathing++;
                Debug.Log(GetHighlightedTiles()[i].enemiesPathing);
                GetHighlightedTiles()[i].UpdateTileHighlight();
            }
            SetAreAccesibleTilesShown(true);
        }
    }
    //Desactiva los highlights de las casillas
    public override void HidePathingTiles()
    {
        if (GetAreAccesibleTilesShown())
        {
            for (int i = 0; i < GetHighlightedTiles().Count; i++)
            {
                GetHighlightedTiles()[i].enemiesPathing--;
                Debug.Log(GetHighlightedTiles()[i].enemiesPathing);

                GetHighlightedTiles()[i].UpdateTileHighlight();
            }
            SetHighlightedTiles(null);

            SetAreAccesibleTilesShown(false);
        }
    }
    public virtual void ReceiveDmg(int dmg)
    {
        barraVida.value = HP;
        HP -= dmg;
        Debug.Log("HP :" + HP);
        if (HP <= 0)
        {
            Kill();
        }
    }
}
