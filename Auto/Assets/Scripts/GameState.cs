using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public int gameState = 0;

    private void Update()
    {
        Debug.Log("Game State " + gameState);
    }
}
