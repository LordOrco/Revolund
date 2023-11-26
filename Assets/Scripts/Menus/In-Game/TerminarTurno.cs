using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminarTurno : MonoBehaviour
{
    public void swapTurn()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.EnemyTurn);
    }
}
