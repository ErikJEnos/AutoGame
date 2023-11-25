using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class InGameUI : MonoBehaviour
{
    GameObject gameManager; 

    public int playerTurn = 1;
    public int playerWins = 0;
    public int playerloses = 10;
    public int actions = 5;

    public TMP_Text playerTurnText;
    public TMP_Text playerWinsText;
    public TMP_Text playerlosesText;
    public TMP_Text actionsText;



    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager != null)
        {
            actions = gameManager.GetComponent<PickingCards>().actions;

            playerTurnText.text = playerTurn.ToString();
            playerWinsText.text = playerWins.ToString();
            playerlosesText.text = playerloses.ToString();
            actionsText.text = actions.ToString();

        }

    }
}
