
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System.Linq;
using UnityEditor;
using static UnityEngine.UI.CanvasScaler;

public class A_star: MonoBehaviour
{
    //Lsitas para implementar A*
    private List<Node> opened_list;
    private List<Node> closed_list;
    private Stack<Node> movement_list;

    //Bool que indica si se ha alcanzado al meta
    private bool meta = false;

    //Pasos
    private int steps = 0;
    public int maxSteps = 500;

    public bool Repath(Tile metaTile, Tile actualTile,int maxG)
    {
        //Booleano para devolver true si se puede llegar y false si no.
        //False de primeras por comodidad
        bool canWalk = false;

        //Establecer el meta del nodo a true
        metaTile.node.meta = true;

        //Creacion de listas dinámicamente
        opened_list = new List<Node>();
        closed_list = new List<Node>();
        movement_list = new Stack<Node>();

        //Añadir el nodo de la ficha ocupada
        opened_list.Add(actualTile.node);
        opened_list[0].Path(null, metaTile.GetPosition());

        while (!meta && steps < maxSteps)
        {
            //Debug.Log("Steps: " + steps);

            //Si el nodo es meta, acaba la iteración
            if (opened_list[0].meta == true)
            {
                meta = true;
            }

            //Función expandir
            for (int i = 0; i < opened_list[0].adyacent_Nodes.Count; i++)
            {
                if (!closed_list.Contains(opened_list[0].adyacent_Nodes[i]))
                {
                    opened_list[0].adyacent_Nodes[i].Path(opened_list[0], metaTile.GetPosition());
                    opened_list.Add(opened_list[0].adyacent_Nodes[i]);
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
            Debug.Log(closed_list.Last().g);
            //Cogemos última posición
            //movement_list.Push(closed_list.Last());
            //Añadimos todos los movimientos hasta encontrar la posición inicial mediante el padre de cada nodo
           /* while (movement_list.Peek().parent != null)
            {
                //Debug.Log("Parte del camino encontrado");
                movement_list.Push(movement_list.Peek().parent);
            }*/

            //Si está en rango, bool a true
            if(closed_list.Last().g <= maxG) canWalk = true;
        }
        else
        {
            Debug.Log("No se puede llegar a la meta");
            //EditorApplication.isPlaying = false;
        }

        CleanAStarRepath(metaTile);
        Debug.Log(canWalk);
        return canWalk;


    }
    //Limpia listas, establece valores a predeterminados.
    private void CleanAStarRepath(Tile metaTile)
    {
        for (int i = 0; i < closed_list.Count; i++)
        {
            closed_list[i].parent = null;
            closed_list[i].g = 0;
        }
        metaTile.node.meta = false;
        meta = false;
        steps = 0;
    }

    //Metodo que crea un highlight sobre las tiles a las que el heroe seleccionado puede acceder
    public List<Tile> ObtainAccesibleTiles(BaseUnit unit)
    {
        //Creacion de listas dinámicamente
        opened_list = new List<Node>();
        closed_list = new List<Node>();
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
        Debug.Log("ClosedList " + closed_list.Count());
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
