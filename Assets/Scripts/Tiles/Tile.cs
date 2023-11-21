using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase base de todas las tiles
public abstract class Tile : MonoBehaviour
{
    //[SerializeField] private Color basecolor, offsetColor;
    public string TileName;
    [SerializeField] protected SpriteRenderer renderer;
    //Highligth para activar el color al estar encima
    [SerializeField] private GameObject highlight;
    [SerializeField] private bool isWalkable;

    //Unidad que esta en la tile
    public BaseUnit OccupiedUnit;

    //Nodo para el A*
    public Node node;

    //Indica si se puede colocar una casilla
    public bool Walkable => isWalkable && OccupiedUnit == null;


    protected Vector2 position;

    public virtual void Init( Vector2 position)
    {
        this.position = position;
        this.node = new Node(this);


    }
    //Al estar el raton encima brilla e indica la informacion de la tile
    private void OnMouseEnter()
    {
        //Debug.Log(position);
        highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
        GetPosition();
    }
    //Al salir desactiva el highlight y la informacion
    private void OnMouseExit()
    {
        highlight.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null);
    }

    //Al clickar gestiona si hay seleccion de personaje, ataque o movimiento
    private void OnMouseDown()
    {
        //Si no es el turno del jugador no hace nada
        if (GameManager.Instance.State != GameManager.GameState.PlayerTurn) return;
        if(GridManager.instance.a_Star.Repath(this, UnitManager.instance.SelectedHero.OccupiedTile, 50))
        {

        }
        //Si la casilla está ocupada...
        if (OccupiedUnit != null)
        {
            //...y es un heroe lo selecciona
            if (OccupiedUnit.Faction == Faction.Hero) UnitManager.instance.SetSelectedHero((BaseHero)OccupiedUnit);
            //...y es no es un heroe, es un enemigo
            else
            {
                //si hay un heroe selccionado, destruye el enemigo
                if(UnitManager.instance.SelectedHero != null)
                {
                    var enemy = (BaseEnemy)OccupiedUnit;
                    Destroy(enemy.gameObject);
                    UnitManager.instance.SetSelectedHero(null);
                }
            }
        }//Si la casilla no está ocupada...
        else
        {
            //..y hay un heroe seleccionado y se puede andar, mueve el personaje
            if(UnitManager.instance.SelectedHero != null && Walkable)
            {
                //Debug.Log("Nodo meta antes: " + node);
                //Debug.Log("Nodo actual antes: " + UnitManager.instance.SelectedHero.OccupiedTile.node);
                GridManager.instance.a_Star.Repath(this, UnitManager.instance.SelectedHero.OccupiedTile, 50);
                SetUnit(UnitManager.instance.SelectedHero);
                UnitManager.instance.SetSelectedHero(null);
            }
        }
    }
    //Obtiene la posicion
    public Vector2 GetPosition()
    {
        //Debug.Log(position.x + " " + position.y);
        return position;
    }

    //Movimiento del personaje
    public void SetUnit(BaseUnit unit)
    {
        //Si la casilla del heroe esta ocupada, la desocupa
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        //Mueve al personaje a esta tile
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }
}
