using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseUnit : MonoBehaviour
{
    public string unitName;
    public Faction Faction;
    public int maxG;

    public int HP, minAttack, maxAttack;

    //Tiles asociados cuyos highlights estan asociados
    protected List<Tile> highlightedTiles;
    private bool areAccesibleTilesShown;
    private Tile OccupiedTile;


    public List<Tile> GetHighlightedTiles() {  return highlightedTiles; }
    public void SetHighlightedTiles(List<Tile> tiles) { this.highlightedTiles = tiles; }
    public bool GetAreAccesibleTilesShown() { return areAccesibleTilesShown; }
    public void SetAreAccesibleTilesShown(bool state) { areAccesibleTilesShown = state; }
    public Tile GetOccupiedTile() { return OccupiedTile; }
    public void SetOccupiedTile(Tile tile) {  this.OccupiedTile = tile; }

    protected void Awake()
    {
        areAccesibleTilesShown = false;
        highlightedTiles = new List<Tile>();
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
        int dmg = Random.Range(minAttack, maxAttack);
        Debug.Log("Dmg: " + dmg);
        enemy.ReceiveDmg(dmg);
    }

    public virtual void ReceiveDmg(int dmg)
    {
        HP -= dmg;
        Debug.Log("HP :" + HP);
        if(HP <= 0)
        {
            Kill();
        }
    }

}


