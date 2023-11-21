using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : Tile
{
    [SerializeField] private Color basecolor, offsetColor;

    public override void Init(Vector2 position)
    {
        var isOffset = (position.x + position.y) % 2 == 1;
        renderer.color = isOffset ? offsetColor : basecolor;
        this.position = position;
        node = new Node(this);
    }
}
