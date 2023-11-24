using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployTowerTile : Tile
{

    protected List<Tile> highlightedTiles;
    private bool areAccesibleTilesShown;

    public bool GetAreAccesibleTilesShown()
    {
        return areAccesibleTilesShown;
    }
    protected override void OnMouseDown()
    {
        //Si no es el turno del jugador no hace nada
        if (GameManager.Instance.State != GameManager.GameState.PlayerTurn) return;

        //Activa y desactiva los highlights de las casilla de despliegue
        if (!areAccesibleTilesShown)
        {
            for(int i = 0; i < node.adyacent_Nodes.Count; i++)
            {
                if(node.adyacent_Nodes[i].myTile is GrassTile accesibleTile)
                    accesibleTile.isAccesedByDeployTower = true;
                node.adyacent_Nodes[i].myTile.UpdateTileHighlight();
            }
            areAccesibleTilesShown = true;
        }
        else
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
}
