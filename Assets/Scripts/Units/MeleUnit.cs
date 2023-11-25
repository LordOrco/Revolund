using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleUnit : BaseUnit
{
    protected override Stack<Tile> CalculatePathToEnemy(BaseUnit enemy)
    {
        Stack<Tile> currentPath;
        if (enemy != null)
        {
            // Usa A* para obtener la ruta más corta
            currentPath = GridManager.instance.a_Star.Repath(GetOccupiedTile(), enemy);
            while (currentPath.Count > 2) currentPath.Pop();
        }
        else
        {
            currentPath = null;
        }
        return currentPath;
    }
}
