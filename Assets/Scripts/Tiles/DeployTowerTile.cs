using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployTowerTile : Tile
{
    [SerializeField] private Color enemyColor, heroColor;
    protected List<Tile> highlightedTiles;
    [HideInInspector]public bool areAccesibleTilesShown;

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

            //..y hay un heroe seleccionado, se puede andar y est� a distancia, mueve el personaje
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
            // if (node.adyacent_Nodes[i].myTile is GrassTile accesibleTile)
            //accesibleTile.isAccesedByDeployTower = true;
            areAccesibleTilesShown = true;
            //Debug.Log("Deployyy");
            node.adyacent_Nodes[i].myTile.UpdateTileHighlight();
        }
    }

    public void DeactivateDeployTiles()
    {
        for (int i = 0; i < node.adyacent_Nodes.Count; i++)
        {
            //if (node.adyacent_Nodes[i].myTile is GrassTile accesibleTile)
            //accesibleTile.isAccesedByDeployTower = false;
            areAccesibleTilesShown = false;
            node.adyacent_Nodes[i].myTile.UpdateTileHighlight();
        }
    }

    public void UpdateTowerFaction()
    {
        Faction Newfaction = Faction.None;
        bool areSeveral = false;
        for(int i = 0; i < node.adyacent_Nodes.Count; i++)
        {
            if (areSeveral != true && node.adyacent_Nodes[i].myTile.OccupiedUnit != null)
            {
                Newfaction = node.adyacent_Nodes[i].myTile.OccupiedUnit.Faction;
                areSeveral = true;
            }
            else if (node.adyacent_Nodes[i].myTile.OccupiedUnit != null 
                && node.adyacent_Nodes[i].myTile.OccupiedUnit.Faction != Newfaction)
            {
                Newfaction = Faction.None;
            }
        }
        if (Newfaction != Faction.None)
        {
            UpdateFaction(Newfaction);
        }
    }
    public Color GetHLcolor()
    {
        if (faction == Faction.Hero)
            return heroColor;
        else
            return enemyColor;
    }
}
