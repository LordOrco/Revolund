using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseUnit
{
    public bool areAccesibleTilesShown = false;

    //Tiles asociados cuyos highlights estan asociados
    public List<Tile> highlightedTiles;
}
