using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHero : BaseUnit
{

    [SerializeField] public GameObject attackButton, endButton;

    //private bool isShowingButtons;

    protected override void Update()
    {


    }


    //Activa los highlights de las casillas
    public override void ShowPathingTiles()
    {
        SetHighlightedTiles(GridManager.instance.a_Star.ObtainAccesibleTiles(this));
        //Debug.Log("HeroesPathing2 : " + GetHighlightedTiles().Count);
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
            //Debug.Log(GetHighlightedTiles()[i].heroesPathing);

            GetHighlightedTiles()[i].UpdateTileHighlight();
        }
        List<Tile> tiles = new List<Tile>();
        SetHighlightedTiles(tiles);

        SetAreAccesibleTilesShown(false);
    }

    public override void MoveToTile(Tile targetTile)
    {
        if (canMove)
        {
            if (targetTile != null)
                targetTile.SetUnit(this);
            else
                Debug.Log("MoveToTile no hay camino");
            ///currentPath = null;

            canMove = false;
            if (Faction == Faction.Hero) { UnitManager.instance.heroesAttacked++; }
            //ShowButtons();
            this.gameObject.GetComponent<SpriteRenderer>().color = hasAttackedColor;
            UnitManager.instance.checkState();
            if (Faction == Faction.Hero) UnitManager.instance.heroesAttacked++;
        }
    }

    private void ShowButtons()
    {
        attackButton.SetActive(true);
        endButton.SetActive(true);
    }

    private void HideButtons()
    {
        attackButton.SetActive(false);
        endButton.SetActive(false);
    }
}
