using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Manger del juego, patron State para organizar estados del juego
public class GameManager : MonoBehaviour
{
    //Patron singleton, solo hay un GameManager y se instancia a si mismo en Awake
    public static GameManager Instance;

    //Stado del juegp
    public GameState State;

    //Llama a las acciones al cambiar estado
    public static event Action<GameState> OnGameStateChanged;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Al Iniciar genera el grid
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        State = newState;
        //switch de estados
        switch (newState) { 
            case GameState.GenerateGrid:
                GridManager.instance.GenerateGrid();
                break;
            case GameState.GeneratePlayerUnits:
                UnitManager.instance.SpawnHeroes();
                break;
            case GameState.GenerateEnemyUnits:
                UnitManager.instance.SpawnEnemies();
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.Victory: 
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState,null);      
        }
        //Si ha cambiado el estado llama al nuevo estado
        OnGameStateChanged?.Invoke(newState);
    }

    //Enumeracion de estados
    public enum GameState
    {
        GenerateGrid = 0,
        GeneratePlayerUnits = 1,
        GenerateEnemyUnits = 2,
        PlayerTurn = 3,
        EnemyTurn = 4,
        Victory = 5,
        Lose = 6
    }
}