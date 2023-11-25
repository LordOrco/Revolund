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
        // Llama al m�todo para calcular la ruta hacia el jugador
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

                // Si ha llegado al final de la ruta, recalcula la ruta hacia el h�roe
                if (currentPathIndex >= currentPath.Count)
                {
                    CalculatePathToHero();
                }
            }
        }
        // Verifica si el h�roe est� al lado y, si lo est�, ataca
        AttackHero();*/

    }
    /*
    // M�todo para calcular la ruta hacia el jugador
    private void CalculatePathToHero()
    {
        GameObject hero = GameObject.FindGameObjectWithTag(BaseHero);
        if (hero != null)
        {
            Tile startTile = GridManager.instance.GetTileAtPosition(transform.position);
            Tile targetTile = GridManager.instance.GetTileAtPosition(hero.transform.position);

            // Usa A* para obtener la ruta m�s corta
            currentPath = GridManager.instance.a_Star.GetShortestPath(startTile, targetTile);

            // Reinicia el �ndice de la ruta
            currentPathIndex = 0;
        }
    }

    // Nuevo m�todo para atacar al h�roe
    private void AttackHero()
    {
        // Obtener la posici�n del h�roe (ajusta seg�n tu implementaci�n)
        BaseUnit hero = GetHeroUnit();

        // Verificar si el h�roe est� en una casilla adyacente
        if (GridManager.instance.AreTilesAdjacent(hero.GetOccupiedTile(), GetOccupiedTile()))
        {
            // Inflictir da�o al jugador
            hero.ReceiveDmg(10); // He puesto 10 como ejemplo
                                 // Ajustar como se vea conveniente
        }
    }
    //Esta funci�n igual deber�a ir en Grid Manager
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
        // Obtiene la posici�n actual del enemigo y la posici�n objetivo
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = targetTile.transform.position;

        // Calcula la nueva posici�n usando Vector3.MoveTowards
        Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, movementSpeed * Time.deltaTime);

        // Mueve al enemigo a la nueva posici�n
        transform.position = newPosition;
    }*/
}
