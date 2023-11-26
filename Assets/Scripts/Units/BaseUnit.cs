using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;

public class BaseUnit : MonoBehaviour
{
    public string unitName;
    public Faction Faction;
    public int maxG;

    //Atributos para movimiento
    protected int movementSpeed;

    //Unidad a la que ataca, null si no hay ninguna
    protected BaseUnit attackedEnemy;

    public int HP, minAttack, maxAttack;

    //Tiles asociados cuyos highlights estan asociados
    protected List<Tile> highlightedTiles;
    private bool areAccesibleTilesShown;
    private Tile OccupiedTile;

    [SerializeField] private Slider barraVida;

    public List<Tile> GetHighlightedTiles() { return highlightedTiles; }
    public void SetHighlightedTiles(List<Tile> tiles) { this.highlightedTiles = tiles; }
    public bool GetAreAccesibleTilesShown() { return areAccesibleTilesShown; }
    public void SetAreAccesibleTilesShown(bool state) { areAccesibleTilesShown = state; }
    public Tile GetOccupiedTile() { return OccupiedTile; }
    public void SetOccupiedTile(Tile tile) { this.OccupiedTile = tile; }

    protected virtual void Awake()
    {
        areAccesibleTilesShown = false;
        highlightedTiles = new List<Tile>();
        attackedEnemy = null;
        barraVida.maxValue = HP;
        barraVida.value = HP;
        barraVida.minValue = 0;
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
    }

    public virtual void Attack(BaseUnit enemy)
    {
        Stack<Tile> currentPath = new Stack<Tile>();
        Tile targetTile;
        targetTile = EvaluateEnemyAdyacentTiles(enemy);
        currentPath = CalculatePathToTile(targetTile);
        if (currentPath != null)
        {
            MoveToTile(currentPath.Peek());
            ApplyDmg(enemy);
        }
        //Debug.Log("Enemy: " + enemy.name);
    }

    public virtual void ReceiveDmg(int dmg)
    {
        HP -= dmg;
        Debug.Log("HP :" + HP);

        barraVida.value = HP;
        if (HP <= 0)
        {
            Invoke("Kill", 1f);
        }
    }

    protected virtual void ApplyDmg(BaseUnit enemy)
    {
        int dmg = Random.Range(minAttack, maxAttack);
        Debug.Log("Dmg: " + dmg);
        enemy.ReceiveDmg(dmg);
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
        if (targetTile != null)
            targetTile.SetUnit(this);
        else
            Debug.Log("MoveToTile no hay camino");
        ///currentPath = null;


    }

    protected virtual Stack<Tile> CalculatePathToEnemy(BaseUnit enemy)
    {
        if (enemy != null)
        {
            // Usa A* para obtener la ruta más corta
            Stack<Tile> currentPath = GridManager.instance.a_Star.Repath(enemy.GetOccupiedTile(), this);
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
            Stack<Tile> currentPath = new Stack<Tile>();
            // Usa A* para obtener la ruta más corta
            currentPath = GridManager.instance.a_Star.Repath(tile, this);
            Debug.Log("CalculatePathToEnemy " + currentPath.Count);
            //for(int i = 0; i  < maxG; i++) currentPath.Pop();
            return currentPath;
        }
        else
        {
            return null;
        }
    }


    protected virtual Tile EvaluateEnemyAdyacentTiles(BaseUnit enemy)
    {
        if (enemy != null)
        {
            Tile enemyTile = enemy.GetOccupiedTile();
            List<Tile> closestTiles = new List<Tile>();
            int minDistance = 100;
            int distance;

            for (int i = 0; i < enemyTile.node.adyacent_Nodes.Count; i++)
            {
                distance = enemyTile.node.adyacent_Nodes.ElementAt(i).Manhattan(OccupiedTile.GetPosition());
                if (distance <= minDistance)
                {
                    minDistance = distance;
                    closestTiles.Add(enemyTile.node.adyacent_Nodes.ElementAt(i).myTile);

                }
                else Debug.Log("Tile Ocupada: " + enemyTile.node.adyacent_Nodes.ElementAt(i).myTile);
            }

            for (int i = 0; i < closestTiles.Count; i++)
            {
                if (closestTiles.ElementAt(i).OccupiedUnit == null
                || closestTiles.ElementAt(i).OccupiedUnit == this) { return closestTiles.ElementAt(i); }
            }
            return null;

        }

        else
        {
            return null;
        }
    }
}


