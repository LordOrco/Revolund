using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

//Manager del Grid, crea un grid y maneja los tiles del mapa
public class GridManager : MonoBehaviour
{
    //Ancho y alto del tablero
    [SerializeField] private int width,height;
    //Centro del tablero
    private Vector2 gridCenter;

    //Tipos de tiles
    [SerializeField] private Tile grassTile, mountainTile, heroDPT, enemyDPT;
    //Camara
    [SerializeField] private Transform cam;

    //Patron singleton, solo hay un GridManager y se instancia a si mismo en Awake
    public static GridManager instance;

    //Diccionario de tiles: Llave posicion, devuelve la Tile
    public Dictionary<Vector2 , Tile> tiles;

    //Lista de DeployTowers
    [HideInInspector]public List<DeployTowerTile> DeployTowers;

    //Higlights de tiles del heroe seleccionado
    //public List<Tile> highlightedTiles;

    public A_star a_Star;

    private void Awake()
    {
        instance = this;
        gridCenter = new Vector2((float)width / 2 - 0.5f, (float)height / 2 - 0.5f);
    }

    //Genera un grid con las dimensiones especificadas
    public void GenerateGrid()
    {
        tiles = new Dictionary<Vector2 , Tile>();
        int random;
        Tile randomTile = null;
        bool hasTowerSpawned = false;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //si el random da 3 spawnea una Mountain, si no un Grass
                random = Random.Range(0, 10);
                if (random == 0) randomTile = mountainTile;
                else if (random == 8 && !hasTowerSpawned) { randomTile = heroDPT; hasTowerSpawned = true; }
                else { randomTile = grassTile; }

                //var randomTile = Random.Range(0,6) == 3 ? mountainTile : grassTile;
                var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                tiles[new Vector2(x, y)] = spawnedTile;

                spawnedTile.Init(new Vector2(x,y), Faction.Hero);
            }
        }

        Vector2 key;
        //for posterior para asociar nodos
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                key = new Vector2(x, y);
                if (tiles[key].GetNeedsAdyacentNodes())
                    tiles[key].node.SetAdyacentNodes(GetNeighboursNodes(tiles[key]));
            }
        }
        //Posiciona la camara en el centro del tablero
        cam.transform.position = new Vector3(gridCenter.x, gridCenter.y, -10);

        //Cambia al estado de generar las unidades aliadas
        GameManager.Instance.ChangeState(GameManager.GameState.GeneratePlayerUnits);
    }

    public void GenerateTutorialGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        Tile actualTile = null;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                switch (x,y)
                {
                    default:
                        actualTile = grassTile;       
                        break;

                    case (4, 0): case (5, 0): case (6, 0): case (7, 0): case (8, 0): case (9, 2): case (9, 3): case (10, 2): case (12, 3): case (12, 4):
                    case (13, 3): case (13, 2): case (17, 1): case (17, 2): case (17, 3): case (17, 4): case (14, 5): case (14, 6): case (14, 7): case (1, 6):
                    case (1, 7): case (1, 8): case (1, 9): case (1, 10): case (2, 10): case (4, 6): case (5, 6): case (6, 8): case (6, 9): case (6, 10):
                    case (14, 10): case (15, 10): case (16, 10): case (17, 10): case (5, 11): case (5, 12): case (5, 13): case (5, 14): case (19, 19): case (19, 18):
                    case (6, 12): case (6, 13): case (6, 14): case (6, 15): case (6, 16): case (6, 17): case (6,19): case (5, 19): case (4, 19): case (3, 19):
                    case (2, 19): case (7, 16): case (8, 17): case (0, 15): case (0, 16): case (0, 17): case (1,17): case (16, 14): case (17, 14): case (17, 15):
                    case (9, 12): case (10, 12): case (10, 13): case (11, 13): case (12, 13): case (13, 13): case (13,14): case (13, 15): case (13, 16):
                        actualTile = mountainTile;
                        break;

                    case (3, 2):
                        actualTile = heroDPT;
                        break;
                    case (10, 9): case (15, 17):
                        actualTile = enemyDPT;
                        break;
                }

                //var randomTile = Random.Range(0,6) == 3 ? mountainTile : grassTile;
                var spawnedTile = Instantiate(actualTile, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                tiles[new Vector2(x, y)] = spawnedTile;

                if(spawnedTile.faction == Faction.Hero)
                    spawnedTile.Init(new Vector2(x, y), Faction.Hero);
                else
                    spawnedTile.Init(new Vector2(x, y), Faction.Enemy);
            }
        }

        Vector2 key;
        //for posterior para asociar nodos
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                key = new Vector2(x, y);
                if (tiles[key].GetNeedsAdyacentNodes())
                    tiles[key].node.SetAdyacentNodes(GetNeighboursNodes(tiles[key]));
            }
        }

        //For para establecer la faccion de las tiles adyacentes a las DeployTowers
        for(int j = 0; j < DeployTowers.Count; j++)
        {
            DeployTowers[j].SetAdyacentFaction();
        }
        //Posiciona la camara en el centro del tablero
        cam.transform.position = new Vector3(gridCenter.x, gridCenter.y, -10);

        //Cambia al estado de generar las unidades aliadas
        GameManager.Instance.ChangeState(GameManager.GameState.GeneratePlayerUnits);
    }

    //Devuelve una posicion a la izquierda del tablero para spawnear heroes
    public Tile GetHeroSpawnedTile()
    {
        return tiles.Where(t => t.Key.x < width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    //Devuelve una posicion a la derecha del tablero para spawnear enemigos
    public Tile GetEnemySpawnedTile()
    {
        return tiles.Where(t => t.Key.x > width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    //Obtiene la tile pasandole su posicion
    public Tile GetTileAtPosition(Vector2 position)
    {
        if(tiles.TryGetValue(position, out var tile))
        {
            return tile;
        }

        return null;
    }  

    public List<Node> GetNeighboursNodes(Tile tile)
    {
        List<Node> nodes = new List<Node>();
        float x = tile.GetPosition().x;
        float y = tile.GetPosition().y;

        //Debug.Log("Vecinos de : " + tiles[new Vector2(x, y)].node.currentPos);

        //x+1
        Vector2 key = new Vector2(x + 1, y);
        if (tiles.ContainsKey(key) && tiles[key].Walkable != false)
        {
            nodes.Add(tiles[key].node);
        }

        //x-1
        key = new Vector2(x - 1, y);
        if (tiles.ContainsKey(key) && tiles[key].Walkable != false)
        {
            nodes.Add(tiles[key].node);
        }

        //y+1
        key = new Vector2(x, y + 1 );
        if (tiles.ContainsKey(key) && tiles[key].Walkable != false)
        {
            nodes.Add(tiles[key].node);
        }

        //y-1
        key = new Vector2(x, y -1 );
        if (tiles.ContainsKey(key) && tiles[key].Walkable != false)
        {
            nodes.Add(tiles[key].node);
        }

        //Si es una Deploy Tower, obtiene las esquinas
        if(tile is DeployTowerTile)
        {
            //x+1, y+1
            key = new Vector2(x + 1, y + 1);
            if (tiles.ContainsKey(key) && tiles[key].Walkable != false)
            {
                nodes.Add(tiles[key].node);
            }

            //x-1,y-1
            key = new Vector2(x - 1, y - 1);
            if (tiles.ContainsKey(key) && tiles[key].Walkable != false)
            {
                nodes.Add(tiles[key].node);
            }

            //x-1, y+1
            key = new Vector2(x - 1, y + 1);
            if (tiles.ContainsKey(key) && tiles[key].Walkable != false)
            {
                nodes.Add(tiles[key].node);
            }

            //x+1, y-1
            key = new Vector2(x+ 1, y - 1);
            if (tiles.ContainsKey(key) && tiles[key].Walkable != false)
            {
                nodes.Add(tiles[key].node);
            }
        }
        return nodes;
    }

    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
    public Vector2 GetGridCenter()
    {
        return gridCenter;
    }

    public bool AreTilesAdjacent(Tile tile1, Tile tile2)
    {
        // Obtiene las posiciones de las dos casillas
        Vector2 position1 = tile1.GetPosition();
        Vector2 position2 = tile2.GetPosition();

        // Verifica si las casillas son adyacentes
        return Mathf.Abs(position1.x - position2.x) + Mathf.Abs(position1.y - position2.y) == 1;
    }

    public void ShowDeployTowersTiles()
    {
        for(int i = 0; i < DeployTowers.Count; i++)
        {
            if (DeployTowers[i].faction == Faction.Hero)
            DeployTowers[i].ActivateDeployTiles();
        }
    }

    public void HideDeployTowersTiles()
    {
        for (int i = 0; i < DeployTowers.Count; i++)
        {
            DeployTowers[i].DeactivateDeployTiles();
        }
    }

    public void NewTurn()
    {
        for(int j = 0; j < DeployTowers.Count; j++)
        {
            DeployTowers[j].UpdateTowerFaction();
        }
    }

}
