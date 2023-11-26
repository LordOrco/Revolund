using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

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
        Tile targetTile;
        targetTile = EvaluateEnemyAdyacentTiles(enemy.OccupiedTile);
        Debug.Log("TargetTile pasado");
        Stack<Tile> currentPath = CalculatePathToTile(targetTile);
        Debug.Log("Target Tile: " + targetTile);
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

        barraVida.value = HP;
        if (HP <= 0)
        {
            Invoke("Kill", 1f);
        }
    }

    protected virtual void ApplyDmg(BaseUnit enemy)
    {
        int dmg = Random.Range(minAttack, maxAttack);
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
            Debug.Log("CalculatePathToEnemy " + currentPath.Count);
            //for(int i = 0; i  < maxG; i++) currentPath.Pop();
            return currentPath;
        }
        else
        {
            return null;
        }
    }


    protected virtual Tile EvaluateEnemyAdyacentTiles(Tile enemyTile)
    {
        Stack<Tile> Path = GridManager.instance.a_Star.Repath(enemyTile, this , false);
        Path.Pop();
        return Path.Peek();
        /*
        int maxSteps = 300;
        int steps = 0;
        if (enemyTile != null)
        {
            for (int i = 0; i < enemyTile.node.adyacent_Nodes.Count; i++)
            {
                if (enemyTile.node.adyacent_Nodes[i].myTile.OccupiedUnit != null && enemyTile.node.adyacent_Nodes[i].myTile.OccupiedUnit == this)
                    return enemyTile.node.adyacent_Nodes[i].myTile;
            }

            List<Node> opened_list = enemyTile.node.adyacent_Nodes;
            List<Node> closed_list = new List<Node>();
            closed_list.Add(enemyTile.node);


            opened_list.Sort((x, y) => x.Manhattan(enemyTile.GetPosition()).CompareTo(y.Manhattan(enemyTile.GetPosition())));


            while(opened_list.Count > 0 && steps < maxSteps)
            {
                
                if (opened_list[0].myTile.OccupiedUnit == null)
                {
                    Debug.Log("Tile encontrada: " + opened_list[0].myTile);
                    return opened_list[0].myTile; 
                }

                else
                {
                    Debug.Log("Tile Ocupada: " + opened_list[0].myTile);
                    for (int j = 0; j < opened_list[0].adyacent_Nodes.Count; j++) {
                        if (!(closed_list.Contains(opened_list[0].adyacent_Nodes[j]))
                            && opened_list[0].adyacent_Nodes[j].myTile.OccupiedUnit == null)
                            opened_list.Add(opened_list[0].adyacent_Nodes[j]);
                    }


                    closed_list.Add(opened_list[0]);
                    opened_list.Remove(opened_list[0]);
                    opened_list.Sort((x, y) => x.Manhattan(enemyTile.GetPosition()).CompareTo(y.Manhattan(enemyTile.GetPosition())));
                    Debug.Log("Tile Ocupada: " + enemyTile.node.adyacent_Nodes[0].myTile);
                }
                steps++;
            }
            return null;
            /*
            openeed_list.Sort((x, y) => x.f_star.CompareTo(y.f_star));

            
            for (int i = 0; i < openeed_list.Count; i++)
            {
                if (openeed_list.ElementAt(i).OccupiedUnit == null
                || openeed_list.ElementAt(i).OccupiedUnit == this) { return openeed_list.ElementAt(i); }
            }

            return EvaluateEnemyAdyacentTiles(openeed_list.First());

            //Debug.Log("EvaluateEnemyAdyacentTiles error");
            //return null;
            
        }

        else
        {
            return null;
        }*/
    }
}


