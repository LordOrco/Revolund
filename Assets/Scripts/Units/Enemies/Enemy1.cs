using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : BaseEnemy
{

    private List<Tile> currentPath;
    private int currentPathIndex;
    // Start is called before the first frame update
    void Start()
    {
        // Llama al método para calcular la ruta hacia el jugador
        //CalculatePathToHero();
    }

    // Update is called once per frame
    void Update()
    {/*
         
        if (currentPath != null && currentPath.Count > 0)
        {
            // Mueve el enemigo hacia la siguiente casilla en la ruta
            MoveToTile(currentPath[currentPathIndex]);

            // Verifica si ha llegado a la casilla actual en la ruta
            if (transform.position == currentPath[currentPathIndex].transform.position)
            {
                currentPathIndex++;

                // Si ha llegado al final de la ruta, recalcula la ruta hacia el héroe
                if (currentPathIndex >= currentPath.Count)
                {
                    CalculatePathToHero();
                }
            }
        }
        // Verifica si el héroe está al lado y, si lo está, ataca
        AttackHero();*/

    }
    /*
    // Método para calcular la ruta hacia el jugador
    private void CalculatePathToHero()
    {
        GameObject hero = GameObject.FindGameObjectWithTag(BaseHero);
        if (hero != null)
        {
            Tile startTile = GridManager.instance.GetTileAtPosition(transform.position);
            Tile targetTile = GridManager.instance.GetTileAtPosition(hero.transform.position);

            // Usa A* para obtener la ruta más corta
            currentPath = GridManager.instance.a_Star.GetShortestPath(startTile, targetTile);

            // Reinicia el índice de la ruta
            currentPathIndex = 0;
        }
    }

    // Nuevo método para atacar al héroe
    private void AttackHero()
    {
        // Obtener la posición del héroe (ajusta según tu implementación)
        BaseUnit hero = GetHeroUnit();

        // Verificar si el héroe está en una casilla adyacente
        if (GridManager.instance.AreTilesAdjacent(hero.GetOccupiedTile(), GetOccupiedTile()))
        {
            // Inflictir daño al jugador
            hero.ReceiveDmg(10); // He puesto 10 como ejemplo
                                 // Ajustar como se vea conveniente
        }
    }
    //Esta función igual debería ir en Grid Manager
    public bool AreTilesAdjacent(Tile tile1, Tile tile2)
    {
        // Obtiene las posiciones de las dos casillas
        Vector2 position1 = tile1.GetPosition();
        Vector2 position2 = tile2.GetPosition();

        // Verifica si las casillas son adyacentes
        return Mathf.Abs(position1.x - position2.x) + Mathf.Abs(position1.y - position2.y) == 1;
    }

    private BaseUnit GetHeroUnit()
    {
        GameObject hero = GameObject.FindGameObjectWithTag("Hero");
        return hero != null ? hero.GetComponent<BaseUnit>() : null;
    }


    public void MoveToTile(Tile targetTile)
    {
        // Obtiene la posición actual del enemigo y la posición objetivo
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = targetTile.transform.position;

        // Calcula la nueva posición usando Vector3.MoveTowards
        Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, movementSpeed * Time.deltaTime);

        // Mueve al enemigo a la nueva posición
        transform.position = newPosition;
    }*/
}
