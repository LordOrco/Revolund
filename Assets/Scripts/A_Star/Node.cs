using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Node
{
    public int f_star;
    public int g;
    public int h_star;
    public bool meta;

    public Tile myTile;

    public Node parent;
    public List<Node> adyacent_Nodes;

    public Node(Tile tile)
    {
        meta = false;
        myTile = tile;
        
    }

    public void Path(Node pParent, Vector2 pMetaPos)
    {
        Vector2 currentPos = myTile.GetPosition();
        if(meta != false){
            h_star = math.abs((int)(currentPos.x - pMetaPos.x));
            h_star += math.abs((int)(currentPos.y - pMetaPos.y));
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
   
    public int GNode(Node pParent)
    {
        if (pParent != null)
        {
            parent = pParent;
            g = parent.g + 1;
        }
        else
        {
            g = 0;
        }
        return g;
    }
    public void SetAdyacentNodes(List<Node> nodes)
    {
        this.adyacent_Nodes = nodes;
        //Debug.Log(adyacent_Nodes.Count);
    }
}


