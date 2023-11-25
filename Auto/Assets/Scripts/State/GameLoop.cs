using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using MilkShake;

public class GameLoop : MonoBehaviour
{
    public static class CardName
    {

        public const int ZOMBIE = 0;
        public const int SKELETON = 1;
        public const int FLYINGSNAKE= 2;
        public const int TREEIMP= 3;
        public const int GNOME= 4;
        public const int SLIME= 5;
        public const int HUMAN = 6;
        public const int GOBLIN = 7;

        public const int FARMER = 8;
        public const int HOBGOBLIN = 9;
        public const int MIMIC = 10;
        public const int WOLVES = 11;
        public const int GAINTRAT = 12;
        public const int GAINTBAT = 13;
        public const int GHOST = 14;
        public const int FLYINGSWORD = 15;

        public const int VAMPIRE = 16;
        public const int KNIGHT = 17;
        public const int GRAVEDIGGER = 18;
        public const int ORC = 19;
        public const int WEREWOLF = 20;
        public const int GUARD = 21;
        public const int BERSERKER = 22;
        public const int DRUID = 23;

        public const int NECROMANCER = 24;
        public const int NYMPH = 25;
        public const int WITCH = 26;
        public const int WIZARD = 27;
        public const int LICH = 28;
        public const int SHAMAN = 30;
        public const int MUMMY = 31;

    }

    public GameObject player;
    public GameObject enemy;
    public GameObject cardPrefab;

    public GameObject playerDeckPos;
    public GameObject playerSpellPos;

    private GameObject card;
    public int playerMosterSlot = 0;
    public List<GameObject> playerDeckOrder;
    public List<GameObject> playerSlotPos;
    public GameObject playerSideSetCard;
    public List<GameObject> actions;

    public GameObject enemyDeckPos;
    public GameObject enemySpellPos;
    
    private GameObject enemyCard;
    public int enemyMosterSlot = 0;
    public List<GameObject> enemyDeckOrder;
    public List<GameObject> enemySlotPos;
    public GameObject enemySideSetCard;


    public TMP_Text PhaseText;
    private GameObject gameManager;
    private GameObject gameLogic;
    public float gameSpeed;
    public bool hasWon = false;
    
    public GameObject Scene1;
    public GameObject Scene2;
    public GameObject Scene3;
    public GameObject ResultsPanel;

    public bool isPaused = false;
    public bool isAutoPlay = false;
    public bool FastForward = false;

    public Animator anim;

    public int turn = 1;
    public int cardPoolSize = 8;
    bool callonce = true;

    public Shaker Camera;
    public ShakePreset shakePreset;

    public GameObject tempSortObject;

    public TMP_Text winTex;




    // Start is called before the first frame update
    void Start()
    {
        gameSpeed = 0.5f;
        player = GameObject.Find("Player1");
        enemy = GameObject.Find("Player2");
        gameManager = GameObject.Find("GameManager");
        gameLogic = GameObject.Find("NetworkClient");
 

        player.GetComponent<Player>().id = gameLogic.GetComponent<GameLogic>().player1ID;
        enemy.GetComponent<Player>().id = gameLogic.GetComponent<GameLogic>().player2ID;

    }

    private void Update()
    {
        if (gameManager.GetComponent<GameState>().gameState == 2)
        {
            if (enemyDeckOrder.Count < 5)
            {
                foreach (GameObject cards in enemy.GetComponent<Player>().deckOrdered)
                {
                    enemyDeckOrder.Insert(0,cards);
                }
                callonce = true;
            }
            else
            {
                CheckLevel();
                gameManager.GetComponent<GameState>().gameState = 3;
                hasWon = false;

                if (turn % 2 == 0 && callonce)
                {
                    cardPoolSize += 8;
                    callonce = false;
                }

                StartCoroutine(GamePlayLoop(gameSpeed));
            }
        }
    }

