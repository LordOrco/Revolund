using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GrassTile : Tile
{
    //Colores de AccesibleHighlight
    [SerializeField] private Color enemyPathingColor, heroPathingColor, heroAndEnemyPathingColor, deployUnitColor, attackableEenemyColor;

    //Bool si esta resaltado por Deploy Tower
    public bool isAccesedByDeployTower;
    public override void Init(Vector2 position, Faction pFaction)
    {
        //Cambia de color si es par o impar
        //var isOffset = (position.x + position.y) % 2 == 1;
        //renderer.color = isOffset ? offsetColor : basecolor;

        this.position = position;
        faction = pFaction;
        node = new Node(this);
        enemiesPathing = 0;
        heroesPathing = 0;
        isAccesedByDeployTower = false;

        if (faction == Faction.Hero)
        {
            renderer.sprite = SteampunkTheme;
        }
        else
        {
            renderer.sprite = CyberpunkTheme;
        }
    }


    protected override void OnMouseDown()
    {
        //Si no es el turno del jugador no hace nada
        if (GameManager.Instance.State != GameManager.GameState.PlayerTurn) return;

        //Si no se puede andar a ella, no activa el movimiento
        if (!isWalkable) Debug.Log("No se puede andar");

        else
        {
            //Booleano para indicar si esta casilla esta dentro de la distancia de la unidad
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
                        isAtDistance = GridManager.instance.a_Star.IsAtdistance(this, UnitManager.instance.SelectedHero);
                        //si hay un heroe seleccionado y a rango, destruye el enemigo
                        if (isAtDistance)
                        {
                            //OccupiedUnit.Kill();
                            UnitManager.instance.SelectedHero.Attack(OccupiedUnit);
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
                        if(OccupiedUnit.GetAreAccesibleTilesShown()) OccupiedUnit.HidePathingTiles();
                        else OccupiedUnit.ShowPathingTiles();
                        if(FindAnyObjectByType<BaseHero>()!= null)
                            OccupiedUnit.Attack(FindAnyObjectByType<BaseHero>());
                    }

                }
                
                //Si la casilla no está ocupada y tienes un heroe seleccionado...
                else if (UnitManager.instance.SelectedHero != null)
                {
                    isAtDistance = GridManager.instance.a_Star.IsAtdistance(this, UnitManager.instance.SelectedHero);

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

    //Funcion que instancia unidades si es un tile accesible por una Deploy Tower
    private void OnMouseOver()
    {
        if (isAccesedByDeployTower && Input.GetMouseButtonDown(1) )
        {
            UnitManager.instance.SpawnHeroOnDemand(this);
        }
    }

    //Metodo que establece colores de los highlights y devuelve dicha tile
    public override void UpdateTileHighlight()
    {
        //Si el raton esta encima, highlight amarailloo, si no lo desactiva
        if (isMouseIn) highlight.SetActive(true);
        else highlight.SetActive(false);

        //Si es accesib
        if (isAccesedByDeployTower)
        {
            accesibleTilehighlight.SetActive(true);
            accesibleTilehighlight.GetComponent<SpriteRenderer>().color = deployUnitColor;
        }
        else if (heroesPathing > 0 && (OccupiedUnit != null && OccupiedUnit.Faction == Faction.Enemy))
        {
            accesibleTilehighlight.SetActive(true);
            accesibleTilehighlight.GetComponent<SpriteRenderer>().color = attackableEenemyColor;
        }
        else
        {
            //Si hay Enemigos o Heroes que necesitan el highlight, lo activa, si no lo desactiva
            if (enemiesPathing > 0 || heroesPathing > 0) accesibleTilehighlight.SetActive(true);
            else accesibleTilehighlight.SetActive(false);

            //Si ambos bandos buscan, activa el morado
            if (enemiesPathing > 0 && heroesPathing > 0)
                accesibleTilehighlight.GetComponent<SpriteRenderer>().color = heroAndEnemyPathingColor;

            //Si solo hay enemigo buscando, activa el rojo
            else if (enemiesPathing > 0 && heroesPathing <= 0)
                accesibleTilehighlight.GetComponent<SpriteRenderer>().color = enemyPathingColor;

            //Si solo hay heroe buscando, activa el rojo
            else accesibleTilehighlight.GetComponent<SpriteRenderer>().color = heroPathingColor;
        }
        //Debug.Log("Enemigos = " + enemiesPathing + " Heroes = " + heroesPathing);
    }
}
