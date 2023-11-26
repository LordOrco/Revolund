
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System.Linq;
using UnityEditor;
using static UnityEngine.UI.CanvasScaler;
using static Unity.Burst.Intrinsics.X86;

public class A_star: MonoBehaviour
{
    //Pasos
    private int steps = 0;
    public int maxSteps = 3000;

    //Metodo que devuelve el camino a la tile, solo la parte dentro del g de la unidad
    public Stack<Tile> Repath(Tile metaTile, BaseUnit unit, bool isG)
    {
        List<Node> metaNodes = new List<Node>();
        bool meta = false;
        Tile actualTile = unit.GetOccupiedTile();
        metaTile.node.meta = true;
        //Establecer el meta del nodo a true
        //metaNodes.Add(metaTile.node);
        Tile minTile = actualTile;
        /*
        for(int i = 0; i < metaTile.node.adyacent_Nodes.Count; i++)
        {
            if(metaTile.node.adyacent_Nodes[i].myTile.OccupiedUnit == null)
            {
                metaNodes.Add(metaTile.node.adyacent_Nodes[i]);
            }
        }
        
        metaNodes.Sort((x, y) => x.Manhattan(unit.GetOccupiedTile().node).CompareTo(y.Manhattan(unit.GetOccupiedTile().node)));
        metaNodes[0].meta = true;*/

        //Creacion de listas dinámicamente
        List<Node> opened_list = new List<Node>();
        List<Node> closed_list = new List<Node>();
        Stack<Node> movement_list = new Stack<Node>();
        Stack<Tile> path = new Stack<Tile>();

        //Añadir el nodo de la ficha ocupada
        opened_list.Add(actualTile.node);
        opened_list[0].Path(null, metaTile.node);
        int minh_star = opened_list[0].h_star;
        Debug.Log(opened_list[0].h_star);

        int actualNodeh;
        while (!meta && steps < maxSteps && opened_list.Count > 0)
        {
            //Debug.Log("Steps: " + steps);
            //Si el nodo es meta, acaba la iteración
            if (opened_list[0].h_star == 1 || opened_list[0].h_star == 0)
            {
                Debug.Log("MEEEEEEEEEEEEEEEETA");
                meta = true;
            }

            //Función expandir
            for (int i = 0; i < opened_list[0].adyacent_Nodes.Count; i++)
            {
                actualNodeh = (opened_list[0].adyacent_Nodes[i].Manhattan(metaTile.node));

                if (!(actualNodeh == 1 && opened_list[0].adyacent_Nodes[i].myTile.OccupiedUnit != null))
                {
                    if (!closed_list.Contains(opened_list[0].adyacent_Nodes[i])
                        && opened_list[0].adyacent_Nodes[i].Manhattan(metaTile.node) < opened_list[0].h_star + 2)
                    {
                        if (minh_star > actualNodeh)
                            minTile = opened_list[0].adyacent_Nodes[i].myTile;

                        opened_list.Add(opened_list[0].adyacent_Nodes[i]);
                        opened_list[0].adyacent_Nodes[i].Path(opened_list[0], metaTile.node);
                        //Debug.Log(opened_list[0].adyacent_Nodes[i].parent.myTile);
                    }
                }
            }

            //Ordenar la lista abierta
            opened_list.Sort((x, y) => x.f_star.CompareTo(y.f_star));

            //Añadir a lista cerrada el nodo usado y quitarlo de la abierta
            closed_list.Add(opened_list[0]);
            opened_list.Remove(opened_list[0]);
            
            steps++;

        }
        //Debug.Log(steps);
        if (meta)
        {

            movement_list.Push(closed_list.Last());
        }
        else
        {
            Debug.Log("No se puede llegar a la meta");
            //EditorApplication.isPlaying = false;
            movement_list.Push(minTile.node);
        }

        int step2 = 0;
        //Añadimos todos los movimientos hasta encontrar la posición inicial mediante el padre de cada nodo
        while (movement_list.Peek().parent != null && step2 < 10)
        {
            //Debug.Log(closed_list.Last().myTile);
            movement_list.Push(movement_list.Peek().parent);
            step2++;
        }
        if (isG)
        {
            int maxG = unit.maxG;
            for (int i = 0; i < movement_list.Count; i++)
            {
                //Debug.Log(movement_list.ElementAt(i).myTile);
                if (movement_list.ElementAt(i).g <= maxG)
                {
                    path.Push(movement_list.ElementAt(i).myTile);
                }

            }
        }
        else
        {
            for (int i = 0; i < movement_list.Count; i++)
            {
                path.Push(movement_list.ElementAt(i).myTile);
                //Debug.Log(movement_list.ElementAt(i).myTile);

            }
        }

        CleanAStarRepath(metaTile, closed_list);
        //Debug.Log("Movement List: " + path.Count);
        return path;

    }
    //Limpia listas, establece valores a predeterminados.
    private void CleanAStarRepath(Tile metaTile, List<Node> closed_list)
    {
        for (int i = 0; i < closed_list.Count; i++)
        {
            closed_list[i].parent = null;
            closed_list[i].g = 0;
        }
        metaTile.node.meta = false;
        steps = 0;
    }

