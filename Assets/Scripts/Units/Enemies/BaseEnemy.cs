using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseUnit
{
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
        else
        {
            HidePathingTiles();
        }
    }

    public override void HidePathingTiles()
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
