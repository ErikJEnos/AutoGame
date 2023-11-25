using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameLoopV2 : MonoBehaviour
{

    public GameObject player;
    public GameObject enemy;

    public GameObject playerDeckPos;
    public GameObject playerShowCardPos;

    public GameObject EnemyDeckPos;
    public GameObject EnemyShowCardPos;

    public List<GameObject> playerDeckOrder;
    public List<GameObject> enemyDeckOrder;


    public TMP_Text PhaseText;
    private GameObject gameManager;
    private GameObject gameLogic;
    public float gameSpeed;
    public bool hasWon = false;

    public GameObject Scene1;
    public GameObject Scene2;
    public GameObject Scene3;

    public bool isPaused = false;
    public bool isAutoPlay = false;


    // Start is called before the first frame update
    void Start()
    {
        gameSpeed = 0.5f;
        player = GameObject.Find("Player1");
        enemy = GameObject.Find("Player2");
        gameManager = GameObject.Find("GameManager");
        gameLogic = GameObject.Find("GameLogic");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
