
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System.Linq;
using UnityEditor;

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

        //Creacion de listas din�micamente
        opened_list = new List<Node>();
        closed_list = new List<Node>();
        movement_list = new Stack<Node>();

        //A�adir el nodo de la ficha ocupada
        opened_list.Add(actualTile.node);
        opened_list[0].Path(null, metaTile.GetPosition());

        while (!meta && steps < maxSteps)
        {
            //Debug.Log("Steps: " + steps);

            //Si el nodo es meta, acaba la iteraci�n
            if (opened_list[0].meta == true)
            {
                meta = true;
            }

            //Funci�n expandir
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

            //A�adir a lista cerrada el nodo usado y quitarlo de la abierta
            closed_list.Add(opened_list[0]);
            opened_list.Remove(opened_list[0]);
            
            steps++;

        }
        //Debug.Log(steps);

        if (meta)
        {
            Debug.Log(closed_list.Last().g);
            //Cogemos �ltima posici�n
            //movement_list.Push(closed_list.Last());
            //A�adimos todos los movimientos hasta encontrar la posici�n inicial mediante el padre de cada nodo
           /* while (movement_list.Peek().parent != null)
            {
                //Debug.Log("Parte del camino encontrado");
                movement_list.Push(movement_list.Peek().parent);
            }*/

            //Si est� en rango, bool a true
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


    public void AccesibleTiles(Tile unitTile, int maxG)
    {
        opened_list = new List<Node>();
        closed_list = new List<Node>();

        bool IsEnemyInside;
        

        opened_list.Add(unitTile.node);
        opened_list[0].GNode(null);

        while (opened_list.Count()!= 0 && steps < maxSteps)
        {
            for (int i = 0; i < opened_list[0].adyacent_Nodes.Count; i++)
            {
                if (!closed_list.Contains(opened_list[0].adyacent_Nodes[i]) && opened_list[0].adyacent_Nodes[i].GNode(opened_list[0]) <= maxG)
                {
                    opened_list.Add(opened_list[0].adyacent_Nodes[i]);
                }
            }

            closed_list.Add(opened_list[0]);
            opened_list.Remove(opened_list[0]);
            steps++;
        }

        for (int i = 0;i< closed_list.Count;i++)
        {
            if (closed_list[i].myTile.OccupiedUnit != null && closed_list[i].myTile.OccupiedUnit.Faction == Faction.Enemy) IsEnemyInside = true;
            else IsEnemyInside = false;


            closed_list[i].myTile.ActivateAccesibleHighlight(IsEnemyInside).SetActive(true);          
            GridManager.instance.highlightedTiles.Add(closed_list[i].myTile);
        }


        for (int i = 0; i < closed_list.Count; i++)
        {
            closed_list[i].parent = null;
            closed_list[i].g = 0;
        }

        steps = 0;

    }

    public void CleanAccesibleTiles()
    {
        for (int i = GridManager.instance.highlightedTiles.Count()-1; i >= 0; i--)
        {
            GridManager.instance.highlightedTiles[i].GetAccesibleHighlight().SetActive(false);
            GridManager.instance.highlightedTiles.RemoveAt(i);
        }
        Debug.Log(GridManager.instance.highlightedTiles.Count());
    }
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

        //Selecci�n de siguiente movimiento en base a la ubicaci�n de la siguiente casilla del camino
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