    public IEnumerator GamePlayLoop(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        CheckWinner();

        PhaseText.text = "Phase: Start";

        StartOfBattle();

        CheckFilledSlots();

        PlayCard();
        PhaseText.text = "Phase: Show card";


        yield return new WaitForSeconds(waitTime);
            
        SetCard();

        PhaseText.text = "Phase: set card";

        MoveCards();

        //CheckMonsters();

        yield return new WaitForSeconds(waitTime);
      
        MonsterAttack();
        PhaseText.text = "Phase: Fight";


        yield return new WaitForSeconds(waitTime);
        CheckPoisonedDebuff();

        if(playerSlotPos.Count > 0 && enemySlotPos.Count > 0)
        {
            if (playerSlotPos[0].GetComponent<Card>().defence > 0 && enemySlotPos[0].GetComponent<Card>().defence <= 0)
            {
                if (actions.Contains(playerSlotPos[0]))
                {
                    playerSlotPos[0].GetComponent<Card>().killed = true;
                }

            }
            else if (playerSlotPos[0].GetComponent<Card>().defence <= 0 && enemySlotPos[0].GetComponent<Card>().defence > 0)
            {
                if (actions.Contains(enemySlotPos[0]))
                {
                    enemySlotPos[0].GetComponent<Card>().killed = true;
                }
            }
        }
  
        yield return new WaitForSeconds(waitTime);

        bool testing = true;

        do {
            CardCleanUp();
            if(actions.Count > 0)
            {
                for (int c = 0; c < actions.Count; c++)
                {
                    if (actions[c].GetComponent<Card>() != null)
                    {
                        if (actions[c].GetComponent<Card>().defence <= 0)
                        {
                            actions[c].GetComponent<Card>().OnDeathTrigger();
                        }
                        if (actions[c].GetComponent<Card>().killed)
                        {
                            actions[c].GetComponent<Card>().OnKillTrigger();
                        }
                        if (actions[c].GetComponent<Card>().hurt && actions[c].GetComponent<Card>().defence > 0)
                        {
                            actions[c].GetComponent<Card>().OnHurtTrigger();
                        }

                        yield return new WaitForSeconds(waitTime);
                    }

                    if (enemySlotPos.Contains(actions[c]) && actions[c].GetComponent<Card>().defence <= 0)
                    {
                        enemySlotPos.Remove(actions[c]);
                        actions[c].SetActive(false);
                    }
                    else if (playerSlotPos.Contains(actions[c]) && actions[c].GetComponent<Card>().defence <= 0)
                    {
                        playerSlotPos.Remove(actions[c]);
                        actions[c].SetActive(false);
                    }
                }
         
            }
            
          

            if (actions.Count > 0)
            {
                int actionCount = actions.Count;
                for (int v = 0; v < actionCount; v++)
                {
                    actions.RemoveAt(0);
                }

                testing = true;
            }
            else
            {
                int actionCount = actions.Count;
                for (int v = 0; v < actionCount; v++)
                {
                    actions.RemoveAt(0);
                }

                testing = false;
            }
        } while (testing);

        PhaseText.text = "Phase: End turn";

        yield return new WaitForSeconds(waitTime);
        MoveCards();

        isPaused = false;

        EndStepEffects();

        StartCoroutine(GamePlayLoop(gameSpeed));

    }
   
    public void StartOfBattle()
    {
        for(int x = 0; x < playerSlotPos.Count; x++)
        {
            playerSlotPos[x].GetComponent<Card>().StartOfTurn();
        }

        for (int x = 0; x < enemySlotPos.Count; x++)
        {
            enemySlotPos[x].GetComponent<Card>().StartOfTurn();
        }

    }

    public void toggleAnimResultsScreen()
    {
        anim.SetBool("Start", true);

    }
    
    private void CheckWinner()
    {
        if(player.GetComponent<Player>().deckOrdered.Count <= 0 && enemy.GetComponent<Player>().deckOrdered.Count <= 0 && hasWon == false)
        {

            if (playerSlotPos.Count == 0 && enemySlotPos.Count == 0)
            {
                StopCoroutine(GamePlayLoop(0));
                toggleAnimResultsScreen();
                hasWon = true;
                StopCoroutine(GamePlayLoop(0));
                Invoke("BackToPickACard", 5.0f);
                EmptyDeckorder();

                winTex.text = "Tie";
            }

            else if (playerSlotPos.Count == 0 && enemySlotPos.Count > 0)
            {
                StopCoroutine(GamePlayLoop(0));
                toggleAnimResultsScreen();
                hasWon = true;


                gameManager.GetComponent<InGameUI>().playerloses -= 1;

                if (gameManager.GetComponent<InGameUI>().playerloses <= 0)
                {
                    winTex.text = "You Lost the battle ";
                    Invoke("BackToPickACard", 5.0f);
                }
                else
                {
                    winTex.text = "You Lose";
                    Invoke("BackToPickACard", 5.0f);
                    
                }
                
            }

            else if (enemySlotPos.Count == 0 && playerSlotPos.Count > 0)
            {
                StopCoroutine(GamePlayLoop(0));
                toggleAnimResultsScreen();
                hasWon = true;


                gameManager.GetComponent<InGameUI>().playerWins += 1;

                if (gameManager.GetComponent<InGameUI>().playerWins >= 10)
                {
                    winTex.text = "You won the battle ";
                    Invoke("BackToPickACard", 5.0f);
                }
                else
                {
                    winTex.text = "You Win";
                    Invoke("BackToPickACard", 5.0f);

                }
            }
        }
    }

    public void CheckFilledSlots()
    {
        playerMosterSlot = 0;
        enemyMosterSlot = 0;

        for (int x = 0; x < playerSlotPos.Count; x++)
        {
            if (playerSlotPos[x].tag == "Card")
            {
                playerMosterSlot = playerMosterSlot + 1;
            }
        }

        for (int x = 0; x < enemySlotPos.Count; x++)
        {
            if (enemySlotPos[x].tag == "Card")
            {
                enemyMosterSlot = enemyMosterSlot + 1;
            }
        }
    }

