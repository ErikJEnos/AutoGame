using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<GameObject> deck;
    public List<GameObject> deckOrdered;
    GameObject gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }
}
