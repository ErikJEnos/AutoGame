using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<GameObject> deck;
    public List<GameObject> deckOrdered;
    public List<string> cardId = new List<string>();
    public int id;

    public bool isReady = false;

    GameObject gameManager;
    

    void Start()
    {

    }

}
