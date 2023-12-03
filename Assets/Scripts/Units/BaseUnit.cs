using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class BaseUnit : MonoBehaviour
{
    public string unitName;
    public Faction Faction;
    public int maxG;
    public int range;

    [SerializeField] public Color hasAttackedColor, hasDontAttackedColor;
    protected bool attacking = false;

    //Atributos para movimiento
    protected int movementSpeed;

    //Unidad a la que ataca, null si no hay ninguna
    protected BaseUnit attackedEnemy;

    public int HP, minAttack, maxAttack;

    //Tiles asociados cuyos highlights estan asociados
    protected List<Tile> highlightedTiles;
    private bool areAccesibleTilesShown;
    private Tile OccupiedTile;

    [SerializeField] public Slider barraVida;

    [HideInInspector]public bool canAttack;
    [HideInInspector]public bool canMove;
    public List<Tile> GetHighlightedTiles() { return highlightedTiles; }
    public void SetHighlightedTiles(List<Tile> tiles) { this.highlightedTiles = tiles; }
    public bool GetAreAccesibleTilesShown() { return areAccesibleTilesShown; }
    public void SetAreAccesibleTilesShown(bool state) { areAccesibleTilesShown = state; }
    public Tile GetOccupiedTile() { return OccupiedTile; }
    public void SetOccupiedTile(Tile tile) { this.OccupiedTile = tile; }

    protected virtual void Awake()
    {
        OccupiedTile = null;
        areAccesibleTilesShown = false;
        highlightedTiles = new List<Tile>();
        attackedEnemy = null;
        barraVida.maxValue = HP;
        barraVida.value = HP;
        barraVida.minValue = 0;
        canAttack = true;
        canMove = true;
        if (Faction == Faction.Hero) UnitManager.instance.heroList.Add(this);
        else UnitManager.instance.enemyList.Add((BaseEnemy)this);
    }
    protected virtual void Update()
    { /*
        
        if (attackedEnemy != null)
        {
            if (currentPath != null && currentPath.Count > 0)
            {
                // Mueve el enemigo hacia la siguiente casilla en la ruta
                MoveToTile(currentPath.Peek());

                // Verifica si ha llegado a la casilla actual en la ruta
                if (transform.position == currentPath.Peek().transform.position)
                {
                    currentPath.Pop();
                    
                    // Si ha llegado al final de la ruta, recalcula la ruta hacia el héroe
                    if (currentPathIndex >= currentPath.Count)
                    {
                        CalculatePathToHero();
                    }
                }
            }
            else
            {
                ApplyDmg(attackedEnemy);
                attackedEnemy = null;
            }
        }
        */
    }

    public virtual void ShowPathingTiles() {; }

    public virtual void HidePathingTiles() {; }

    protected virtual void Kill()
    {
        HidePathingTiles();
        Destroy(gameObject);
        if (Faction == Faction.Hero)
        {
            UnitManager.instance.heroList.Remove(this);
            UnitManager.instance.heroes--;
        }
        else UnitManager.instance.enemyList.Remove(this);
    }

    public virtual void Attack(BaseUnit enemy)
    {
        if (!attacking && canAttack)
        {
            attacking = true;
            //Debug.Log("TargetTile pasado");
            Stack<Tile> currentPath = CalculatePathToTile(enemy.OccupiedTile);
            //Debug.Log("Target Tile: " + enemy.OccupiedTile);
            if (currentPath != null && canMove)
            {
                MoveToTile(currentPath.Peek());
                ApplyDmg(enemy);
            }
            attacking = false;
            canAttack = false;
            this.gameObject.GetComponent<SpriteRenderer>().color = hasAttackedColor;
            if (Faction == Faction.Hero) UnitManager.instance.heroesAttacked++;
            UnitManager.instance.checkState();
        }
    }

    public virtual void ReceiveDmg(int dmg)
    {
        HP -= dmg;

        barraVida.value = HP;
        if (HP <= 0)
        {
            Kill();
        }
        UnitManager.instance.checkState();
    }

    protected virtual void ApplyDmg(BaseUnit enemy)
    {
        int dmg = UnityEngine.Random.Range(minAttack, maxAttack);
        enemy.ReceiveDmg(dmg);
    }

    public virtual void CalculAndMovement(Tile targetTile)
    {
        Stack<Tile> path = CalculatePathToTile(targetTile);
        MoveToTile(path.Peek());
    }
    public virtual void MoveToTile(Tile targetTile)
    {
        /*
        // Obtiene la posición actual del enemigo y la posición objetivo
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = targetTile.transform.position;

        // Calcula la nueva posición usando Vector3.MoveTowards
        Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, movementSpeed * Time.deltaTime);
        */
        // Mueve al enemigo a la nueva posición
        if(canMove)
        {
            if (targetTile != null)
                targetTile.SetUnit(this);
            else
                Debug.Log("MoveToTile no hay camino");
            ///currentPath = null;

            canMove = false;
            if(Faction == Faction.Hero) { UnitManager.instance.heroesAttacked++; }
            this.gameObject.GetComponent<SpriteRenderer>().color = hasAttackedColor;
            UnitManager.instance.checkState();
            if (Faction == Faction.Hero) UnitManager.instance.heroesAttacked++;
        }
    }

    protected virtual Stack<Tile> CalculatePathToEnemy(BaseUnit enemy)
    {
        if (enemy != null)
        {
            // Usa A* para obtener la ruta más corta
            Stack<Tile> currentPath = GridManager.instance.a_Star.Repath(enemy.GetOccupiedTile(), this, true);
            Debug.Log("CalculatePathToEnemy " + currentPath.Count);
            return currentPath;
            //for(int i = 0; i  < maxG; i++) currentPath.Pop();
        }
        else
        {
            return null;
        }
    }

    protected virtual Stack<Tile> CalculatePathToTile(Tile tile)
    {
        if (tile != null)
        {
            // Usa A* para obtener la ruta más corta
            Stack<Tile> currentPath = GridManager.instance.a_Star.Repath(tile, this, true);
            //Debug.Log("CalculatePathToEnemy " + currentPath.Count);
            //for(int i = 0; i  < maxG; i++) currentPath.Pop();
            return currentPath;
        }
        else
        {
            return null;
        }
    }
}


