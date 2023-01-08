using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameLoop : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public GameObject cardPrefab;

    public GameObject playerDeckPos;
    public GameObject playerSpellPos;
    private GameObject card;
    public int playerMosterSlot = 0;
    public List<GameObject> playerDeckOrder;
    public List<GameObject> playerSlotPos;
    public List<GameObject> playerSlotPosTemp;

    public GameObject enemyDeckPos;
    public GameObject enemySpellPos;
    private GameObject enemyCard;
    public int enemyMosterSlot = 0;
    public List<GameObject> enemyDeckOrder;
    public List<GameObject> enemySlotPos;
    public List<GameObject> enemySlotPosTemp;


    public TMP_Text PhaseText;
    private GameObject gameManager;
    public float gameSpeed;
    public bool hasWon = false;
    
    public GameObject Scene1;
    public GameObject Scene2;
    public GameObject Scene3;

    public bool isPaused = false;
    public bool isAutoPlay = false;

    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        gameSpeed = 1.0f;
        player = GameObject.Find("Player");
        enemy = GameObject.Find("Enemy");
        gameManager = GameObject.Find("GameManager");
    }

    private void Update()
    {

        if(gameManager.GetComponent<GameState>().gameState == 2 && isAutoPlay)
        {      
            gameManager.GetComponent<GameState>().gameState = 3;
            StartCoroutine(GamePlayLoop(gameSpeed));
        }
    }

    public IEnumerator GamePlayLoop(float waitTime)
    {
        while(!isPaused && !isAutoPlay)
        {
            yield return null;
        }

        PhaseText.text = "Phase: Start";
        CheckWinner();

        if (playerDeckOrder.Count > 0 || enemyDeckOrder.Count > 0)
        {
            yield return new WaitForSeconds(waitTime);
            CheckSlot();


            yield return new WaitForSeconds(waitTime);
            PlayCard();
            PhaseText.text = "Phase: Show card";


            yield return new WaitForSeconds(waitTime);
            SetCard();
            PhaseText.text = "Phase: Setcard";

        }

        //CardCleanUp();

        yield return new WaitForSeconds(waitTime);
        MonsterAttack();
        PhaseText.text = "Phase: Fight";

    
        yield return new WaitForSeconds(waitTime);
        CardCleanUp();
        PhaseText.text = "Phase: End turn";

        yield return new WaitForSeconds(waitTime);

        isPaused = false;

        StartCoroutine(GamePlayLoop(gameSpeed));



    }

    private void CheckWinner()
    {

        if(playerDeckOrder.Count <= 0 && enemyDeckOrder.Count <= 0 && hasWon == false)
        {
            int enemyCount = 0;
            int playerCount = 0;

            for (int x = 0; x < enemySlotPos.Count; x++)
            {
                if (enemySlotPos[x].tag == "Card")
                {
                    enemyCount++;
                }
            }  

            for (int x = 0; x < playerSlotPos.Count; x++)
            {
                if (playerSlotPos[x].tag == "Card")
                {
                    playerCount++;
                }
            }

            //Debug.Log("Player count " + playerCount);
           // Debug.Log("Enemy count " + enemyCount);

            if (playerCount == 0)
            {
                hasWon = true;
                //Debug.Log("P1 Wins");
                //WinningText.text = "P1 Wins";
                BackToPickACard();
            }
            else if (enemyCount == 0)
            {
                hasWon = true;
                //Debug.Log("P2 Wins");
                //WinningText.text = "P2 Wins";
                BackToPickACard();
            }

        }



    }

    private void CheckSlot()
    {
        playerMosterSlot = 0;
        enemyMosterSlot = 0;

        for (int x = 0; x < playerSlotPos.Count; x++)
        {
            if (playerSlotPos[x].tag != "Card")
            {
                //Debug.Log("no monster is slot " + x);
            }
            else
            {
                //Debug.Log("moster card in slot " + x + " moving next monster to open slot");
                playerMosterSlot = x + 1;
            }
        }

        for (int x = 0; x < enemySlotPos.Count; x++)
        {
            if (enemySlotPos[x].tag != "Card")
            {
                //Debug.Log("no monster is slot " + x);
            }
            else
            {
                //Debug.Log("moster card in slot " + x + " moving next monster to open slot");
                enemyMosterSlot = x + 1;
            }
        }
        
    }

    private void SetCard()
    {
        if(card != null)
        {


            if (enemyCard.GetComponent<Card>().isMonster == false && card.GetComponent<Card>().isMonster)
            {
                CheckSpell(enemyCard.GetComponent<Card>().cardID, enemyCard);
                enemyCard.transform.position = enemySlotPos[enemyMosterSlot].transform.position;
                enemyCard.SetActive(false);

                card.transform.position = playerSlotPos[playerMosterSlot].transform.position;
                playerSlotPos[playerMosterSlot] = card;
            }
            else if (enemyCard.GetComponent<Card>().isMonster && card.GetComponent<Card>().isMonster == false)
            {

                CheckSpell(card.GetComponent<Card>().cardID, card);
                card.transform.position = playerSlotPos[playerMosterSlot].transform.position; 
                card.SetActive(false);

                enemyCard.transform.position = enemySlotPos[enemyMosterSlot].transform.position;
                enemySlotPos[enemyMosterSlot] = enemyCard;

            }
            else if(enemyCard.GetComponent<Card>().isMonster && card.GetComponent<Card>().isMonster)
            {

                enemyCard.transform.position = enemySlotPos[enemyMosterSlot].transform.position;
                enemySlotPos[enemyMosterSlot] = enemyCard;

                card.transform.position = playerSlotPos[playerMosterSlot].transform.position;
                playerSlotPos[playerMosterSlot] = card;
            }
            else if (enemyCard.GetComponent<Card>().isMonster == false && card.GetComponent<Card>().isMonster == false)
            {

                CheckSpell(enemyCard.GetComponent<Card>().cardID, enemyCard);
                enemyCard.transform.position = enemySlotPos[enemyMosterSlot].transform.position;
                enemyCard.SetActive(false);

                CheckSpell(card.GetComponent<Card>().cardID, card);
                card.transform.position = playerSlotPos[playerMosterSlot].transform.position;
                card.SetActive(false);
            }

        }
    }

    private void MonsterAttack()
    {
        if (playerSlotPos[0].GetComponent<Card>() != null && enemySlotPos[0].GetComponent<Card>() != null)
        {
            if (playerSlotPos[0].GetComponent<Card>().isMonster == true && enemySlotPos[0].GetComponent<Card>().isMonster == true)
            {
                playerSlotPos[0].GetComponent<Card>().defence -= enemySlotPos[0].GetComponent<Card>().attack;
                enemySlotPos[0].GetComponent<Card>().defence -= playerSlotPos[0].GetComponent<Card>().attack;
            }
        }
    }

    private void CardCleanUp()
    {

        if (playerSlotPos[0].GetComponent<Card>() != null)
        {
            if (playerSlotPos[0].GetComponent<Card>().defence <= 0)
            {
                playerSlotPos[0].SetActive(false);
                playerSlotPos.Remove(playerSlotPos[0]);
            }

        }

        if (enemySlotPos[0].GetComponent<Card>() != null)
        {
            if (enemySlotPos[0].GetComponent<Card>().defence <= 0)
            {
                enemySlotPos[0].SetActive(false);
                enemySlotPos.Remove(enemySlotPos[0]);
            }
        }

        for (int x = 0; x < playerSlotPos.Count; x++)
        {
            if (playerSlotPos[x].GetComponent<Card>() is null)
            {
                playerSlotPos[x] = playerSlotPosTemp[x];
            }
            else
            {
                playerSlotPos[x].transform.position = playerSlotPosTemp[x].transform.position;
            }

        }

        for (int x = 0; x < enemySlotPos.Count; x++)
        {
            if (enemySlotPos[x].GetComponent<Card>() is null)
            {
                enemySlotPos[x] = enemySlotPosTemp[x];
            }
            else
            {
                enemySlotPos[x].transform.position = enemySlotPosTemp[x].transform.position;
            }
        }

    }

    public void PlayCard()
    {

        if(player.GetComponent<Player>().deckOrdered.Count > 0)
        {
            card = player.GetComponent<Player>().deckOrdered[player.GetComponent<Player>().deckOrdered.Count - 1];
            card.transform.position = playerSpellPos.transform.position;
            player.GetComponent<Player>().deckOrdered.RemoveAt(player.GetComponent<Player>().deckOrdered.Count - 1);

            enemyCard = enemyDeckOrder[enemyDeckOrder.Count - 1];
            enemyCard.transform.position = enemySpellPos.transform.position;
            enemyDeckOrder.RemoveAt(enemyDeckOrder.Count - 1);

            CheckSpell(1, enemyCard);
            CheckSpell(1, card);   
        }
        else
        {
            card = null;
            enemyCard = null;
        }
    }

    public void BackToPickACard()
    {
        Scene1.SetActive(true);
        Scene3.SetActive(false);

        StopAllCoroutines();
        gameManager.GetComponent<GameState>().gameState = 0;
        gameManager.GetComponent<PickACard>().OnSceneLoad();

        foreach(GameObject card in player.GetComponent<Player>().deck)
        {
            card.GetComponent<Card>().CheckCardID();
        }

        foreach (GameObject card in enemy.GetComponent<Enemy>().deck)
        {
            card.GetComponent<Card>().CheckCardID();
        }

        int temp = playerSlotPos.Count;
        for (int c = 0; c < temp; c++)
        {
            if (playerSlotPos[0].GetComponent<Card>() != null)
            {
                playerSlotPos[0].SetActive(false);
                playerSlotPos.Remove(playerSlotPos[0]);
             
            }
        }

        temp = enemySlotPos.Count;
        for (int z = 0; z < temp; z++)
        {
            if (enemySlotPos[0].GetComponent<Card>() != null)
            {
                enemySlotPos[0].SetActive(false);
                enemySlotPos.Remove(enemySlotPos[0]); 
            }
        }


    }

    public void CheckSpell(int id, GameObject playerID)
    {
        if (id == 3)
        {
            if (playerID.GetComponent<Card>().playerID == 1)
            {
                if (playerSlotPos[0].GetComponent<Card>() is null)
                {
                    if (enemySlotPos[0].GetComponent<Card>() != null)
                    {
                        enemySlotPos[0].GetComponent<Card>().defence -= 1;
                    }
                    else
                    {
                        //Debug.Log("Not hitting anything");
                    }
                }
                else
                {
                    playerSlotPos[0].GetComponent<Card>().defence += 1;
                    //Debug.Log("Not hitting anything again");
                }
            }
            else if (playerID.GetComponent<Card>().playerID == 2)
            {
                if (enemySlotPos[0].GetComponent<Card>() is null)
                {
                    if (playerSlotPos[0].GetComponent<Card>() != null)
                    {
                        playerSlotPos[0].GetComponent<Card>().defence -= 1;
                    }
                    else
                    {
                        //Debug.Log("Not hitting anything");
                    }
                }
                else
                {
                    enemySlotPos[0].GetComponent<Card>().defence += 1;
                    //Debug.Log("Not hitting anything again");
                }
            }

        }
        else if (id == 4)
        {
            if (playerID.GetComponent<Card>().playerID == 1)
            {
                if (playerSlotPos[0].GetComponent<Card>() is null)
                {
                    GameObject temp = Instantiate(cardPrefab, playerSlotPos[playerMosterSlot].transform.position, transform.rotation);
                    temp.transform.parent = playerDeckPos.transform;
                    temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    playerSlotPos[playerMosterSlot] = temp;
                    playerSlotPos[playerMosterSlot].GetComponent<Card>().setCardId(99);
                    //Debug.Log("SPawning in card");
                }
                else
                {
                    if (enemySlotPos[0].GetComponent<Card>() != null)
                    {
                        enemySlotPos[0].GetComponent<Card>().defence -= 2;
                    }
                    else
                    {
                        //Debug.Log("Not hitting anything");
                    }
                }

            }

            if (playerID.GetComponent<Card>().playerID == 2)
            {
                if (enemySlotPos[0].GetComponent<Card>() is null)
                {
                    GameObject temp = Instantiate(cardPrefab, enemySlotPos[enemyMosterSlot].transform.position, transform.rotation);
                    temp.transform.parent = playerDeckPos.transform;
                    temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    enemySlotPos[enemyMosterSlot] = temp;
                    enemySlotPos[enemyMosterSlot].GetComponent<Card>().setCardId(99);
                    //Debug.Log("SPawning in card");
                }
                else
                {
                    if (playerSlotPos[0].GetComponent<Card>() != null)
                    {
                        playerSlotPos[0].GetComponent<Card>().defence -= 2;
                    }
                    else
                    {
                        //Debug.Log("Not hitting anything");
                    }
                }

            }
        }
    }

    public void OnStart()
    {
        for(int x = 0; x < 5; x++)
        {
            if(playerSlotPos.Count > 0)
            {
                playerSlotPos.RemoveAt(0);
            }
           
        }

        for (int b = 0; b < 5; b++)
        {
            if (enemySlotPos.Count > 0)
            {
                enemySlotPos.RemoveAt(0);
            }
        }

        for (int c = 0; c < playerSlotPosTemp.Count; c++)
        {
            playerSlotPos.Add(playerSlotPosTemp[c]);
        }

        for (int s = 0; s < enemySlotPosTemp.Count; s++)
        {
            enemySlotPos.Add(enemySlotPosTemp[s]);
        }

    }



}
