using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase base de todas las tiles
public abstract class Tile : MonoBehaviour
{
    //[SerializeField] private Color basecolor, offsetColor;
    public string TileName;
    [SerializeField] protected SpriteRenderer renderer;    
    [SerializeField] protected GameObject accesibleTilehighlight;
    [SerializeField] protected bool isWalkable;
    [SerializeField] protected bool needAdyacentNodes;

    [SerializeField] protected Sprite CyberpunkTheme;
    [SerializeField] protected Sprite SteampunkTheme;

    //Highligth para activar el color al estar encima
    [SerializeField] protected GameObject highlight;
    [SerializeField] protected Color basecolor;
    //[SerializeField] protected Color enemyInsideColor;
    //[SerializeField] protected Color noEnemyInsideColor;

    public int enemiesPathing;
    public int heroesPathing;
    protected bool isMouseIn;

    //Facción a la que pertenece
    public Faction faction;

    //Unidad que esta en la tile
    public BaseUnit OccupiedUnit;

    //Nodo para el A*
    public Node node;

    //Indica si se puede colocar una casilla
    public bool Walkable => isWalkable && OccupiedUnit == null;


    protected Vector2 position;

    public GameObject GetAccesibleHighlight()
    {
        return accesibleTilehighlight;
    }

    public bool GetNeedsAdyacentNodes()
    {
        return needAdyacentNodes;
    }

    public Vector2 GetPosition()
    {
        //Debug.Log(position.x + " " + position.y);
        return position;
    }

    public virtual void Init( Vector2 position, Faction faction)
    {
        this.position = position;
        this.faction = faction;
        node = new Node(this);
        
        if(this.faction == Faction.Hero)
        {
            this.renderer.sprite = SteampunkTheme;
        }
        else
        {
            this.renderer.sprite = CyberpunkTheme;
        }

    }
    //Al estar el raton encima brilla e indica la informacion de la tile
    protected virtual void OnMouseEnter()
    {
        //Debug.Log(position);
        isMouseIn = true;
        UpdateTileHighlight();
        MenuManager.Instance.ShowTileInfo(this);
        GetPosition();
    }
    //Al salir desactiva el highlight y la informacion
    protected virtual void OnMouseExit()
    {
        isMouseIn = false;
        UpdateTileHighlight();
        MenuManager.Instance.ShowTileInfo(null);
    }

    //Al clickar gestiona si hay seleccion de personaje, ataque o movimiento
    protected virtual void OnMouseDown()
    {
        //Si no es el turno del jugador no hace nada
        if (GameManager.Instance.State != GameManager.GameState.PlayerTurn) return;
        Debug.Log(this);
    }

    public virtual void UpdateTileHighlight()
    {
        if(isMouseIn) highlight.SetActive(true);
        else highlight.SetActive(false);
    }

    //Movimiento del personaje
    public void SetUnit(BaseUnit unit)
    {
        //Si la casilla del heroe esta ocupada, la desocupa

        if (unit.GetOccupiedTile() != null && unit.GetOccupiedTile().OccupiedUnit != null) unit.GetOccupiedTile().OccupiedUnit = null;
        //Mueve al personaje a esta tile
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.SetOccupiedTile(this);
    }

    //Metodo que establece colores de los highlights y devuelve dicha tile
    /*public GameObject ActivateAccesibleHighlight(bool IsEnemyInside)
    {
        if(IsEnemyInside)
        {
            accesibleTileshighlight.GetComponent<SpriteRenderer>().color = enemyInsideColor;
        }
        else
        {
            accesibleTileshighlight.GetComponent<SpriteRenderer>().color = noEnemyInsideColor;
        }
        return this.accesibleTileshighlight;
    }*/

    
}
