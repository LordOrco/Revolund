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
            {
                SoundManager.Instance.PlayWalkunits();
                targetTile.SetUnit(this);
            }
                //targetTile.SetUnit(this);
            else
                Debug.Log("MoveToTile no hay camino");
            ///currentPath = null;
            EndMovement();
        }
    }
    public void AttackOnplace(BaseUnit enemy)
    {
        if (enemy != null && canAttack)
        {
            ApplyDmg(enemy);
            SoundManager.Instance.PlaySteampunkShot();
        }
        attacking = false;
        canAttack = false;
        this.gameObject.GetComponent<SpriteRenderer>().color = hasAttackedColor;
        UnitManager.instance.checkState();
    }

    private void EndMovement()
    {
        canMove = false;
        //bool enemyClose = false;

        UnitManager.instance.heroesAttacked++;

        for(int i = 0;i< GetOccupiedTile().node.adyacent_Nodes.Count;i++)
        {
            var enemy = GetOccupiedTile().node.adyacent_Nodes[i].myTile.OccupiedUnit;
           // if (enemy != null && enemy.Faction != Faction.Hero)
           //     enemyClose = true;
        }

        //ShowButtons(enemyClose);

        this.gameObject.GetComponent<SpriteRenderer>().color = hasAttackedColor;
        UnitManager.instance.checkState();
    }
    //bool en true si hay un enemigo cerca
    /*private void ShowButtons(bool isEnemyClose)
    {
        if (isEnemyClose)
        { attackButton.SetActive(true); }

        endButton.SetActive(true);
    }*/

    /*public void HideButtons()
    {
        attackButton.SetActive(false);
        endButton.SetActive(false);
    }*/
}
