using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployTowerTile : Tile
{
    protected override void OnMouseDown()
    {
        //Si no es el turno del jugador no hace nada
        if (GameManager.Instance.State != GameManager.GameState.PlayerTurn) return;
        else
        {
            //node.adyacent_Node
        }
    }
}