    private void SetCard()
    {
        GameObject temp = null;
        if(enemyCard != null && card != null)
        {
            enemySlotPos.Add(enemyCard);
            enemyCard.GetComponent<Card>().OnSetTrigger();

            playerSlotPos.Add(card);
            card.GetComponent<Card>().OnSetTrigger();
        }
        else if(enemyCard != null)
        {
            enemySlotPos.Add(enemyCard);
            enemyCard.GetComponent<Card>().OnSetTrigger();
        }
        else if(card != null)
        {
            playerSlotPos.Add(card);
            card.GetComponent<Card>().OnSetTrigger();
        }
      

        if (enemyCard != null)
        {
            if (enemyCard.GetComponent<Card>().isMonster)
            {
                if (enemyCard.GetComponent<Card>().cardID == CardName.GRAVEDIGGER)
                {
                    temp = Instantiate(enemyCard, gameObject.transform.position, transform.rotation);
                    temp.transform.parent = gameManager.GetComponent<GameLoop>().playerDeckPos.transform;
                    temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    temp.GetComponent<Card>().setCardId(0);
                    temp.GetComponent<Card>().playerID = enemyCard.GetComponent<Card>().playerID;
                    gameManager.GetComponent<GameLoop>().enemySlotPos.Add(temp);
                }
                else if (enemyCard.GetComponent<Card>().cardID == CardName.MIMIC)
                {
                    if (playerSlotPos.Count == 0)
                    {
                        enemyCard.GetComponent<Card>().attack = card.GetComponent<Card>().defence;
                    }
                    else
                    {
                        enemyCard.GetComponent<Card>().attack = playerSlotPos[0].GetComponent<Card>().defence;
                    }
                }
                else if (enemyCard.GetComponent<Card>().cardID == CardName.GUARD && gameManager.GetComponent<GameLoop>().enemySlotPos.Contains(enemyCard))
                {
                    gameManager.GetComponent<GameLoop>().enemySlotPos.Remove(enemyCard);
                    gameManager.GetComponent<GameLoop>().enemySlotPos.Insert(0, enemyCard);
                }
                else if (enemyCard.GetComponent<Card>().cardID == CardName.DRUID)
                {

                    temp = Instantiate(cardPrefab, gameObject.transform.position, transform.rotation);
                    temp.transform.parent = gameManager.GetComponent<GameLoop>().enemyDeckPos.transform;
                    temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    temp.GetComponent<Card>().setCardId(3);
                    temp.GetComponent<Card>().playerID = enemyCard.GetComponent<Card>().playerID;
                    gameManager.GetComponent<GameLoop>().enemySlotPos.Add(temp);

                    temp = Instantiate(cardPrefab, gameObject.transform.position, transform.rotation);
                    temp.transform.parent = gameManager.GetComponent<GameLoop>().enemyDeckPos.transform;
                    temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    temp.GetComponent<Card>().setCardId(3);
                    temp.GetComponent<Card>().playerID = enemyCard.GetComponent<Card>().playerID;
                    gameManager.GetComponent<GameLoop>().enemySlotPos.Add(temp);

                }
                else if (enemyCard.GetComponent<Card>().cardID == CardName.BERSERKER && gameManager.GetComponent<GameLoop>().enemySlotPos.Contains(enemyCard))
                {
                    gameManager.GetComponent<GameLoop>().enemySlotPos.Remove(enemyCard);
                    gameManager.GetComponent<GameLoop>().enemySlotPos.Insert(0, enemyCard);
                }
                else if (enemyCard.GetComponent<Card>().cardID == CardName.FLYINGSWORD)
                {
                    gameManager.GetComponent<GameLoop>().enemySlotPos[0].GetComponent<Card>().attack += 2;
                    gameManager.GetComponent<GameLoop>().enemySlotPos[0].GetComponent<Card>().defence += 1;
                    gameManager.GetComponent<GameLoop>().enemySlotPos[0].GetComponent<CardUI>().damageInfoText.color = Color.green;
                    gameManager.GetComponent<GameLoop>().enemySlotPos[0].GetComponent<CardUI>().damageInfoText.text = "+" + 2 + "+" + 1;

                }
                else if (enemyCard.GetComponent<Card>().cardID == CardName.HUMAN)
                {
                    foreach (GameObject card in enemySlotPos)
                    {
                        if (card.GetComponent<Card>().cardID == CardName.HUMAN)
                        {
                            card.GetComponent<Card>().defence += 1;
                            card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                            card.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 0 + " +" + 1;
                        }
                        else if (card.GetComponent<Card>().cardID == CardName.GRAVEDIGGER)
                        {
                            card.GetComponent<Card>().defence += 1;
                            card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                            card.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 0 + " +" + 1;
                        }
                        else if (card.GetComponent<Card>().cardID == CardName.FARMER)
                        {
                            card.GetComponent<Card>().defence += 1;
                            card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                            card.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 0 + " +" + 1;
                        }
                        else if (card.GetComponent<Card>().cardID == CardName.MIMIC)
                        {
                            card.GetComponent<Card>().defence += 1;
                            card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                            card.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 0 + " +" + 1;
                        }
                    }
                }
                else if (enemyCard.GetComponent<Card>().cardID == CardName.GOBLIN)
                {
                    if (playerSlotPos.Count >= 1)
                    {
                        playerSlotPos[0].GetComponent<Card>().defence -= 1;
                        playerSlotPos[0].GetComponent<Card>().cardUIScript.damageInfoText.color = Color.white;
                        playerSlotPos[0].GetComponent<Card>().cardUIScript.damageInfoText.text = "-" + 1;
                    }

                }
                else if (enemyCard.GetComponent<Card>().cardID == CardName.HOBGOBLIN)
                {
                    if (playerSlotPos.Count >= 1)
                    {
                        playerSlotPos[playerSlotPos.Count - 1].GetComponent<Card>().defence -= enemyCard.GetComponent<Card>().defence;
                        playerSlotPos[playerSlotPos.Count - 1].GetComponent<Card>().cardUIScript.damageInfoText.color = Color.white;
                        playerSlotPos[playerSlotPos.Count - 1].GetComponent<Card>().cardUIScript.damageInfoText.text = "-" + enemyCard.GetComponent<Card>().defence;
                    }
                }
                else if (enemyCard.GetComponent<Card>().cardID == CardName.SHAMAN)
                {
                    foreach (GameObject playerCard in enemySlotPos)
                    {
                        playerCard.GetComponent<Card>().defence += 4;
                        playerCard.GetComponent<CardUI>().damageInfoText.color = Color.green;
                        playerCard.GetComponent<CardUI>().damageInfoText.text = "+" + 0 + "+" + 4;
                    }
                }
                else if (enemyCard.GetComponent<Card>().cardID == CardName.MUMMY)
                {
                    Debug.Log("playerSlotPos.Count: " + playerSlotPos.Count);
                    if (playerSlotPos.Count > 0)
                    {
                        GameObject weakestCard = playerSlotPos[0];

                        foreach (GameObject playerCard in playerSlotPos)
                        {
                            if (playerCard.GetComponent<Card>().defence < weakestCard.GetComponent<Card>().defence)
                            {
                                weakestCard = playerCard;
                            }
                        }

                        playerSlotPos.Remove(weakestCard);
                        playerSlotPos.Insert(0, weakestCard);

                    }
                }
            }
        }
        if (card != null)
        {
            if (card.GetComponent<Card>().isMonster)
            {
                if (card.GetComponent<Card>().cardID == CardName.GRAVEDIGGER)
                {
                    temp = Instantiate(card, gameObject.transform.position, transform.rotation);
                    temp.transform.parent = gameManager.GetComponent<GameLoop>().playerDeckPos.transform;
                    temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    temp.GetComponent<Card>().setCardId(0);
                    temp.GetComponent<Card>().playerID = card.GetComponent<Card>().playerID;
                    gameManager.GetComponent<GameLoop>().playerSlotPos.Add(temp);
                }
                else if (card.GetComponent<Card>().cardID == CardName.MIMIC)
                {
                    if (playerSlotPos.Count == 0)
                    {
                        card.GetComponent<Card>().attack = enemyCard.GetComponent<Card>().defence;
                    }
                    else
                    {
                        card.GetComponent<Card>().attack = enemySlotPos[0].GetComponent<Card>().defence;
                    }
                    
                }
                else if (card.GetComponent<Card>().cardID == CardName.GUARD && gameManager.GetComponent<GameLoop>().playerSlotPos.Contains(card))
                {
                    gameManager.GetComponent<GameLoop>().playerSlotPos.Remove(card);
                    gameManager.GetComponent<GameLoop>().playerSlotPos.Insert(0, card);
                }
                else if (card.GetComponent<Card>().cardID == CardName.DRUID)
                {
                    temp = Instantiate(cardPrefab, gameObject.transform.position, transform.rotation);
                    temp.transform.parent = gameManager.GetComponent<GameLoop>().playerDeckPos.transform;
                    temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    temp.GetComponent<Card>().setCardId(3);
                    temp.GetComponent<Card>().playerID = card.GetComponent<Card>().playerID;
                    gameManager.GetComponent<GameLoop>().playerSlotPos.Add(temp);

                    temp = Instantiate(cardPrefab, gameObject.transform.position, transform.rotation);
                    temp.transform.parent = gameManager.GetComponent<GameLoop>().playerDeckPos.transform;
                    temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    temp.GetComponent<Card>().setCardId(3);
                    temp.GetComponent<Card>().playerID = card.GetComponent<Card>().playerID;
                    gameManager.GetComponent<GameLoop>().playerSlotPos.Add(temp);
                }
                else if (card.GetComponent<Card>().cardID == CardName.BERSERKER && gameManager.GetComponent<GameLoop>().playerSlotPos.Contains(card))
                {
                    gameManager.GetComponent<GameLoop>().playerSlotPos.Remove(card);
                    gameManager.GetComponent<GameLoop>().playerSlotPos.Insert(0, card);
                }
                else if (card.GetComponent<Card>().cardID == CardName.FLYINGSWORD)
                {
                    gameManager.GetComponent<GameLoop>().playerSlotPos[0].GetComponent<Card>().attack += 2;
                    gameManager.GetComponent<GameLoop>().playerSlotPos[0].GetComponent<Card>().defence += 1;
                    gameManager.GetComponent<GameLoop>().playerSlotPos[0].GetComponent<CardUI>().damageInfoText.color = Color.green;
                    gameManager.GetComponent<GameLoop>().playerSlotPos[0].GetComponent<CardUI>().damageInfoText.text = "+" + 2 + "+" + 1;

                }
                else if (card.GetComponent<Card>().cardID == CardName.HUMAN)
                {
                    foreach (GameObject cards in playerSlotPos)
                    {
                        if (cards.GetComponent<Card>().cardID == CardName.HUMAN)
                        {
                            cards.GetComponent<Card>().defence += 1;
                            cards.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                            cards.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 0 + " +" + 1;
                        }
                        else if (cards.GetComponent<Card>().cardID == CardName.GRAVEDIGGER)
                        {
                            cards.GetComponent<Card>().defence += 1;
                            cards.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                            cards.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 0 + " +" + 1;
                        }
                        else if (cards.GetComponent<Card>().cardID == CardName.FARMER)
                        {
                            cards.GetComponent<Card>().defence += 1;
                            cards.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                            cards.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 0 + " +" + 1;
                        }
                        else if (cards.GetComponent<Card>().cardID == CardName.MIMIC)
                        {
                            cards.GetComponent<Card>().defence += 1;
                            cards.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                            cards.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 0 + " +" + 1;
                        }
                    }
                }
                else if (card.GetComponent<Card>().cardID == CardName.GOBLIN)
                {
                    if(enemySlotPos.Count >= 1)
                    {
                        enemySlotPos[0].GetComponent<Card>().defence -= 1;
                        enemySlotPos[0].GetComponent<Card>().cardUIScript.damageInfoText.color = Color.white;
                        enemySlotPos[0].GetComponent<Card>().cardUIScript.damageInfoText.text = "-" + 1;
                    }
                    
                }
                else if (card.GetComponent<Card>().cardID == CardName.HOBGOBLIN)
                {
                    if (enemySlotPos.Count >= 1)
                    {
                        enemySlotPos[enemySlotPos.Count-1].GetComponent<Card>().defence -= card.GetComponent<Card>().defence;
                        enemySlotPos[enemySlotPos.Count-1].GetComponent<Card>().cardUIScript.damageInfoText.color = Color.white;
                        enemySlotPos[enemySlotPos.Count-1].GetComponent<Card>().cardUIScript.damageInfoText.text = "-" + card.GetComponent<Card>().defence;

                    }
                }
                else if (card.GetComponent<Card>().cardID == CardName.SHAMAN)
                {
                    foreach (GameObject playerCard in playerSlotPos)
                    {
                        playerCard.GetComponent<Card>().defence += 4;
                        playerCard.GetComponent<CardUI>().damageInfoText.color = Color.green;
                        playerCard.GetComponent<CardUI>().damageInfoText.text = "+" + 0 + "+" + 4;
                    }
                }
                else if (card.GetComponent<Card>().cardID == CardName.MUMMY)
                {
                    Debug.Log("enemySlotPos.Count: " + enemySlotPos.Count);

                    if(enemySlotPos.Count > 0) 
                    {
                        GameObject weakestCard = enemySlotPos[0];

                        foreach (GameObject playerCard in enemySlotPos)
                        {
                            if (playerCard.GetComponent<Card>().defence < weakestCard.GetComponent<Card>().defence)
                            {
                                weakestCard = playerCard;
                            }
                        }

                        enemySlotPos.Remove(weakestCard);
                        enemySlotPos.Insert(0, weakestCard);

                    }


                }
            }
        }

        AuraBuffs();
    }

