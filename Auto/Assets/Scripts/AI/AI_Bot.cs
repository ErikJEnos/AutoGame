using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI_Bot : MonoBehaviour
{
    private GameObject StartButton;
    private GameObject StartFightButton;
    public GameObject QuickPlay;
    public GameObject BotPlayer;
    public GameObject gameManager;

    public List<GameObject> cardsInShop;
    public List<GameObject> botCards;

    public int state = 0;
    public static GameObject ob;
    // Start is called before the first frame update
    private void Awake()
    {

        if (ob == null)
        {
            ob = this.gameObject;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    void Start()
    {
        StartButton = GameObject.Find("StartBot");
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 1)
        {
            QuickPlay.GetComponent<Button>().onClick.Invoke();
            state++;
        }
        else if (state == 2)
        {
            BotPlayer = GameObject.Find("Player1");
            gameManager = GameObject.Find("GameManager");


            if (BotPlayer != null && gameManager != null)
            {
                state++;
            }
        }
        else if (state == 3)//Shop
        {
            cardsInShop = gameManager.GetComponent<PickingCards>().pickingCardList;
           
            int tempCount = gameManager.GetComponent<PickingCards>().actions;

            for (int x = 0; x < tempCount; x++)
            {
                cardsInShop[0].GetComponent<Button>().onClick.Invoke();
            }

            state++;
        }
        else if (state == 4)//Deck Build
        {
            int tempCount = 0;

            for (int x = 0; x < BotPlayer.GetComponent<Player>().deck.Count; x++)
            {
                botCards.Add(BotPlayer.GetComponent<Player>().deck[x]);
            }

            LevelUpCards();

            if (BotPlayer.GetComponent<Player>().deck.Count >= 5)
            {
                tempCount = 5;
            }
            else
            {
                tempCount = BotPlayer.GetComponent<Player>().deck.Count;
            }

            for (int x = 0; x < tempCount; x++)
            {
                returnThickCard();
            }

            tempCount = botCards.Count;

            for (int x = 0; x < tempCount; x++)
            {
                botCards.RemoveAt(0);
            }
           
           state++;
        }
        else if (state == 5)//Start Battle 
        {
            state++;

            StartFightButton = GameObject.Find("StartBattleBttn");
            StartFightButton.GetComponent<Button>().onClick.Invoke();
           
        }
        else if (state == 6)
        {
            if (gameManager.GetComponent<GameState>() != null)
            {
                if (gameManager.GetComponent<GameState>().gameState == 0)
                {
                    ResetState();
                }
            }
            
        }
        
        
    }

    public void OnBotButtonPress()
    {
        state = 1; 
    }


    public void LevelUpCards()
    {
        for (int x = 0; x < botCards.Count; x++)
        {
            for (int y = 0; y < botCards.Count; y++)
            {
                if(botCards[x].GetComponent<Card>().cardID == botCards[y].GetComponent<Card>().cardID && botCards[x] != botCards[y])
                {
                    botCards[x].GetComponent<Card>().cardLevel++;
                    botCards[x].GetComponent<Card>().CheckCardLevel();
                    Destroy(botCards[y]);
                    BotPlayer.GetComponent<Player>().deck.Remove(botCards[y]);
                    botCards.Remove(botCards[y]);
                }
            }
        }
    }

    public void returnThickCard()
    {
        GameObject thickestCard;
        thickestCard = botCards[0];

        for(int x = 0; x < botCards.Count; x++)
        {
            if (botCards[x].GetComponent<Card>().defence > thickestCard.GetComponent<Card>().defence)
            {
                thickestCard = botCards[x];
            }
        }
   
        botCards.Remove(thickestCard);
        BotPlayer.GetComponent<Player>().deckOrdered.Add(thickestCard);
    }

    public void ResetState()
    {
        state = 3;
    }
}
