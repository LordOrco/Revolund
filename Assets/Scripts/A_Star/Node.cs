using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int f_star;
    public int g;
    public int h_star;
    public bool meta;

    public Vector2 currentPos;
    public Vector2 metaPos;

    public Node parent;
    public List<Node> adyacent_Nodes = new List<Node>();


    public Node(Node pParent, Vector2 TilePos, Vector2 pMetaPos)
    {
        this.currentPos = TilePos;
        metaPos = pMetaPos;

        //Debug.Log(currentPos.RowId);

        //Calculo de la distancia de Manhattan.
        h_star = math.abs((int)(currentPos.x - metaPos.x));
        h_star += math.abs((int)(currentPos.y - metaPos.y));
        if (h_star == 0)
        {
            meta = true;
        }

        if (pParent != null)
        {
            parent = pParent;
            g = parent.g + 1;
        }
        else
        {
            g = 0;
        }

        f_star = h_star + g;
    }
    /*
    public void findAdyacentNodes()
    {
        CellInfo[] neighbours = cell.WalkableNeighbours(board);
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i] != null)
            {
                Node neighbour = new Node(this, neighbours[i], metaPos, board);
                adyacent_Nodes.Add(neighbour);
            }
        }
    }
    
}
*/

}