    private void MonsterAttack()
    {
        int damageP1 = 0;
        int damageP2 = 0;

        //Camera.Shake(shakePreset);

        if (playerSlotPos.Count > 0 && enemySlotPos.Count > 0)
        {
            if (playerSlotPos[0].GetComponent<Card>().isMonster == true && enemySlotPos[0].GetComponent<Card>().isMonster == true)
            {
                if(playerSlotPos[0].GetComponent<Card>().cardID == CardName.GHOST)
                {
                    damageP1 = enemySlotPos[0].GetComponent<Card>().attack - 1;
                    playerSlotPos[0].GetComponent<Card>().defence -= enemySlotPos[0].GetComponent<Card>().attack - 1;
                }
                else
                {
                    damageP1 = enemySlotPos[0].GetComponent<Card>().attack;
                    playerSlotPos[0].GetComponent<Card>().defence -= enemySlotPos[0].GetComponent<Card>().attack;
                }

               
                if (enemySlotPos[0].GetComponent<Card>().cardID == CardName.GHOST)
                {
                    damageP2 = playerSlotPos[0].GetComponent<Card>().attack - 1;
                    enemySlotPos[0].GetComponent<Card>().defence -= playerSlotPos[0].GetComponent<Card>().attack - 1;
                }
                else
                {
                    damageP2 = playerSlotPos[0].GetComponent<Card>().attack;
                    enemySlotPos[0].GetComponent<Card>().defence -= playerSlotPos[0].GetComponent<Card>().attack;
                }
                

                if(playerSlotPos[0].GetComponent<Card>().isPoisoned == false && playerSlotPos[0].GetComponent<Card>().canTarget)
                {
                    playerSlotPos[0].GetComponent<Card>().isPoisoned = enemySlotPos[0].GetComponent<Card>().canPoison;
                }

                if(enemySlotPos[0].GetComponent<Card>().isPoisoned == false && enemySlotPos[0].GetComponent<Card>().canTarget)
                {
                    enemySlotPos[0].GetComponent<Card>().isPoisoned = playerSlotPos[0].GetComponent<Card>().canPoison;
                }

                if(playerSlotPos[0].GetComponent<Card>().isBleeding == false && playerSlotPos[0].GetComponent<Card>().canTarget)
                {
                    playerSlotPos[0].GetComponent<Card>().isBleeding = enemySlotPos[0].GetComponent<Card>().canBleed;
                }

                if (enemySlotPos[0].GetComponent<Card>().isBleeding == false && enemySlotPos[0].GetComponent<Card>().canTarget)
                {
                    enemySlotPos[0].GetComponent<Card>().isBleeding = playerSlotPos[0].GetComponent<Card>().canBleed;
                }

                playerSlotPos[0].GetComponent<CardUI>().damageInfoText.color = Color.yellow;
                playerSlotPos[0].GetComponent<CardUI>().damageInfoText.text = "-" + damageP1;
                
                enemySlotPos[0].GetComponent<CardUI>().damageInfoText.color = Color.yellow;
                enemySlotPos[0].GetComponent<CardUI>().damageInfoText.text = "-" + damageP2;
            }
        }
    }

