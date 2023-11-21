
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System.Linq;
using UnityEditor;

public class A_star: MonoBehaviour
{
    private List<Node> opened_list;
    private List<Node> closed_list;
    private Stack<Node> movement_list;


    private bool meta = false;
    private int steps = 0;
    public int maxSteps = 500;

    public bool Repath(Tile metaTile, Tile actualTile,int maxG)
    {
        bool canWalk;
        metaTile.node.meta = true;


        opened_list = new List<Node>();
        closed_list = new List<Node>();
        movement_list = new Stack<Node>();

        opened_list.Add(actualTile.node);
        opened_list[0].Path(null, metaTile.GetPosition());

        while (!meta && steps < maxSteps)
        {
            //Debug.Log("Steps: " + steps);
            //Si el nodo es meta
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


            closed_list.Add(opened_list[0]);
            opened_list.Remove(opened_list[0]);
            
            steps++;

        }
        //Debug.Log(steps);

        if (meta)
        {
            Debug.Log(closed_list.Last().g);
            //Cogemos �ltima posici�n
            movement_list.Push(closed_list.Last());
            //A�adimos todos los movimientos hasta encontrar la posici�n inicial mediante el padre de cada nodo
            while (movement_list.Peek().parent != null)
            {
                //Debug.Log("Parte del camino encontrado");
                movement_list.Push(movement_list.Peek().parent);
            }
            canWalk = true;
            meta = false;
        }
        else
        {
            Debug.Log("No se puede llegar a la meta");
            canWalk = false;
            //EditorApplication.isPlaying = false;
        }
        for(int i = 0;i < closed_list.Count;i++) closed_list[i].parent = null;
        metaTile.node.meta = false;
        steps = 0;
        Debug.Log(canWalk);
        return canWalk;


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
