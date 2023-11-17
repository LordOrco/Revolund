using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Manager de las unidades: Spawnea unidades y selecciona unidades
public class UnitManager : MonoBehaviour
{
    //Patron singleton, solo hay un UnitManager y se instancia a si mismo en Awake
    public static UnitManager instance;
    
    //Lista de unidades
    private List<ScriptableUnit> unitList;

    //Heroe seleccionado
    public BaseHero SelectedHero;
    private void Awake()
    {
        instance = this;

        //Obtiene todas las unidades en la carpeta Units que sean ScpritableUnits
        unitList = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    //Genera heroes aleatoriamente
    public void SpawnHeroes()
    {
        var heroCount = 1;
        //for(int i = 0; i < unitList.Count; i++) { }
        for (int i = 0; i < heroCount; i++) {
            var randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
            var spawnedHero = Instantiate(randomPrefab);

            //Obtiene un Tile donde spawnear el heroe
            var randomSpawanTile = GridManager.instance.GetHeroSpawnedTile();

            //Asocia el heroe a la casilla
            randomSpawanTile.SetUnit(spawnedHero);

        }

        //Cambia al estado de generar enemigos
        GameManager.Instance.ChangeState(GameManager.GameState.GenerateEnemyUnits);
    }

    //Genera heroes aleatoriamente
    public void SpawnEnemies()
    {
        var enemyCount = 1;
        //for(int i = 0; i < unitList.Count; i++) { }
        for (int i = 0; i < enemyCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            var spawnedEnemy = Instantiate(randomPrefab);

            //Obtiene un Tile donde spawnear el enemigo
            var randomSpawanTile = GridManager.instance.GetEnemySpawnedTile();

            //Asocia el enemigo a la casilla
            randomSpawanTile.SetUnit(spawnedEnemy);

        }

        //Cambia al estado de turno del jugador
        GameManager.Instance.ChangeState(GameManager.GameState.PlayerTurn);
    }

    //Obtiene heroe aletaorio
    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        return (T) unitList.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }
    //Selecciona al heroe pasado como parámetro
    public void SetSelectedHero(BaseHero hero)
    {
        SelectedHero = hero;
        //Enseña al heroe seleccionado
        MenuManager.Instance.ShowSelectedHero(hero);
    }
}
