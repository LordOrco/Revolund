using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHero : BaseUnit
{
    //Activa los highlights de las casillas
    public override void ShowPathingTiles()
    {
        SetHighlightedTiles(GridManager.instance.a_Star.ObtainAccesibleTiles(this));
        Debug.Log("HeroesPathing2 : " + GetHighlightedTiles().Count);
        for (int i = 0; i < GetHighlightedTiles().Count; i++)
        {
            GetHighlightedTiles()[i].heroesPathing++;
            //Debug.Log(GetHighlightedTiles()[i].heroesPathing);

            GetHighlightedTiles()[i].UpdateTileHighlight();
        }
        SetAreAccesibleTilesShown(true);

    }

    //Desactiva los highlights de las casillas
    public override void HidePathingTiles()
    {
        for (int i = 0; i < GetHighlightedTiles().Count; i++)
        {
            GetHighlightedTiles()[i].heroesPathing--;
            Debug.Log(GetHighlightedTiles()[i].heroesPathing);

            GetHighlightedTiles()[i].UpdateTileHighlight();
        }
        List<Tile> tiles = new List<Tile>();
        SetHighlightedTiles(tiles);

        SetAreAccesibleTilesShown(false);
    }
}
