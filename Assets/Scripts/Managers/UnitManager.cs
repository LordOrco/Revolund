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

    private List<BaseEnemy> enemyList;
    private List<BaseHero> heroList;

    [SerializeField] private BaseHero hero;
    [SerializeField] private BaseEnemy enemy;
    //Heroe seleccionado
    public BaseHero SelectedHero;
    public bool canInstance;
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
            //var randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
            var spawnedHero = Instantiate(hero);
            heroList.Add(spawnedHero);

            //Obtiene un Tile donde spawnear el heroe
            var randomSpawnTile = GridManager.instance.GetHeroSpawnedTile();

            //Asocia el heroe a la casilla
            randomSpawnTile.SetUnit(spawnedHero);

        }

        //Cambia al estado de generar enemigos
        GameManager.Instance.ChangeState(GameManager.GameState.GenerateEnemyUnits);
    }

    //Spawnea heroe aleatorio en al tile pasada
    public void SpawnHeroOnDemand(Tile tile)
    {
        var spawnedHero = Instantiate(hero);
        tile.SetUnit(spawnedHero);
    }

    //Genera heroes aleatoriamente
    public void SpawnEnemies()
    {
        var enemyCount = 2;
        //for(int i = 0; i < unitList.Count; i++) { }
        for (int i = 0; i < enemyCount; i++)
        {
            //var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            var spawnedEnemy = Instantiate(enemy);
            enemyList.Add(spawnedEnemy);

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
        if (hero.GetOccupiedTile() == null)
        {
            canInstance = true;
            SelectedHero = hero;
        }
        else
        {


            //Debug.Log(hero);
            if (SelectedHero != null)
            {
                SelectedHero.HidePathingTiles();
            }

            if (hero != null)
            {
                hero.ShowPathingTiles();
            }
            if (hero == null)
            {
                SelectedHero.HidePathingTiles();
            }

            SelectedHero = hero;
            //Enseña al heroe seleccionado
            MenuManager.Instance.ShowSelectedHero(hero);
        }
    }

    public void SetBoughtHero(BaseHero hero)
    {
        SelectedHero = hero;
    }

    public List<ScriptableUnit> GetUnitList()
    {
        return unitList;
    }

    public void HasToChangeState(int type)
    {
        bool change = true;
        if (type == 0)
        {
            for(int i = 0; i < heroList.Count; i++)
            {
                if (heroList[i].canAttack) change = false;
            }
            if (change) GameManager.Instance.ChangeState(GameManager.GameState.EnemyTurn);
            Debug.Log("EnemyTurn");
        }
        else
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].canAttack) change = false;
            }
            if (change) GameManager.Instance.ChangeState(GameManager.GameState.PlayerTurn);
            Debug.Log("HeroTurn");
        }

    }
}
