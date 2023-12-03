using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployTowerTile : Tile
{

    protected List<Tile> highlightedTiles;
    private bool areAccesibleTilesShown;

    public override void Init(Vector2 position, Faction faction)
    {
        base.Init(position, faction);
        GridManager.instance.DeployTowers.Add(this);
    }
    public bool GetAreAccesibleTilesShown()
    {
        return areAccesibleTilesShown;
    }
    protected override void OnMouseDown()
    {
        //Si no es el turno del jugador no hace nada
        if (GameManager.Instance.State != GameManager.GameState.PlayerTurn) return;
        /*
        if(UnitManager.instance.SelectedHero != null)
        {
            isAtDistance = GridManager.instance.a_Star.IsAtdistance(this, UnitManager.instance.SelectedHero);

            //..y hay un heroe seleccionado, se puede andar y está a distancia, mueve el personaje
            if (UnitManager.instance.SelectedHero != null && Walkable && isAtDistance)
            {
                //Debug.Log("Nodo meta antes: " + node);
                //Debug.Log("Nodo actual antes: " + UnitManager.instance.SelectedHero.OccupiedTile.node);
                //SetUnit(UnitManager.instance.SelectedHero);
                UnitManager.instance.SelectedHero.MoveToTile(this);
                UnitManager.instance.SetSelectedHero(null);
            }
        }*/
        //Activa y desactiva los highlights de las casilla de despliegue
        else if(!areAccesibleTilesShown)
        {
            ActivateDeployTiles();
        }
        else
        {
            DeactivateDeployTiles();
        }
    }

    public void ActivateDeployTiles()
    {
        for (int i = 0; i < node.adyacent_Nodes.Count; i++)
        {
            if (node.adyacent_Nodes[i].myTile is GrassTile accesibleTile)
                accesibleTile.isAccesedByDeployTower = true;
            node.adyacent_Nodes[i].myTile.UpdateTileHighlight();
        }
        areAccesibleTilesShown = true;
    }

    public void DeactivateDeployTiles()
    {
        for (int i = 0; i < node.adyacent_Nodes.Count; i++)
        {
            if (node.adyacent_Nodes[i].myTile is GrassTile accesibleTile)
                accesibleTile.isAccesedByDeployTower = false;
            node.adyacent_Nodes[i].myTile.UpdateTileHighlight();
        }
        areAccesibleTilesShown = false;
    }
}
