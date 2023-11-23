using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : Tile
{
    [SerializeField] private Color enemyInsideColor, noEnemyInsideColor, heroAndEnemyPathing;
    public override void Init(Vector2 position)
    {
        //Cambia de color si es par o impar
        //var isOffset = (position.x + position.y) % 2 == 1;
        //renderer.color = isOffset ? offsetColor : basecolor;

        this.position = position;
        node = new Node(this);
        enemiesPathing = 0;
        heroesPathing = 0;
    }
    
    //Metodo que establece colores de los highlights y devuelve dicha tile
    public GameObject ActivateAccesibleHighlight(int acces)
    {

        switch (acces)
        {
            case 0:
                accesibleTileshighlight.GetComponent<SpriteRenderer>().color = noEnemyInsideColor;
                break;
            case 1:
                accesibleTileshighlight.GetComponent<SpriteRenderer>().color = enemyInsideColor;
                break;
            case 2:
                accesibleTileshighlight.GetComponent<SpriteRenderer>().color = heroAndEnemyPathing;
                break;
        }
        return accesibleTileshighlight;
    }
}