    private void CardCleanUp()//check this out 
    {

        for (int x = 0; x < playerSlotPos.Count; x++)
        {
            if (playerSlotPos[x].GetComponent<Card>() != null)
            {
                playerSlotPos[x].GetComponent<Card>().cardUIScript.damageInfoText.text = "";

                if (playerSlotPos[x].GetComponent<Card>().defence <= 0)
                {
                    playerSlotPos[x].GetComponent<Card>().cardPos = x;

                }

            }
        }

        for (int x = 0; x < enemySlotPos.Count; x++)
        {

            if (enemySlotPos[x].GetComponent<Card>() != null)
            {
                enemySlotPos[x].GetComponent<Card>().cardUIScript.damageInfoText.text = "";

                if (enemySlotPos[x].GetComponent<Card>().defence <= 0)
                {
                    enemySlotPos[x].GetComponent<Card>().cardPos = x;
                }
            }
        }

      

        for (int x = 0; x < actions.Count; x++)
        {
            for (int y = 0; y < actions.Count; y++)
            {
                if (actions[x].GetComponent<Card>().attack < actions[y].GetComponent<Card>().attack)
                {
                    tempSortObject = actions[y];
                }
                else if(actions[x].GetComponent<Card>().attack == actions[y].GetComponent<Card>().attack)
                {
                    if (enemy.GetComponent<Player>().id > player.GetComponent<Player>().id)
                    {
                        tempSortObject = actions[y];
                    }
                    else
                    {
                        tempSortObject = actions[y];
                    }
                }
            }
            actions.Remove(tempSortObject);
            actions.Insert(0, tempSortObject);

        }

     
    }

