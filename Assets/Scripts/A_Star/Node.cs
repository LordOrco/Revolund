using System;
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

    //Metodo para encontrar camino A*
    public void Path(Node pParent, Node pMetaPos)
    {
        
        Vector2 currentPos = myTile.GetPosition();
        if(meta != false){
            /*
            h_star = math.abs((int)(currentPos.x - pMetaPos.x));
            h_star += math.abs((int)(currentPos.y - pMetaPos.y));
            */
            h_star = Manhattan(pMetaPos);
        }

        GNode(pParent);

        f_star = h_star + g;
    }
   
    //Metodo que devuelve la g del nodo
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

    public int Manhattan(Node pMetaPos)
    {
        Vector2 currentPos = myTile.GetPosition();
        int manhattan;
        
        manhattan = math.abs((int)(currentPos.x - pMetaPos.myTile.GetPosition().x));
        manhattan += math.abs((int)(currentPos.y - pMetaPos.myTile.GetPosition().y));
        
        return manhattan;
    }

    //Setter de adyacentNodes
    public void SetAdyacentNodes(List<Node> nodes)
    {
        this.adyacent_Nodes = nodes;
        //Debug.Log(adyacent_Nodes.Count);
    }

    internal int Manhattan()
    {
        throw new NotImplementedException();
    }
}


