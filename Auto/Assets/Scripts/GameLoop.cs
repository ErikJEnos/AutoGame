using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

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

    private void Update()
    {
        if (gameManager.GetComponent<GameState>().gameState == 2)
        {
            if(enemyDeckOrder.Count <= 5)
            {
                foreach (GameObject cards in enemy.GetComponent<Player>().deckOrdered)
                {
                    enemyDeckOrder.Insert(0,cards);
                }
            }
            else
            {
                CheckLevel();
                gameManager.GetComponent<GameState>().gameState = 3;
                StartCoroutine(GamePlayLoop(gameSpeed));
            }
        }
    }

    public IEnumerator GamePlayLoop(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);

        CheckWinner();

        PhaseText.text = "Phase: Start";
        
        if (player.GetComponent<Player>().deckOrdered.Count > 0 || enemy.GetComponent<Player>().deckOrdered.Count > 0)
        {
            yield return new WaitForSeconds(waitTime);
            CheckSlot();


            yield return new WaitForSeconds(waitTime);
            PlayCard();
            PhaseText.text = "Phase: Show card";


            yield return new WaitForSeconds(waitTime);
            SetCard();
            PhaseText.text = "Phase: Setcard";

            CheckMonsters();

        }


        yield return new WaitForSeconds(waitTime);
        MonsterAttack();
        PhaseText.text = "Phase: Fight";

        yield return new WaitForSeconds(waitTime);
        CheckPoisonedDebuff();

        yield return new WaitForSeconds(waitTime);
        CardCleanUp();
        PhaseText.text = "Phase: End turn";

        yield return new WaitForSeconds(waitTime);

        MoveCards();

        isPaused = false;

        StartCoroutine(GamePlayLoop(gameSpeed));

    }

    private void CheckWinner()
    {

        if(player.GetComponent<Player>().deckOrdered.Count <= 0 && enemy.GetComponent<Player>().deckOrdered.Count <= 0 && hasWon == false)
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

            if (playerCount == 0)
            {
                hasWon = true;
                BackToPickACard();
                gameManager.GetComponent<VersusModeGameLoop>().done = false;
                gameManager.GetComponent<VersusModeGameLoop>().checkedPl = false;
                gameManager.GetComponent<PickingCards>().OnSceneLoad();
                gameLogic.GetComponent<GameLogic>().playerWins+=1;
                gameLogic.GetComponent<GameLogic>().playerTurn += 1;
                EmptyDeckorder();

            }
            else if (enemyCount == 0)
            {
                hasWon = true;
                BackToPickACard();
                gameManager.GetComponent<VersusModeGameLoop>().done = false;
                gameManager.GetComponent<VersusModeGameLoop>().checkedPl = false;
                gameManager.GetComponent<PickingCards>().OnSceneLoad();
                gameLogic.GetComponent<GameLogic>().playerloses-=1;
                gameLogic.GetComponent<GameLogic>().playerTurn += 1;
                EmptyDeckorder();
            }
            
        }



    }

    private void CheckSlot()
    {
        playerMosterSlot = 0;
        enemyMosterSlot = 0;

        for (int x = 0; x < playerSlotPos.Count; x++)
        {
            if (playerSlotPos[x].tag == "Card")
            {
                playerMosterSlot = x + 1;
            }
        }

        for (int x = 0; x < enemySlotPos.Count; x++)
        {
            if (enemySlotPos[x].tag == "Card")
            {
                enemyMosterSlot = x + 1;
            }
        }
        
    }

    private void SetCard()
    {
        if(card != null && enemyCard != null)
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

                playerSlotPos[0].GetComponent<Card>().isPoisoned = enemySlotPos[0].GetComponent<Card>().canPoison;
                enemySlotPos[0].GetComponent<Card>().isPoisoned = playerSlotPos[0].GetComponent<Card>().canPoison;


                playerSlotPos[0].GetComponent<Card>().damageInfoText.color = Color.red;
                playerSlotPos[0].GetComponent<Card>().damageInfoText.text = "-" + enemySlotPos[0].GetComponent<Card>().attack;
                
                enemySlotPos[0].GetComponent<Card>().damageInfoText.color = Color.red;
                enemySlotPos[0].GetComponent<Card>().damageInfoText.text = "-" + playerSlotPos[0].GetComponent<Card>().attack;
            }
        }
    }

    public void DestroyMonster(GameObject card) 
    {
    
        card.SetActive(false);
        playerSlotPos.Remove(card);
     
    }

    private void CardCleanUp()
    {

        if (playerSlotPos[0].GetComponent<Card>() != null && enemySlotPos[0].GetComponent<Card>() != null)
        {
            playerSlotPos[0].GetComponent<Card>().damageInfoText.text = "";
            enemySlotPos[0].GetComponent<Card>().damageInfoText.text = "";


            if (playerSlotPos[0].GetComponent<Card>().defence <= 0 && enemySlotPos[0].GetComponent<Card>().defence <= 0)
            {
                OnDeath(playerSlotPos[0].GetComponent<Card>().cardID, playerSlotPos[0]);
                OnDeath(enemySlotPos[0].GetComponent<Card>().cardID, enemySlotPos[0]);

                playerSlotPos[0].SetActive(false);
                playerSlotPos.Remove(playerSlotPos[0]);

                enemySlotPos[0].SetActive(false);
                enemySlotPos.Remove(enemySlotPos[0]);

            }
            else if (playerSlotPos[0].GetComponent<Card>().defence <= 0 && enemySlotPos[0].GetComponent<Card>().defence > 0)
            {
                OnDeath(playerSlotPos[0].GetComponent<Card>().cardID, playerSlotPos[0]);
                OnKill(enemySlotPos[0]);
                Debug.Log("enemy card on kill");

                playerSlotPos[0].SetActive(false);
                playerSlotPos.Remove(playerSlotPos[0]);
            }
            else if (playerSlotPos[0].GetComponent<Card>().defence > 0 && enemySlotPos[0].GetComponent<Card>().defence <= 0)
            {
                OnDeath(enemySlotPos[0].GetComponent<Card>().cardID, enemySlotPos[0]);
                OnKill(playerSlotPos[0]);

                Debug.Log("Player card on kill");
                enemySlotPos[0].SetActive(false);
                enemySlotPos.Remove(enemySlotPos[0]);
            }

        }
    }

    public void MoveCards()
    {
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
        }
        else
        {
            card = null;
        }

        if(enemy.GetComponent<Player>().deckOrdered.Count > 0)
        {
            enemyCard = enemy.GetComponent<Player>().deckOrdered[enemy.GetComponent<Player>().deckOrdered.Count - 1];
            enemyCard.transform.position = enemySpellPos.transform.position;
            enemy.GetComponent<Player>().deckOrdered.RemoveAt(enemy.GetComponent<Player>().deckOrdered.Count - 1);
        }
        else
        {
            
            enemyCard = null;
        }
    }

    public void BackToPickACard()
    {
        Scene1.SetActive(true);
        Scene3.SetActive(false);

        StopAllCoroutines();
        gameManager.GetComponent<GameState>().gameState = 0;
        
        foreach(GameObject card in player.GetComponent<Player>().deck)
        {
            card.GetComponent<Card>().CheckCardID();
        }

        foreach (GameObject card in enemy.GetComponent<Player>().deck)
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
        temp = playerDeckOrder.Count;
        for(int c = 0; c < temp; c++)
        {
            playerDeckOrder.RemoveAt(0);
        }

        temp = enemyDeckOrder.Count;
        for (int c = 0; c < temp; c++)
        {
            enemyDeckOrder.RemoveAt(0);
        }

    }

    public void CheckSpell(int id, GameObject playerID)
    {
        if (id == 9)
        {
            Debug.Log("Checking spell: " + playerID.GetComponent<Card>().playerID);

            if (playerID.GetComponent<Card>().playerID == player.GetComponent<Player>().id)
            {
                if (playerSlotPos[0].GetComponent<Card>() is null)
                {
                    if (enemySlotPos[0].GetComponent<Card>() != null && enemySlotPos[0].GetComponent<Card>().canTarget)
                    {
                        enemySlotPos[0].GetComponent<Card>().defence -= 1;
                        enemySlotPos[0].GetComponent<Card>().damageInfoText.color = Color.red;
                        enemySlotPos[0].GetComponent<Card>().damageInfoText.text = "-" + 1;
                    }
                    else
                    {
                        Debug.Log("Not hitting anything");
                    }
                }
                else
                {
                    playerSlotPos[0].GetComponent<Card>().defence += 1;
                    Debug.Log("Not hitting anything again");
                }
            }
            if(playerID.GetComponent<Card>().playerID == enemy.GetComponent<Player>().id)
            {
                Debug.Log("Checking spell: " + playerID.GetComponent<Card>().playerID);
                if (enemySlotPos[0].GetComponent<Card>() is null)
                {
                    if (playerSlotPos[0].GetComponent<Card>() != null && playerSlotPos[0].GetComponent<Card>().canTarget)
                    {
                        playerSlotPos[0].GetComponent<Card>().defence -= 1;
                        playerSlotPos[0].GetComponent<Card>().damageInfoText.color = Color.red;
                        playerSlotPos[0].GetComponent<Card>().damageInfoText.text = "-" + 1;
                    }
                    else
                    {
                        Debug.Log("Not hitting anything");
                    }
                }
                else
                {
                    enemySlotPos[0].GetComponent<Card>().defence += 1;
                    enemySlotPos[0].GetComponent<Card>().damageInfoText.color = Color.green;
                    enemySlotPos[0].GetComponent<Card>().damageInfoText.text = "+" + 1;
                    Debug.Log("Not hitting anything again");
                }
            }

        }
        else if (id == 10)
        {
            if (playerID.GetComponent<Card>().playerID == player.GetComponent<Player>().id)
            {
                if (playerSlotPos[0].GetComponent<Card>() is null)
                {
                    GameObject temp = Instantiate(cardPrefab, playerSlotPos[playerMosterSlot].transform.position, transform.rotation);
                    temp.transform.parent = playerDeckPos.transform;
                    temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    playerSlotPos[playerMosterSlot] = temp;
                    playerSlotPos[playerMosterSlot].GetComponent<Card>().setCardId(99);
                    Debug.Log("SPawning in card");
                }
                else
                {
                    if (enemySlotPos[0].GetComponent<Card>() != null && enemySlotPos[0].GetComponent<Card>().canTarget)
                    {
                        enemySlotPos[0].GetComponent<Card>().defence -= 2;
                        enemySlotPos[0].GetComponent<Card>().damageInfoText.color = Color.red;
                        enemySlotPos[0].GetComponent<Card>().damageInfoText.text = "-" + 2;
                    }
                    else
                    {
                        Debug.Log("Not hitting anything");
                    }
                }

            }
            if(playerID.GetComponent<Card>().playerID == enemy.GetComponent<Player>().id)
            {
                Debug.Log("Checking spell: " + playerID.GetComponent<Card>().playerID);
                if (enemySlotPos[0].GetComponent<Card>() is null)
                {
                    GameObject temp = Instantiate(cardPrefab, enemySlotPos[enemyMosterSlot].transform.position, transform.rotation);
                    temp.transform.parent = playerDeckPos.transform;
                    temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    enemySlotPos[enemyMosterSlot] = temp;
                    enemySlotPos[enemyMosterSlot].GetComponent<Card>().setCardId(99);
                    Debug.Log("SPawning in card");
                }
                else
                {
                    if (playerSlotPos[0].GetComponent<Card>() != null && playerSlotPos[0].GetComponent<Card>().canTarget)
                    {
                        playerSlotPos[0].GetComponent<Card>().defence -= 2;
                        playerSlotPos[0].GetComponent<Card>().damageInfoText.color = Color.red;
                        playerSlotPos[0].GetComponent<Card>().damageInfoText.text = "-" + 2;
                    }
                    else
                    {
                        Debug.Log("Not hitting anything");
                    }
                }
            }
        }
    }

    public void CheckPoisonedDebuff()
    {
        
        foreach (GameObject card in playerSlotPos)
        {
            if (card.GetComponent<Card>() != null)
            {
                if (card.GetComponent<Card>().isPoisoned)
                {
                    card.GetComponent<Card>().defence -= 1;
                    card.GetComponent<Card>().damageInfoText.color = Color.blue;
                    card.GetComponent<Card>().damageInfoText.text = "-" + 1;
                }
            }
        }
        

      
        foreach (GameObject card in enemySlotPos)
        {
            if(card.GetComponent<Card>() != null)
            {
                if (card.GetComponent<Card>().isPoisoned)
                {
                    card.GetComponent<Card>().defence -= 1;
                    card.GetComponent<Card>().damageInfoText.color = Color.blue;
                    card.GetComponent<Card>().damageInfoText.text = "-" + 1;
                }
            }
        }
        
    }

    public void CheckMonsters()
    {
        int goblinCount = 0;
        int humanCount = 0;

        foreach (GameObject card in enemySlotPos)
        {
            if (card.GetComponent<Card>() != null)
            {
                if (card.GetComponent<Card>().cardID == 3)
                {
                    card.GetComponent<Card>().defence = card.GetComponent<Card>().defence += goblinCount;
                }

                if (card.GetComponent<Card>().cardID == 5)
                {
                    card.GetComponent<Card>().defence = card.GetComponent<Card>().defence += humanCount;
                }
            }
        }

        foreach (GameObject card in enemySlotPos)
        {
            if (card.GetComponent<Card>() != null)
            {
                if (card.GetComponent<Card>().cardID == 3)
                {
                    card.GetComponent<Card>().defence = card.GetComponent<Card>().defence += goblinCount;
                }

                if (card.GetComponent<Card>().cardID == 5)
                {
                    card.GetComponent<Card>().defence = card.GetComponent<Card>().defence += humanCount;
                }
            }
        }


        goblinCount = 0;
        humanCount = 0;

        foreach (GameObject card in playerSlotPos)
        {
            if (card.GetComponent<Card>() != null)
            {
                if (card.GetComponent<Card>().cardID == 3)
                {
                    goblinCount++;
                }

                if (card.GetComponent<Card>().cardID == 5)
                {
                    humanCount++;
                }
            }
        }

        foreach (GameObject card in playerSlotPos)
        {
            if (card.GetComponent<Card>() != null)
            {
                if (card.GetComponent<Card>().cardID == 3)
                {
                    card.GetComponent<Card>().defence = goblinCount;
                }

                if (card.GetComponent<Card>().cardID == 5)
                {
                    card.GetComponent<Card>().defence = humanCount;
                }
            }
        }
    }

    public void OnStart()
    {
        gameManager.GetComponent<PickingCards>().ChangeScene(2);

        gameManager.GetComponent<VersusModeGameLoop>().SetUpPlayer1Deck();


        enemy = GameObject.Find("Player2");

        for (int x = 0; x < 5; x++)
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


        foreach(GameObject card in player.GetComponent<Player>().deckOrdered)
        {
            playerDeckOrder.Insert(0,card);
        }

        gameManager.GetComponent<GameState>().gameState = 2;

    }

    public bool OnDeath(int id, GameObject playerID)
    {
        if (id == 0)
        {
            if (playerID.GetComponent<Card>().playerID == player.GetComponent<Player>().id)
            {
                GameObject temp = Instantiate(cardPrefab, playerSlotPos[0].transform.position, transform.rotation);
                temp.transform.parent = playerDeckPos.transform;
                temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                playerSlotPos.Insert(1,temp);
                playerSlotPos[1].GetComponent<Card>().setCardId(98);
                Debug.Log("SPawning in card");
            }
            
            if(playerID.GetComponent<Card>().playerID == enemy.GetComponent<Player>().id)
            { 
                GameObject temp = Instantiate(cardPrefab, enemySlotPos[0].transform.position, transform.rotation);
                temp.transform.parent = enemyDeckPos.transform;
                temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                enemySlotPos.Insert(1, temp);
                enemySlotPos[1].GetComponent<Card>().setCardId(98);
                Debug.Log("SPawning in card");

            }
            return true;
        }
        if (id == 1)
        {
            if (playerID.GetComponent<Card>().playerID == player.GetComponent<Player>().id)
            {
                if (enemySlotPos[enemyMosterSlot] != null)
                {                 
                    enemySlotPos[enemyMosterSlot].GetComponent<Card>().defence -= 1;
                    enemySlotPos[enemyMosterSlot].GetComponent<Card>().damageInfoText.color = Color.red;
                    enemySlotPos[enemyMosterSlot].GetComponent<Card>().damageInfoText.text = "-" + 1;
                }
            }

            if (playerID.GetComponent<Card>().playerID == enemy.GetComponent<Player>().id)
            {
                if(playerSlotPos[playerMosterSlot] != null)
                {             
                    playerSlotPos[playerMosterSlot].GetComponent<Card>().defence -= 1;
                    playerSlotPos[playerMosterSlot].GetComponent<Card>().damageInfoText.color = Color.red;
                    playerSlotPos[playerMosterSlot].GetComponent<Card>().damageInfoText.text = "-" + 1;

                }
            }
        }
        if (id == -1)
        {
            if (playerID.GetComponent<Card>().playerID == player.GetComponent<Player>().id)
            {

            }

            if (playerID.GetComponent<Card>().playerID == enemy.GetComponent<Player>().id)
            {

            }

        }

        if (id == -1)
        {
            if (playerID.GetComponent<Card>().playerID == player.GetComponent<Player>().id)
            {

            }

            if (playerID.GetComponent<Card>().playerID == enemy.GetComponent<Player>().id)
            {

            }
        }

        

        return false;
    }

    public void OnKill(GameObject aliveCard)
    {
        Debug.Log("Alive Card " + aliveCard.GetComponent<Card>().cardID);
        
        if (aliveCard.GetComponent<Card>().cardID == 7)
        {
            Debug.Log("wolves");
            aliveCard.GetComponent<Card>().defence++;
        }
     
    }

    public void CheckLevel()
    {
        foreach(GameObject card in enemyDeckOrder)
        {
            card.GetComponent<Card>().CheckCardID();       
        }
    }

    public void SetPlayerDeckIDs(int id )
    {
        foreach(GameObject card in playerDeckOrder)
        {
            card.GetComponent<Card>().playerID = id;
        }
    }

    public void EmptyDeckorder()
    {
        int deckCount = enemyDeckOrder.Count;

        for(int x = 0; x < deckCount; x++)
        {
            enemyDeckOrder.RemoveAt(0);
        }
    }
}
