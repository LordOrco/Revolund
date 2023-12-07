using System.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// Manager de los turnos, determina quien mueve y cuantos turnos llevamos
public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public enum GameState
    {
        PlayerTurn,
        EnemyTurn
    }

    private int turnoactual = 0;
    private GameState currentState;

    private void Awake()
    {
        Instance = this;
        currentState = GameState.PlayerTurn;
    }

    public void ChangeState()
    {
        if (currentState == GameState.PlayerTurn)
        {
            //Esto debería aparecer por pantalla
            Debug.Log("Turno " + turnoactual);
            Debug.Log("Jugador");

            turnoactual++;
            currentState = GameState.EnemyTurn;

        }
        else
        {
            currentState = GameState.PlayerTurn;

            //Esto debería aparecer por pantalla
            Debug.Log("Turno " + turnoactual);
            Debug.Log("Enemigo");
        }
    }



}
