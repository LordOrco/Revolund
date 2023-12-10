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
    [HideInInspector] private List<ScriptableUnit> unitList;

    [HideInInspector] public List<BaseUnit> enemyList;
    [HideInInspector] public List<BaseUnit> heroList;

    public BaseEnemy[] tiposEnemigos;

    [HideInInspector] public int heroesAttacked = 0;
    [HideInInspector] public int heroes = 0;

    [SerializeField] private BaseHero hero;
    [SerializeField] private BaseEnemy enemy;
    //Heroe seleccionado
    public BaseHero SelectedHero;
    public bool canInstance;
    
    public delegate void SoundcolocarTropa(int sonido);
    public static event SoundcolocarTropa OnSoundcolocarTropa;
    private void Awake()
    {
        instance = this;

        //Obtiene todas las unidades en la carpeta Units que sean ScpritableUnits
        unitList = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }
    //Genera heroes aleatoriamente

    public void checkState()
    {
        //Debug.Log(heroList.Count + " AAAAAAAAAHEROES");
         //Debug.Log(enemyList.Count + " AAAAAAAAAENEMIES");
        if (heroList.Count <= 0)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.Lose);
            Debug.Log("Lose");
            return;
        }
        else if (enemyList.Count <= 0)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.Victory);
            Debug.Log("Victory");
            return;
        }
        bool change = true;

        if(GameManager.Instance.State == GameManager.GameState.PlayerTurn)
        {
            //for(int i = 0; i < heroList.Count; i++) 
            //{
            //    if (heroList[i].canAttack == true) { change = false; }
            //}
            Debug.Log("HereoesAttacked " + heroesAttacked);
            Debug.Log("Hereoes " + heroes);
            if (heroes <= heroesAttacked)
                GameManager.Instance.ChangeState(GameManager.GameState.EnemyTurn);
            //if (change) GameManager.Instance.ChangeState(GameManager.GameState.EnemyTurn);
        }

        if (GameManager.Instance.State == GameManager.GameState.EnemyTurn)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].canMove == true) { change = false; }
            }
            if (change) GameManager.Instance.ChangeState(GameManager.GameState.PlayerTurn);
        }
    }

    //Metodo que gestiona los nuevos turnos
    public void NewTurn(Faction faccion)
    {
        //SI es el turno de los heroes, resetea el ataque
        if(faccion == Faction.Hero)
        {
            for (int i = 0; i < heroList.Count; i++)
            {
                heroList[i].canAttack = true;
                heroList[i].canMove = true;
                heroList[i].gameObject.GetComponent<SpriteRenderer>().color = heroList[i].hasDontAttackedColor;
                heroesAttacked = 0;
            }
        }

        //Si es el turno enemigo, los enemigos buscan un enemigo en rango y atacan
        if (faccion == Faction.Enemy)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                //Tiles de alrededor
                BaseUnit enemigo = null;
                List<Tile> accesibleTile = GridManager.instance.a_Star.ObtainAccesibleTiles(enemyList[i]);

                //Busca enemigo
                for (int  j = 0; j < accesibleTile.Count; j++)
                {
                    if (accesibleTile[j].OccupiedUnit != null && accesibleTile[j].OccupiedUnit.Faction == Faction.Hero) 
                    {
                        enemigo = accesibleTile[j].OccupiedUnit;
                    }
                }

                enemyList[i].canAttack = true;
                enemyList[i].canMove = true;

                //Si hay ataca
                if (enemigo != null)
                {
                    Debug.Log("Ataque Enemigo");
                    enemyList[i].Attack(enemigo);
                }

                //Si no se queda quieto
                else
                {
                    enemyList[i].MoveToTile(enemyList[i].GetOccupiedTile());
                }
            }

            //SpawnEnemiesOnTowers();
        }
    }
    public void SpawnHeroes()
    {
        var heroCount = 1;
        //for(int i = 0; i < unitList.Count; i++) { }
        for (int i = 0; i < heroCount; i++) {
            //var randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
            BaseHero spawnedHero = Instantiate(hero);

            //Obtiene un Tile donde spawnear el heroe
            //var randomSpawnTile = GridManager.instance.GetHeroSpawnedTile();

            //Asocia el heroe a la casilla
            GridManager.instance.tiles[new Vector2(5,4)].SetUnit(spawnedHero);
            heroes++;
        }

        //Cambia al estado de generar enemigos
        GameManager.Instance.ChangeState(GameManager.GameState.GenerateEnemyUnits);
    }

    //Spawnea heroe aleatorio en al tile pasada
    public void SpawnHeroOnDemand(Tile tile, BaseHero hero)
    {
        var spawnedHero = Instantiate(hero);
        tile.SetUnit(spawnedHero);
        OnSoundcolocarTropa?.Invoke(4);
    }

    //Spawnea enemnigos en una tile aleatoria adyacente a una torre enemiga
    public void SpawnEnemiesOnSpecificTower(DeployTowerTile tower, BaseEnemy enemigo)
    {
        bool desplegado = false;
        int i = 0;
        if(tower.faction == Faction.Enemy)
        {
            //int tileDespliegue = Random.Range(0, tower.node.adyacent_Nodes.Count);
            //BaseEnemy enemigo = Random.Range(0, (tiposEnemigos.Count() - 1));
            var spawnedEnemy = Instantiate(enemigo);
            while( i < tower.node.adyacent_Nodes.Count && desplegado == false)
            {
                if (tower.node.adyacent_Nodes[i].myTile.OccupiedUnit == null)
                {
                    desplegado= true;
                    tower.node.adyacent_Nodes[i].myTile.SetUnit(spawnedEnemy);
                }

                i++;
            }
            if(desplegado == false)
            {
                Debug.Log("No se puede desplegar el enemigo");
            }
        }
    }

    public void SpawnEnemiesOnTowers()
    {
        int tope = GridManager.instance.DeployTowers.Count;
        for(int i = 0; i < tope; i++) 
        {
            if (GridManager.instance.DeployTowers[i].faction == Faction.Enemy)
                SpawnEnemiesOnSpecificTower(GridManager.instance.DeployTowers[i], tiposEnemigos[0]);
        }
    }


    //Genera heroes aleatoriamente
    public void SpawnEnemies()
    {
        var enemyCount = 2;
        //for(int i = 0; i < unitList.Count; i++) { }
       /* for (int i = 0; i < enemyCount; i++)
        {
            //var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            var spawnedEnemy = Instantiate(enemy);

            //Obtiene un Tile donde spawnear el enemigo
            var randomSpawanTile = GridManager.instance.GetEnemySpawnedTile();

            //Asocia el enemigo a la casilla
            randomSpawanTile.SetUnit(spawnedEnemy);

        }*/
        var spawnedEnemy = Instantiate(enemy);

        //Asocia el enemigo a la casilla
        GridManager.instance.tiles[new Vector2(10, 10)].SetUnit(spawnedEnemy);

        var spawnedEnemy2 = Instantiate(enemy);

        GridManager.instance.tiles[new Vector2(15, 16)].SetUnit(spawnedEnemy2);


        //Cambia al estado de turno del jugador
        GameManager.Instance.ChangeState(GameManager.GameState.PlayerTurn);
    }

    //Obtiene heroe aletaorio
    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        return (T) unitList.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }
    //Selecciona al heroe pasado como par�metro
    public void SetSelectedHero(BaseHero hero)
    {
        if (hero != null && hero.GetOccupiedTile() == null)
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
            //Ense�a al heroe seleccionado
            MenuManager.Instance.ShowSelectedHero(hero);
        }
    }

    public void cancelBuyUnit(BaseHero Hero)
    {
        if(hero == null)
        {
            canInstance = false;
            SelectedHero = null;
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
