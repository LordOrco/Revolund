using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : BaseEnemy
{
    /*public override void Attack(BaseUnit enemy)
    {
        if (GridManager.instance.AreTilesAdjacent(enemy.GetOccupiedTile(), GetOccupiedTile()))
        {
            int dmg = Random.Range(minAttack, maxAttack);
            Debug.Log("Dmg: " + dmg);
            enemy.ReceiveDmg(dmg);
        }
    }
    /*
    private BaseUnit GetHeroUnit()
    {
        GameObject hero = GameObject.FindGameObjectWithTag("Hero");
        return hero != null ? hero.GetComponent<BaseUnit>() : null;
    }
    */
}
