using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPausa : MonoBehaviour
{
    // Start is called before the first frame update
    public bool active = false;
    GameManager.GameState gameState;
    void Start()
    {
        gameObject.SetActive(false);
        active = false;
    }
    public void TogglePause()
    {
        if (!active)
        {
            gameState = GameManager.Instance.State;
            GameManager.Instance.ChangeState(GameManager.GameState.Pause);
        }
        else
        {
            GameManager.Instance.ChangeState(gameState);
        }
        active = !active;
        gameObject.SetActive(!gameObject.activeSelf);
        Time.timeScale = (gameObject.activeSelf) ? 0f : 1f;
    }

}