    public bool IsAtdistance(Tile tile,BaseUnit unit)
    {
        if (unit.GetOccupiedTile().node.Manhattan(tile.node) <= unit.maxG)
            return true;
        else return false;
    }
    //Metodo que devuelve las tiles a las que el heroe seleccionado puede acceder
    public List<Tile> ObtainAccesibleTiles(BaseUnit unit)
    {
        //Creacion de listas dinámicamente
        List<Node> opened_list = new List<Node>();
        List<Node> closed_list = new List<Node>();
        List<Tile> result = new List<Tile>();

        Faction faction = unit.Faction;
        Tile tile = unit.GetOccupiedTile();
        int maxG = unit.maxG;
        
        //Añade la tile del heroe seleccionado a la lista abierta y
        //establece la g a 0
        opened_list.Add(tile.node);
        opened_list[0].GNode(null);

        //Mientras la lista abierta tenga nodos y no se hayan alcanzado los maxSteps
        while (opened_list.Count()!= 0 && steps < maxSteps)
        {
            if((opened_list[0].myTile.OccupiedUnit != null && opened_list[0].myTile.OccupiedUnit.Faction == faction )
                || opened_list[0].myTile.OccupiedUnit == null)
            {
                //Añade a la lista abierta los nodos existentes cuyo g no se mayor a maxG
                for (int i = 0; i < opened_list[0].adyacent_Nodes.Count; i++)
                {
                    if (!(closed_list.Contains(opened_list[0].adyacent_Nodes[i]))
                        && opened_list[0].adyacent_Nodes[i].GNode(opened_list[0]) <= maxG)
                    {
                        opened_list.Add(opened_list[0].adyacent_Nodes[i]);
                    }
                }
            }

            //Añadir a la lista cerrada el nodo y quitarlo de la abierta
            closed_list.Add(opened_list[0]);
            opened_list.Remove(opened_list[0]);
            steps++;
        }
        //Debug.Log("ClosedList " + closed_list.Count());
        //Limpieza de los nodos y steps, convertir la lista tiles
        for (int i = 0; i < closed_list.Count; i++)
        {
            result.Add(closed_list[i].myTile);
            closed_list[i].parent = null;
            closed_list[i].g = 0;
        }
        steps = 0;
        return result;

    }
    //Futura implementacion(si eso) de movimiento de unidades a traves de los tiles
    /*
    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        if (movement_list.Count == 0)
        {
            //Debug.Log(goals[0].CellId);
            Repath2(boardInfo, currentPos, goals);
        }

        CellInfo nextPos;
        if (movement_list.Count != 0)
        {
            nextPos = movement_list.Pop().cell;
        }
        else
        {
            nextPos = currentPos;
        }

        //Debug.Log(movement_list.Count);

        //for(int i = 0; i < movement_list.Count; i++)
        //{
        //    Debug.Log(movement_list.ElementAt(i).cell.RowId + ", " + movement_list.ElementAt(i).cell.ColumnId);
        //    Debug.Log(" ");
        //}

        //Selección de siguiente movimiento en base a la ubicación de la siguiente casilla del camino
        if (nextPos != currentPos)
        {
            if (nextPos.RowId > currentPos.RowId)
            {
                return Locomotion.MoveDirection.Up;
            }
            else if(nextPos.RowId < currentPos.RowId)
            {
                return Locomotion.MoveDirection.Down;
            }

            else if (nextPos.ColumnId > currentPos.ColumnId)
            {
                return Locomotion.MoveDirection.Right;
            }
            else
            {
                return Locomotion.MoveDirection.Left;
            }
        }
        else
        {
            return Locomotion.MoveDirection.None;
        }

        //var val = Random.Range(0, 4);
        //if (val == 0) return Locomotion.MoveDirection.Up;
        //if (val == 1) return Locomotion.MoveDirection.Down;
        //if (val == 2) return Locomotion.MoveDirection.Left;
        //return Locomotion.MoveDirection.Right;
    
    }*/
}
