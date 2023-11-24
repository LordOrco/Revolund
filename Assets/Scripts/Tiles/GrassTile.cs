using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GrassTile : Tile
{
    [SerializeField] private Color enemyPathingColor, heroPathingColor, heroAndEnemyPathingColor;
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


    protected override void OnMouseDown()
    {
        //Si no es el turno del jugador no hace nada
        if (GameManager.Instance.State != GameManager.GameState.PlayerTurn) return;
        if (!isWalkable) Debug.Log("No se puede andar");
        else
        {
            bool isAtDistance;
            {
                //Si la casilla está ocupada...
                if (OccupiedUnit != null)
                {
                    //...por un heroe...
                    if (OccupiedUnit.Faction == Faction.Hero)
                    {
                        //...y es el seleccionado, lo deselecciona
                        if (UnitManager.instance.SelectedHero != null && OccupiedUnit == UnitManager.instance.SelectedHero)
                        {
                            UnitManager.instance.SetSelectedHero(null);
                            
                        }
                        //y no es el seleccionado, lo selecciona
                        else
                        {
                            UnitManager.instance.SetSelectedHero((BaseHero) OccupiedUnit);
                            
                        }
                    }
                    //...y es un enemigo y tengo seleccionado un heroe
                    else if (UnitManager.instance.SelectedHero != null)
                    {
                        Debug.Log("Enemigo");
                        isAtDistance = GridManager.instance.a_Star.Repath(this, UnitManager.instance.SelectedHero.GetOccupiedTile(),
                            UnitManager.instance.SelectedHero.maxG);
                        //si hay un heroe seleccionado y a rango, destruye el enemigo
                        if (isAtDistance)
                        {
                            var enemy = (BaseEnemy)OccupiedUnit;
                            Destroy(enemy.gameObject);
                            UnitManager.instance.SetSelectedHero(null);
                        }
                        else
                        {
                            Debug.Log("Enemigo fuera de rango");
                        }
                    }
                    //...y es un enemigo y no tengo seleccionado un heroe
                    else
                    {
                        OccupiedUnit.ShowPathingTiles();
                    }
                }//Si la casilla no está ocupada y tienes un heroe seleccionado...
                else if (UnitManager.instance.SelectedHero != null)
                {
                    isAtDistance = GridManager.instance.a_Star.Repath(this, UnitManager.instance.SelectedHero.GetOccupiedTile(), 
                        UnitManager.instance.SelectedHero.maxG);
                    //..y hay un heroe seleccionado, se puede andar y está a distancia, mueve el personaje
                    if (UnitManager.instance.SelectedHero != null && Walkable && isAtDistance)
                    {
                        //Debug.Log("Nodo meta antes: " + node);
                        //Debug.Log("Nodo actual antes: " + UnitManager.instance.SelectedHero.OccupiedTile.node);
                        SetUnit(UnitManager.instance.SelectedHero);
                        UnitManager.instance.SetSelectedHero(null);
                    }
                }
                //Si la casilla no está ocupada y no tienes un heroe seleccionado...
                else
                {
                    Debug.Log("Selecciona un heroe");
                }
            }
        }
    
    }
    //Metodo que establece colores de los highlights y devuelve dicha tile
    public override void UpdateTileHighlight()
    {
        if (isMouseIn) highlight.SetActive(true);
        else highlight.SetActive(false);


        if (enemiesPathing > 0 || heroesPathing > 0) accesibleTilehighlight.SetActive(true);
        else accesibleTilehighlight.SetActive(false);

        if (enemiesPathing > 0 && heroesPathing > 0)
            accesibleTilehighlight.GetComponent<SpriteRenderer>().color = heroAndEnemyPathingColor;
        else if(enemiesPathing > 0 && heroesPathing <= 0)
            accesibleTilehighlight.GetComponent<SpriteRenderer>().color = enemyPathingColor;
        else accesibleTilehighlight.GetComponent<SpriteRenderer>().color = heroPathingColor;
        //Debug.Log("Enemigos = " + enemiesPathing + " Heroes = " + heroesPathing);
    }
}