    public void MoveCards()
    {
        float offset = 2.0f;
        for (int x = 0; x < playerSlotPos.Count; x++)
        { 
            playerSlotPos[x].transform.position = new Vector3(playerSideSetCard.transform.position.x - offset * x, 0.0f, 0.0f);
        }

        offset = 2.0f;
        for (int x = 0; x < enemySlotPos.Count; x++)
        {
            enemySlotPos[x].transform.position = new Vector3(enemySideSetCard.transform.position.x + offset * x, 0.0f, 0.0f);
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

    public void ResetAnimationWindow()
    {
        anim.SetBool("Start", false);
    }
    
    public void BackToPickACard()
    {

        Invoke("ResetAnimationWindow", 5.0f);

        if(gameManager.GetComponent<InGameUI>().playerWins >= 10 || gameManager.GetComponent<InGameUI>().playerloses <= 0)
        {
            gameLogic.GetComponent<GameLogic>().SendingPlayerToMenu();
            gameLogic.GetComponent<NetworkedClient>().SendMessageToServer(11 + ",");
        }
       

        gameManager.GetComponent<VersusModeGameLoop>().done = false;
        gameManager.GetComponent<VersusModeGameLoop>().checkedPl = false;
        gameManager.GetComponent<PickingCards>().OnSceneLoad();
        gameManager.GetComponent<InGameUI>().playerTurn += 1;

        turn += 1;

        Scene1.SetActive(true);
        Scene3.SetActive(false);

        StopAllCoroutines();
        gameManager.GetComponent<GameState>().gameState = 0;
        
        foreach(GameObject card in player.GetComponent<Player>().deck)
        {
            card.GetComponent<Card>().CheckCardID();
            card.GetComponent<CardDrag>().inArray = false;
        }

        foreach (GameObject card in enemy.GetComponent<Player>().deck)
        {
            card.GetComponent<Card>().CheckCardID();
            card.GetComponent<CardDrag>().inArray = false;
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

        EmptyDeckorder();

    }

    public void CheckSpell(int id, GameObject playerID)
    {
        if (id == 9)
        {
            if (playerID.GetComponent<Card>().playerID == player.GetComponent<Player>().id)
            {
                if (playerSlotPos[0].GetComponent<Card>() is null)
                {
                    if (enemySlotPos[0].GetComponent<Card>() != null && enemySlotPos[0].GetComponent<Card>().canTarget)
                    {
                        enemySlotPos[0].GetComponent<Card>().defence -= 1;
                        enemySlotPos[0].GetComponent<Card>().cardUIScript.damageInfoText.color = Color.red;
                        enemySlotPos[0].GetComponent<Card>().cardUIScript.damageInfoText.text = "-" + 1;
                    }
                    else
                    {

                    }
                }
                else
                {
                    playerSlotPos[0].GetComponent<Card>().defence += 1;
                }
            }
            if(playerID.GetComponent<Card>().playerID == enemy.GetComponent<Player>().id)
            {
                if (enemySlotPos[0].GetComponent<Card>() is null)
                {
                    if (playerSlotPos[0].GetComponent<Card>() != null && playerSlotPos[0].GetComponent<Card>().canTarget)
                    {
                        playerSlotPos[0].GetComponent<Card>().defence -= 1;
                        playerSlotPos[0].GetComponent<Card>().cardUIScript.damageInfoText.color = Color.red;
                        playerSlotPos[0].GetComponent<Card>().cardUIScript.damageInfoText.text = "-" + 1;
                    }
                    else
                    {
                    }
                }
                else
                {
                    enemySlotPos[0].GetComponent<Card>().defence += 1;
                    enemySlotPos[0].GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                    enemySlotPos[0].GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 1;
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
                }
                else
                {
                    if (enemySlotPos[0].GetComponent<Card>() != null && enemySlotPos[0].GetComponent<Card>().canTarget)
                    {
                        enemySlotPos[0].GetComponent<Card>().defence -= 2;
                        enemySlotPos[0].GetComponent<Card>().cardUIScript.damageInfoText.color = Color.red;
                        enemySlotPos[0].GetComponent<Card>().cardUIScript.damageInfoText.text = "-" + 2;
                    }
                    else
                    {
                    }
                }

            }
            if(playerID.GetComponent<Card>().playerID == enemy.GetComponent<Player>().id)
            {
                if (enemySlotPos[0].GetComponent<Card>() is null)
                {
                    GameObject temp = Instantiate(cardPrefab, enemySlotPos[enemyMosterSlot].transform.position, transform.rotation);
                    temp.transform.parent = playerDeckPos.transform;
                    temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    enemySlotPos[enemyMosterSlot] = temp;
                    enemySlotPos[enemyMosterSlot].GetComponent<Card>().setCardId(99);
                }
                else
                {
                    if (playerSlotPos[0].GetComponent<Card>() != null && playerSlotPos[0].GetComponent<Card>().canTarget)
                    {
                        playerSlotPos[0].GetComponent<Card>().defence -= 2;
                        playerSlotPos[0].GetComponent<Card>().cardUIScript.damageInfoText.color = Color.red;
                        playerSlotPos[0].GetComponent<Card>().cardUIScript.damageInfoText.text = "-" + 2;
                    }
                    else
                    {
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
                    card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.blue;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.text = "-" + 1;
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
                    card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.blue;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.text = "-" + 1;
                }
            }
        }
        
    }

    public void AuraBuffs()
    {
        int knightBuff = 0;
        
        foreach (GameObject card in enemySlotPos)
        {
            if (card.GetComponent<Card>().cardID == CardName.KNIGHT)
            {
                knightBuff++;
            }
        }

        if(knightBuff > 0)
        {
            foreach (GameObject card in enemySlotPos)
            {
                if (card.GetComponent<Card>().cardID == CardName.HUMAN)
                {
                    card.GetComponent<Card>().attack = 3 * knightBuff + card.GetComponent<Card>().OrignalAttack;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 3 * knightBuff + " +" + 0 * knightBuff;
                }
                else if (card.GetComponent<Card>().cardID == CardName.GRAVEDIGGER)
                {
                    card.GetComponent<Card>().attack = 3 * knightBuff + card.GetComponent<Card>().OrignalAttack;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 3 * knightBuff + " +" + 0 * knightBuff;
                }
                else if (card.GetComponent<Card>().cardID == CardName.FARMER)
                {
                    card.GetComponent<Card>().attack = 3 * knightBuff + card.GetComponent<Card>().OrignalAttack;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 3 * knightBuff + " +" + 0 * knightBuff;
                }
                else if (card.GetComponent<Card>().cardID == CardName.MIMIC)
                {
                    card.GetComponent<Card>().attack = 3 * knightBuff + card.GetComponent<Card>().OrignalAttack;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 3 * knightBuff + " +" + 0 * knightBuff;
                }
                else if (card.GetComponent<Card>().cardID == CardName.KNIGHT)
                {
                    card.GetComponent<Card>().attack = 3 * knightBuff + card.GetComponent<Card>().OrignalAttack;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 3 * knightBuff + " +" + 0 * knightBuff;
                }
            }
        }
      

        knightBuff = 0;
        foreach (GameObject card in playerSlotPos)
        {
            if (card.GetComponent<Card>().cardID == CardName.KNIGHT)
            {
                knightBuff++;
            }
        }

        if(knightBuff > 0)
        {
            foreach (GameObject card in playerSlotPos)
            {
                if (card.GetComponent<Card>().cardID == CardName.HUMAN)
                {
                    card.GetComponent<Card>().attack = 3 * knightBuff + card.GetComponent<Card>().OrignalAttack;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 3 * knightBuff + " +" + 0 * knightBuff;
                }
                else if (card.GetComponent<Card>().cardID == CardName.GRAVEDIGGER)
                {
                    card.GetComponent<Card>().attack = 3 * knightBuff + card.GetComponent<Card>().OrignalAttack;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 3 * knightBuff + " +" + 0 * knightBuff;
                }
                else if (card.GetComponent<Card>().cardID == CardName.FARMER)
                {
                    card.GetComponent<Card>().attack = 3 * knightBuff + card.GetComponent<Card>().OrignalAttack;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 3 * knightBuff + " +" + 0 * knightBuff;
                }
                else if (card.GetComponent<Card>().cardID == CardName.MIMIC)
                {
                    card.GetComponent<Card>().attack = 3 * knightBuff + card.GetComponent<Card>().OrignalAttack;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 3 * knightBuff + " +" + 0 * knightBuff;
                }
                else if (card.GetComponent<Card>().cardID == CardName.KNIGHT)
                {
                    card.GetComponent<Card>().attack = 3 * knightBuff + card.GetComponent<Card>().OrignalAttack;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.green;
                    card.GetComponent<Card>().cardUIScript.damageInfoText.text = "+" + 3 * knightBuff + " +" + 0 * knightBuff;
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
                if (card.GetComponent<Card>().cardID == CardName.GOBLIN)
                {
                    goblinCount++;
                }

                else if (card.GetComponent<Card>().cardID == CardName.HOBGOBLIN)
                {
                    goblinCount++;
                }

                else if (card.GetComponent<Card>().cardID == CardName.MIMIC)
                {
                    goblinCount++;
                    humanCount++;
                }

                else if (card.GetComponent<Card>().cardID == CardName.HUMAN)
                {
                    humanCount++;
                }


                else if (card.GetComponent<Card>().cardID == CardName.GRAVEDIGGER)
                {
                    humanCount++;
                }

                else if (card.GetComponent<Card>().cardID == CardName.FARMER)
                {
                    humanCount++;
                }
            }
        }

        foreach (GameObject card in enemySlotPos)
        {
            if (card.GetComponent<Card>() != null)
            {
                if (card.GetComponent<Card>().cardID == CardName.HOBGOBLIN)
                {
                    card.GetComponent<Card>().attack = goblinCount + card.GetComponent<Card>().OrignalAttack;
                    card.GetComponent<Card>().defence = goblinCount + card.GetComponent<Card>().OrignalDefence;
                    CheckBleed(card);
                }
            }
        }


        goblinCount = 0;
        humanCount = 0;

        foreach (GameObject card in playerSlotPos)
        {
            if (card.GetComponent<Card>() != null)
            {
                if (card.GetComponent<Card>().cardID == CardName.GOBLIN)
                {
                    goblinCount++;
                }

                else if (card.GetComponent<Card>().cardID == CardName.HOBGOBLIN)
                {
                    goblinCount++;
                }

                else if (card.GetComponent<Card>().cardID == CardName.MIMIC)
                {
                    goblinCount++;
                    humanCount++;
                }

                else if (card.GetComponent<Card>().cardID == CardName.HUMAN)
                {
                    humanCount++;
                }

                else if (card.GetComponent<Card>().cardID == CardName.GRAVEDIGGER)
                {
                    humanCount++;
                }

                else if (card.GetComponent<Card>().cardID == CardName.FARMER)
                {
                    humanCount++;
                }
            }
        }

        foreach (GameObject card in playerSlotPos)
        {
            if (card.GetComponent<Card>() != null)
            {
                if (card.GetComponent<Card>().cardID == CardName.HOBGOBLIN)
                {
                    card.GetComponent<Card>().attack = goblinCount + card.GetComponent<Card>().OrignalAttack;
                    card.GetComponent<Card>().defence = goblinCount + card.GetComponent<Card>().OrignalDefence;
                    CheckBleed(card);
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


        foreach(GameObject card in player.GetComponent<Player>().deckOrdered)
        {
            playerDeckOrder.Insert(0,card);
        }

        gameManager.GetComponent<GameState>().gameState = 2;

    }

    public void CheckLevel()
    {
        foreach(GameObject card in enemyDeckOrder)
        {
            card.GetComponent<Card>().CheckCardID();       
            card.GetComponent<Card>().CheckCardLevel();
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

    public void CheckBleed(GameObject Card)
    {
        if (Card.GetComponent<Card>().isBleeding)
        {
            Card.GetComponent<Card>().cardUIScript.damageInfoText.color = Color.red;
            Card.GetComponent<Card>().cardUIScript.damageInfoText.text = "-" + 1;
            Card.GetComponent<Card>().defence -= 1;
        }
    }

    public int CheckModifiedEffects(List<GameObject> checkList, int cardLookingFor)
    {
        for(int x = 0; x < checkList.Count; x++)
        {
            if (checkList[x].GetComponent<Card>().cardID == cardLookingFor)
            {
                return cardLookingFor;
            }
        }

        return -1;
    }

    public void EndStepEffects()
    {
        for(int x = 0; x < playerSlotPos.Count; x++)
        {
            playerSlotPos[x].GetComponent<Card>().EndStep();
        }

        for (int x = 0; x < enemySlotPos.Count; x++)
        {
            enemySlotPos[x].GetComponent<Card>().EndStep();
        }
    }
}
