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
    //Tipos de tiles
    [SerializeField] private Tile grassTile, mountainTile;
    //Camara
    [SerializeField] private Transform cam;

    //Patron singleton, solo hay un GridManager y se instancia a si mismo en Awake
    public static GridManager instance;

    //Diccionario de tiles: Llave posicion, devuelve la Tile
    private Dictionary<Vector2 , Tile> tiles;

    public List<Tile> highlightedTiles;

    public A_star a_Star;

    private void Awake()
    {
        instance = this;
    }

    //Genera un grid con las dimensiones especificadas
    public void GenerateGrid()
    {
        tiles = new Dictionary<Vector2 , Tile>();
        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //si el random da 3 spawnea una Mountain, si no un Grass
                var randomTile = Random.Range(0,6) == 3 ? mountainTile : grassTile;
                var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                tiles[new Vector2(x, y)] = spawnedTile;

                spawnedTile.Init(new Vector2(x,y));
            }
        }

        Vector2 key;
        //for posterior para asociar nodos
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                key = new Vector2(x, y);
                if (tiles[key].Walkable)
                    tiles[key].node.SetAdyacentNodes(GetNeighboursNodes(tiles[key]));
            }
        }
        //Posiciona la camara en el centro del tablero
        cam.transform.position = new Vector3((float) width/2 -0.5f, (float)height / 2 - 0.5f,-10);

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
        return nodes;
    }
}
    