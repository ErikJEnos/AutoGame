using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;
public class Card : MonoBehaviour
{
    public static class CardName
    {
        public const int ZOMBIE = 0;
        public const int SKELETON = 1;
        public const int FLYINGSNAKE = 2;
        public const int TREEIMP = 3;
        public const int GNOME = 4;
        public const int SLIME = 5;
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
        public const int WYVERN = 29;
        public const int SHAMAN = 30;
        public const int MUMMY = 31;

        public const int PALADIN = 32;
        public const int HYDRA = 33;


    }


    public GameObject order;
    public GameObject foreground;
    public GameObject background;
    public bool interactable = true;

    public CardUI cardUIScript;

    public int cardID = 0;
    public int attack = 0;
    public int defence = 1;
    public int OrignalAttack = 0;
    public int OrignalDefence = 1;
    private int lastDefence = 1;
    public int cardLevel = 0;
    public bool isMonster = false;
    public bool canPoison = false;
    public bool canBleed = false;
    public bool canTarget = false;
    public bool killed = false;
    public string text;
    public int playerID = 0;
    public int cardPos = 0;
    public int revives = 0;
    public bool inScene = false;
    public bool chosen = false;

    public bool isPoisoned = false;
    public bool isBleeding = false;
    public bool spawned = false;
    public bool kill = false;
    public bool hurt = false;

    private int cardPoolSize = 8;


    GameObject player;
    GameObject gameManager;
    System.Random random = new System.Random();

    private GameObject gameLogic;


    private void Awake()
    {

        foreground = GameObject.Find("ForeGround");
        background = GameObject.Find("CardSpawn");
        player = GameObject.Find("Player1");
        gameManager = GameObject.Find("GameManager");

        cardUIScript = gameObject.GetComponent<CardUI>();

        if (gameManager.GetComponent<GameLoop>().cardPoolSize > 34)
        {
            cardID = Random.RandomRange(0, 34);
        }
        else
        {
            cardID = Random.RandomRange(0, gameManager.GetComponent<GameLoop>().cardPoolSize);
        }

        //cardID = Random.RandomRange(29, 34);

        CheckCardID();

    }

    // Update is called once per frame
    void Update()
    {
        cardUIScript.attackText.text = attack.ToString();
        cardUIScript.defenceText.text = defence.ToString();
        cardUIScript.levelText.text = cardLevel.ToString();

        if(defence <= 0 && !hurt)
        {
            gameManager.GetComponent<GameLoop>().actions.Add(gameObject);
            hurt = true;
        }
        else if(lastDefence > defence && !hurt)
        {
            lastDefence = defence;
            gameManager.GetComponent<GameLoop>().actions.Add(gameObject);
            hurt = true;

        } 
        else if(lastDefence < defence)
        {
            lastDefence = defence;
        } 
        else if(lastDefence > defence)
        {
            Debug.Log("Bleed here");
        }

    }

    public void SelectCard()
    {

        if (gameManager.GetComponent<GameState>().gameState == 0 && chosen == false) //picking cards to add to the player pool
        {
            gameObject.transform.parent = gameManager.GetComponent<PickingCards>().deckSpawnLocation.transform.parent;
            player.GetComponent<Player>().deck.Insert(0, gameObject);
            gameManager.GetComponent<PickingCards>().pickingCardList.Remove(gameObject);
            gameManager.GetComponent<PickingCards>().actions--;
            chosen = true;
           
        }
    }

    public void CheckCardID()
    {   //POOL 1
        if (cardID == CardName.ZOMBIE)
        {
            cardUIScript.cardTitle.text = "Zombie";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Death Spawns a (1, 1) Crawler";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 1;
            OrignalDefence = defence = 2;
            lastDefence = defence;
        }
        else if(cardID == CardName.SKELETON)
        {
            cardUIScript.cardTitle.text = "Skeleton";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Death deals 2 damage to last pos";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 2;
            OrignalDefence = defence = 1;
            lastDefence = defence;
        }
        else if (cardID == CardName.FLYINGSNAKE)
        {
            cardUIScript.cardTitle.text = "Flying Snake";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Hit gives poison  ";
            isMonster = true;
            canPoison = true;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 1;
            OrignalDefence = defence = 1;
            lastDefence = defence;
        }
        else if (cardID == CardName.TREEIMP)
        {
            cardUIScript.cardTitle.text = "Tree imp";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 1;
            OrignalDefence = defence = 2;
            cardUIScript.attackText.text = attack.ToString();
            cardUIScript.defenceText.text = defence.ToString();
            cardUIScript.infoText.text = "On Death gives +0 +1 to last pos";
            lastDefence = defence;

        }
        else if (cardID == CardName.GNOME)
        {
            cardUIScript.cardTitle.text = "Gnomes";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "Can't be targeted by abilities";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = false;
            hurt = false;
            OrignalAttack = attack = 2;
            OrignalDefence = defence = 2;
            lastDefence = defence;
        }
        else if (cardID == CardName.SLIME)
        {
            cardUIScript.cardTitle.text = "Slime";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 1;
            OrignalDefence = defence = 1;
            lastDefence = defence;
        }
        else if (cardID == CardName.HUMAN)
        {
            cardUIScript.cardTitle.text = "Human";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Set gives all humans +0 +1";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 2;
            OrignalDefence = defence = 2;
            lastDefence = defence;
        }
        else if (cardID == CardName.GOBLIN)
        {
            cardUIScript.cardTitle.text = "Goblin";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Set deal 1 damage to first pos";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 3;
            OrignalDefence = defence = 1;
            lastDefence = defence;
        }

        //POOL 2
        else if (cardID == CardName.FARMER)
        {
            cardUIScript.cardTitle.text = "Farmer";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Death gives unit behind +2 +1";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 3;
            OrignalDefence = defence = 2;
            lastDefence = defence;
        }
        else if (cardID == CardName.HOBGOBLIN)
        {
            cardUIScript.cardTitle.text = "Hobgoblin";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Set deal damage to last pos equal to defence";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 2;
            OrignalDefence = defence = 3;
            lastDefence = defence;
        }
        else if (cardID == CardName.MIMIC)
        {
            cardUIScript.cardTitle.text = "Mimic";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Set attack becomes equal to enemy's defence in first pos";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 1;
            OrignalDefence = defence = 1;
            lastDefence = defence;
        }
        else if (cardID == CardName.WOLVES)
        {
            cardUIScript.cardTitle.text = "Wolves";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On kill gain +1 +0";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            kill = true;
            hurt = false;
            OrignalAttack = attack = 2;
            OrignalDefence = defence = 2;
            lastDefence = defence;
        }
        else if (cardID == CardName.GAINTRAT)
        {
            cardUIScript.cardTitle.text = "Giant Rat";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Hit apply poison debuff";
            isMonster = true;
            canPoison = true;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 1;
            OrignalDefence = defence = 3;
            lastDefence = defence;
        }
        
        else if (cardID == CardName.GAINTBAT)
        {
            cardUIScript.cardTitle.text = "Giant Bat";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Hit apply Bleed debuff";
            isMonster = true;
            canPoison = false;
            canBleed = true;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 3;
            OrignalDefence = defence = 2;
            lastDefence = defence;
        }
        else if (cardID == CardName.GHOST)
        {
            cardUIScript.cardTitle.text = "Ghost";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Hit takes 1 less damage ";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 1;
            OrignalDefence = defence = 3;
            lastDefence = defence;
        }
        else if (cardID == CardName.FLYINGSWORD)
        {
            cardUIScript.cardTitle.text = "Flying Sowrd";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Set gives +2 +1 to first pos";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 3;
            OrignalDefence = defence = 1;
            lastDefence = defence;
        }

        //Pool 3

        else if (cardID == CardName.VAMPIRE)
        {
            cardUIScript.cardTitle.text = "Vampire";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On kill gain +0 +1 ";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 3;
            OrignalDefence = defence = 2;
            lastDefence = defence;
        }
        else if (cardID == CardName.KNIGHT)
        {
            cardUIScript.cardTitle.text = "Knight";
            cardUIScript.infoText.text = "Gives all humans +3 +0";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 2;
            OrignalDefence = defence = 4;
            lastDefence = defence;
        }
        else if (cardID == CardName.GRAVEDIGGER)
        {
            cardUIScript.cardTitle.text = "Gravedigger";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Set summons a zombie behind ";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 1;
            OrignalDefence = defence = 2;
            lastDefence = defence;
        }
        else if (cardID == CardName.ORC)
        {
            cardUIScript.cardTitle.text = "Orc";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Hurt deals 2 damage to unit in first Pos ";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 3;
            OrignalDefence = defence = 3;
            lastDefence = defence;
        }
        else if (cardID == CardName.WEREWOLF)
        {
            cardUIScript.cardTitle.text = "Werewolf";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "Get +1+1 at the end of the turn";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 3;
            OrignalDefence = defence = 3;
            lastDefence = defence;
        }
        else if (cardID == CardName.GUARD)
        {
            cardUIScript.cardTitle.text = "Guard";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Set moves to first pos";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 1;
            OrignalDefence = defence = 4;
            lastDefence = defence;
        }
        else if (cardID == CardName.BERSERKER)
        {
            cardUIScript.cardTitle.text = "Berserker";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Set moves to first pos";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 5;
            OrignalDefence = defence = 1;
            lastDefence = defence;
        }
        else if (cardID == CardName.DRUID)
        {
            cardUIScript.cardTitle.text = "Druid";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Set summons 2 tree imps behind";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 2;
            OrignalDefence = defence = 3;
            lastDefence = defence;
        }
        else if (cardID == CardName.NECROMANCER)
        {
            cardUIScript.cardTitle.text = "Necromancer";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "Death effects double";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 2;
            OrignalDefence = defence = 5;
            lastDefence = defence;
        }
        else if (cardID == CardName.NYMPH)
        {
            cardUIScript.cardTitle.text = "Nymph";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "Give monster in first pos +0+2 at end step";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 2;
            OrignalDefence = defence = 4;
            lastDefence = defence;
        }
        else if (cardID == CardName.WITCH)
        {
            cardUIScript.cardTitle.text = "Witch";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Death deal 1 damage to all enemy monsters. Damage Effects gain posion";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 1;
            OrignalDefence = defence = 4;
            lastDefence = defence;
        }
        else if (cardID == CardName.WIZARD)
        {
            cardUIScript.cardTitle.text = "Wizard";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "Doubles damage effects";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 2;
            OrignalDefence = defence = 5;
            lastDefence = defence;
        }
        else if (cardID == CardName.LICH)
        {
            cardUIScript.cardTitle.text = "Lich";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "Every Monster summons a skeleton on death ";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 4;
            OrignalDefence = defence = 6;
            lastDefence = defence;
        }
        else if (cardID == CardName.WYVERN)
        {
            cardUIScript.cardTitle.text = "Wyvern";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Hurt deals 1 damage to all frienly monsters";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 6;
            OrignalDefence = defence = 8;
            lastDefence = defence;
        }
        else if (cardID == CardName.SHAMAN)
        {
            cardUIScript.cardTitle.text = "Shaman";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Set give all friendly units +0+4";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 3;
            OrignalDefence = defence = 4;
            lastDefence = defence;
        }
        else if (cardID == CardName.MUMMY)
        {
            cardUIScript.cardTitle.text = "Mummy";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Set move lowest defence enemy unit to first pos ";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 3;
            OrignalDefence = defence = 3;
            lastDefence = defence;
        }
        else if (cardID == CardName.PALADIN)
        {
            cardUIScript.cardTitle.text = "Paladin";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Hurt give all friendly units +0+2";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 6;
            OrignalDefence = defence = 6;
            lastDefence = defence;
        }
        else if (cardID == CardName.HYDRA)
        {
            cardUIScript.cardTitle.text = "Hydra";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "On Death revives and loses (-1-1) up to 6 times";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            revives = 5;
            OrignalAttack = attack = 6;
            OrignalDefence = defence = 6;
            lastDefence = defence;
        }





















        else if (cardID == 2131236)
        {
            cardUIScript.cardTitle.text = "Boar";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = false;
            hurt = false;
            OrignalAttack = attack = 1;
            OrignalDefence = defence = 2;
            lastDefence = defence;
        }
        
    
        else if (cardID == 912)
        {
            cardUIScript.cardTitle.text = "Spell";
            isMonster = false;
            canPoison = false;
            OrignalAttack = attack = 0;
            OrignalDefence = defence = 0;
            cardUIScript.attackText.text = "";
            cardUIScript.defenceText.text = "";
            cardUIScript.infoText.text = "Give '1' defence to monster in front if possible else deal '1' damage to monster on enemy side";
        }
        else if (cardID == 103)
        {
            cardUIScript.cardTitle.text = "Spell";
            isMonster = false;
            canPoison = false;
            OrignalAttack = attack = 0;
            OrignalDefence = defence = 0;
            cardUIScript.attackText.text = "";
            cardUIScript.defenceText.text = "";
            cardUIScript.infoText.text = "If you control a monster deal '2' damage to front enemy monster, else spawn a 0-1 monster ";
        }
        else if(cardID == 98)
        {
            cardUIScript.cardTitle.text = "Crawler";
            this.name = cardUIScript.cardTitle.text + "_" + player;
            cardUIScript.infoText.text = "";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = true;
            hurt = false;
            OrignalAttack = attack = 1;
            OrignalDefence = defence = 1;
            cardUIScript.attackText.text = attack.ToString();
            cardUIScript.defenceText.text = defence.ToString();
            lastDefence = defence;
        }
        
        else
        {
            cardUIScript.cardTitle.text = "Monster";
            isMonster = true;
            canPoison = false;
            canBleed = false;
            canTarget = false;
            OrignalAttack = attack = 0;
            OrignalDefence = defence = 1;
            cardUIScript.attackText.text = attack.ToString();
            cardUIScript.defenceText.text = defence.ToString();
            cardUIScript.infoText.text = "Monster";
            lastDefence = defence;

        }

        CheckCardLevel();
    }

    public void setCardId(int id)
    {
        cardID = id;
        CheckCardID();
    }

    public void CheckCardLevel()
    {

        if(cardLevel == 0)
        {
            attack = OrignalAttack + cardLevel;
            defence = OrignalDefence + cardLevel;
        } 
        else if (cardLevel == 1)
        {
            attack = OrignalAttack + cardLevel;
            defence = OrignalDefence + cardLevel;
        }
        else if (cardLevel == 2)
        {
            attack = OrignalAttack + cardLevel;
            defence = OrignalDefence + cardLevel;
        }
        else if (cardLevel == 3)
        {
            attack = OrignalAttack + cardLevel;
            defence = OrignalDefence + cardLevel;
        }
        else if (cardLevel == 4)
        {
            attack = OrignalAttack + cardLevel;
            defence = OrignalDefence + cardLevel;
        }
        else if (cardLevel == 5)
        {
            attack = OrignalAttack + cardLevel;
            defence = OrignalDefence + cardLevel;
        }
        else
        {
            attack = OrignalAttack + cardLevel;
            defence = OrignalDefence + cardLevel;
        }
    }

    public void OnDeathTrigger()
    {
        bool callonce = true;
        bool witchPoison = false;
        int damageEffects = 1;
        int onDeathEffects = 1;

        if (callonce)
        {
            if (playerID == gameManager.GetComponent<GameLoop>().enemy.GetComponent<Player>().id)
            {
                if (gameManager.GetComponent<GameLoop>().CheckModifiedEffects(gameManager.GetComponent<GameLoop>().enemySlotPos, CardName.NECROMANCER) == CardName.NECROMANCER)
                {
                    onDeathEffects++;
                }

                if(gameManager.GetComponent<GameLoop>().CheckModifiedEffects(gameManager.GetComponent<GameLoop>().enemySlotPos, CardName.WITCH) == CardName.WITCH)
                {
                    witchPoison = true;
                }

                if (gameManager.GetComponent<GameLoop>().CheckModifiedEffects(gameManager.GetComponent<GameLoop>().enemySlotPos, CardName.WIZARD) == CardName.WIZARD)
                {
                    damageEffects++;
                }

                for (int x = 0; x <= onDeathEffects-1; x++)
                {

                    if (gameManager.GetComponent<GameLoop>().CheckModifiedEffects(gameManager.GetComponent<GameLoop>().enemySlotPos, CardName.LICH) == CardName.LICH)
                    {
                        if (spawned == false)
                        {
                            GameObject temp = Instantiate(gameManager.GetComponent<GameLoop>().cardPrefab, gameObject.transform.position, transform.rotation);
                            temp.transform.parent = gameManager.GetComponent<GameLoop>().enemyDeckPos.transform;
                            temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                            temp.GetComponent<Card>().setCardId(CardName.SKELETON);
                            temp.GetComponent<Card>().spawned = true;
                            temp.GetComponent<Card>().playerID = playerID;
                            gameManager.GetComponent<GameLoop>().enemySlotPos.Insert(cardPos, temp);
                        }
                    }

                    if (cardID == CardName.ZOMBIE)
                    {
                        GameObject temp = Instantiate(gameManager.GetComponent<GameLoop>().cardPrefab, gameObject.transform.position, transform.rotation);
                        temp.transform.parent = gameManager.GetComponent<GameLoop>().enemyDeckPos.transform;
                        temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        temp.GetComponent<Card>().setCardId(98);
                        gameManager.GetComponent<GameLoop>().enemySlotPos.Insert(cardPos, temp);
                    }
                    else if (cardID == CardName.SKELETON)
                    {

                        gameManager.GetComponent<GameLoop>().CheckFilledSlots();

                        if (gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1 >= 0)
                        {
                            if (gameManager.GetComponent<GameLoop>().playerSlotPos[gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1].GetComponent<Card>() != null && gameManager.GetComponent<GameLoop>().playerSlotPos[gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1].GetComponent<Card>().canTarget)
                            {
                                gameManager.GetComponent<GameLoop>().playerSlotPos[gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1].GetComponent<Card>().defence -= 2 * onDeathEffects * damageEffects;
                                gameManager.GetComponent<GameLoop>().playerSlotPos[gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1].GetComponent<CardUI>().damageInfoText.color = Color.white;
                                gameManager.GetComponent<GameLoop>().playerSlotPos[gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1].GetComponent<CardUI>().damageInfoText.text = "-" + 2 * onDeathEffects * damageEffects;
                                gameManager.GetComponent<GameLoop>().playerSlotPos[gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1].GetComponent<Card>().isPoisoned = witchPoison;
                            }
                        }
                    }
                    else if (cardID == CardName.FARMER)
                    {
                        if (gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1 > 0)
                        {
                            if (gameManager.GetComponent<GameLoop>().enemySlotPos[gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1].GetComponent<Card>() != null)
                            {
                                gameManager.GetComponent<GameLoop>().enemySlotPos[gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1].GetComponent<Card>().attack += 2;
                                gameManager.GetComponent<GameLoop>().enemySlotPos[gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1].GetComponent<Card>().defence += 1;
                                gameManager.GetComponent<GameLoop>().enemySlotPos[gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1].GetComponent<CardUI>().damageInfoText.color = Color.green;
                                gameManager.GetComponent<GameLoop>().enemySlotPos[gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1].GetComponent<CardUI>().damageInfoText.text = "+" + 2 + "+" + 1;

                            }
                        }
                    }
                    else if (cardID == CardName.TREEIMP)
                    {
                        Debug.Log("Treeimpsss");
                        if (gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1 > 0)
                        {
                            if (gameManager.GetComponent<GameLoop>().enemySlotPos[gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1].GetComponent<Card>() != null)
                            {
                                gameManager.GetComponent<GameLoop>().enemySlotPos[gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1].GetComponent<Card>().defence += 1;
                                gameManager.GetComponent<GameLoop>().enemySlotPos[gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1].GetComponent<CardUI>().damageInfoText.color = Color.green;
                                gameManager.GetComponent<GameLoop>().enemySlotPos[gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1].GetComponent<CardUI>().damageInfoText.text = "+" + 0 + "+" + 1;

                            }
                        }
                    }
                    else if (cardID == CardName.WITCH)
                    {
                        if (gameManager.GetComponent<GameLoop>().enemySlotPos.Count > 0)
                        {
                            for(int c = 0; c < gameManager.GetComponent<GameLoop>().playerSlotPos.Count; c++)
                            {
                                if (gameManager.GetComponent<GameLoop>().playerSlotPos[c].GetComponent<Card>() != null)
                                {
                                    gameManager.GetComponent<GameLoop>().playerSlotPos[c].GetComponent<Card>().defence -= 1 * onDeathEffects * damageEffects;
                                    gameManager.GetComponent<GameLoop>().playerSlotPos[c].GetComponent<CardUI>().damageInfoText.color = Color.white;
                                    gameManager.GetComponent<GameLoop>().playerSlotPos[c].GetComponent<CardUI>().damageInfoText.text = "-" + 1 * onDeathEffects * damageEffects;
                                    gameManager.GetComponent<GameLoop>().playerSlotPos[c].GetComponent<Card>().isPoisoned = witchPoison;
                                }
                            }
                        }
                    }
                    else if (cardID == CardName.HYDRA && revives > 0)
                    {
                        GameObject temp = Instantiate(gameManager.GetComponent<GameLoop>().cardPrefab, gameObject.transform.position, transform.rotation);
                        temp.transform.parent = gameManager.GetComponent<GameLoop>().enemyDeckPos.transform;
                        temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        temp.GetComponent<Card>().setCardId(CardName.HYDRA);
                        temp.GetComponent<Card>().revives = revives - 1;
                        temp.GetComponent<Card>().GetComponent<Card>().attack = attack - 1;
                        temp.GetComponent<Card>().GetComponent<Card>().defence = revives;
                        temp.GetComponent<Card>().GetComponent<CardUI>().damageInfoText.color = Color.white;
                        temp.GetComponent<Card>().GetComponent<CardUI>().damageInfoText.text = "-" + 1 + " -" + 1;
                        temp.GetComponent<Card>().playerID = playerID;

                        gameManager.GetComponent<GameLoop>().enemySlotPos.Insert(cardPos, temp);
                    }
                }
            }

            else if (playerID == player.GetComponent<Player>().id)
            {
                damageEffects = 1;
                onDeathEffects = 1;
                witchPoison = false;

                if (gameManager.GetComponent<GameLoop>().CheckModifiedEffects(gameManager.GetComponent<GameLoop>().playerSlotPos, CardName.NECROMANCER) == CardName.NECROMANCER)
                {
                    onDeathEffects++;
                }

                if (gameManager.GetComponent<GameLoop>().CheckModifiedEffects(gameManager.GetComponent<GameLoop>().playerSlotPos, CardName.WITCH) == CardName.WITCH)
                {
                    witchPoison = true;
                }

                if (gameManager.GetComponent<GameLoop>().CheckModifiedEffects(gameManager.GetComponent<GameLoop>().playerSlotPos, CardName.WIZARD) == CardName.WIZARD)
                {
                    damageEffects++;
                }

                for (int x = 0; x <= onDeathEffects-1; x++)
                {
                    if (gameManager.GetComponent<GameLoop>().CheckModifiedEffects(gameManager.GetComponent<GameLoop>().playerSlotPos, CardName.LICH) == CardName.LICH)
                    {
                        if(spawned == false)
                        {
                            Debug.Log("Player lich called");
                            GameObject temp = Instantiate(gameManager.GetComponent<GameLoop>().cardPrefab, gameObject.transform.position, transform.rotation);
                            temp.transform.parent = gameManager.GetComponent<GameLoop>().enemyDeckPos.transform;
                            temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                            temp.GetComponent<Card>().setCardId(CardName.SKELETON);
                            temp.GetComponent<Card>().spawned = true;
                            temp.GetComponent<Card>().playerID = playerID;
                            gameManager.GetComponent<GameLoop>().playerSlotPos.Insert(cardPos, temp);
                        }
                    }

                    if (cardID == CardName.ZOMBIE)
                    {
                        GameObject temp = Instantiate(gameManager.GetComponent<GameLoop>().cardPrefab, gameObject.transform.position, transform.rotation);
                        temp.transform.parent = gameManager.GetComponent<GameLoop>().playerDeckPos.transform;
                        temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        temp.GetComponent<Card>().setCardId(98);
                        gameManager.GetComponent<GameLoop>().playerSlotPos.Insert(cardPos, temp);
                    }
                    else if (cardID == CardName.SKELETON)
                    {
                        gameManager.GetComponent<GameLoop>().CheckFilledSlots();

                        if (gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1 >= 0)
                        {
                            if (gameManager.GetComponent<GameLoop>().enemySlotPos[gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1].GetComponent<Card>() != null && gameManager.GetComponent<GameLoop>().enemySlotPos[gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1].GetComponent<Card>().canTarget)
                            {
                                gameManager.GetComponent<GameLoop>().enemySlotPos[gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1].GetComponent<Card>().defence -= 2 * onDeathEffects * damageEffects;
                                gameManager.GetComponent<GameLoop>().enemySlotPos[gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1].GetComponent<CardUI>().damageInfoText.color = Color.white;
                                gameManager.GetComponent<GameLoop>().enemySlotPos[gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1].GetComponent<CardUI>().damageInfoText.text = "-" + 2 * onDeathEffects * damageEffects;
                                gameManager.GetComponent<GameLoop>().enemySlotPos[gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1].GetComponent<Card>().isPoisoned = witchPoison;

                            }
                        }
                    }
                    else if (cardID == CardName.FARMER)
                    {
                        if (gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1 > 0)
                        {
                            if (gameManager.GetComponent<GameLoop>().playerSlotPos[gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1].GetComponent<Card>() != null)
                            {
                                gameManager.GetComponent<GameLoop>().playerSlotPos[gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1].GetComponent<Card>().attack += 2;
                                gameManager.GetComponent<GameLoop>().playerSlotPos[gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1].GetComponent<Card>().defence += 1;
                                gameManager.GetComponent<GameLoop>().playerSlotPos[gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1].GetComponent<CardUI>().damageInfoText.color = Color.green;
                                gameManager.GetComponent<GameLoop>().playerSlotPos[gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1].GetComponent<CardUI>().damageInfoText.text = "+" + 2 + " +" + 1;

                            }
                        }
                    }
                    else if (cardID == CardName.TREEIMP)
                    {
                        if (gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1 > 0)
                        {
                            if (gameManager.GetComponent<GameLoop>().playerSlotPos[gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1].GetComponent<Card>() != null)
                            {
                                gameManager.GetComponent<GameLoop>().playerSlotPos[gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1].GetComponent<Card>().defence += 1;
                                gameManager.GetComponent<GameLoop>().playerSlotPos[gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1].GetComponent<CardUI>().damageInfoText.color = Color.green;
                                gameManager.GetComponent<GameLoop>().playerSlotPos[gameManager.GetComponent<GameLoop>().playerSlotPos.Count - 1].GetComponent<CardUI>().damageInfoText.text = "+" + 0 + "+" + 1;

                            }
                        }
                    }
                    else if (cardID == CardName.WITCH)
                    {
                        if (gameManager.GetComponent<GameLoop>().enemySlotPos.Count - 1 > 0)
                        {
                            for (int c = 0; c < gameManager.GetComponent<GameLoop>().enemySlotPos.Count; c++)
                            {
                                if (gameManager.GetComponent<GameLoop>().enemySlotPos[c].GetComponent<Card>() != null)
                                {
                                    gameManager.GetComponent<GameLoop>().enemySlotPos[c].GetComponent<Card>().defence -= 1 * onDeathEffects * damageEffects;
                                    gameManager.GetComponent<GameLoop>().enemySlotPos[c].GetComponent<CardUI>().damageInfoText.color = Color.white;
                                    gameManager.GetComponent<GameLoop>().enemySlotPos[c].GetComponent<CardUI>().damageInfoText.text = "-" + 1 * onDeathEffects * damageEffects;
                                    gameManager.GetComponent<GameLoop>().enemySlotPos[c].GetComponent<Card>().isPoisoned = witchPoison;
                                }
                            }
                        }
                    }
                    else if (cardID == CardName.HYDRA && revives > 0)
                    {
                        GameObject temp = Instantiate(gameManager.GetComponent<GameLoop>().cardPrefab, gameObject.transform.position, transform.rotation);
                        temp.transform.parent = gameManager.GetComponent<GameLoop>().enemyDeckPos.transform;
                        temp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        temp.GetComponent<Card>().setCardId(CardName.HYDRA);
                        temp.GetComponent<Card>().revives = revives - 1;
                        temp.GetComponent<Card>().GetComponent<Card>().attack = attack - 1; ;
                        temp.GetComponent<Card>().GetComponent<Card>().defence = revives;
                        temp.GetComponent<Card>().GetComponent<CardUI>().damageInfoText.color = Color.white;
                        temp.GetComponent<Card>().GetComponent<CardUI>().damageInfoText.text = "-" + 1 + " -" + 1;
                        temp.GetComponent<Card>().playerID = playerID;

                        gameManager.GetComponent<GameLoop>().playerSlotPos.Insert(cardPos, temp);
                    }
                }
            }

            callonce = false;
        }

    }

    public virtual void StartOfTurn()
    {
        if(cardID == CardName.WEREWOLF)
        {
            attack++;
            defence++;
            gameObject.GetComponent<CardUI>().damageInfoText.color = Color.green;
            gameObject.GetComponent<CardUI>().damageInfoText.text = "+" + 1 + "+" + 1;
        }
    }

    public virtual void OnSetTrigger()
    {

    }

    public virtual void OnKillTrigger()
    {
        if (cardID == CardName.WOLVES)
        {
            attack += 1;
            cardUIScript.damageInfoText.color = Color.green;
            cardUIScript.damageInfoText.text = "+" + 1 + "+0";
            gameManager.GetComponent<GameLoop>().CheckBleed(gameObject);
        }

        else if (cardID == CardName.VAMPIRE)
        {
            defence += 1;
            cardUIScript.damageInfoText.color = Color.green;
            cardUIScript.damageInfoText.text = "+0" + "+" + 1;

            gameManager.GetComponent<GameLoop>().CheckBleed(gameObject);
        }

        killed = false;
    }

    public virtual void OnHurtTrigger()
    {
        bool witchPoison = false;
        int damageEffects = 1;

        if (hurt)
        {
            if (playerID == gameManager.GetComponent<GameLoop>().enemy.GetComponent<Player>().id)
            {
                if (gameManager.GetComponent<GameLoop>().CheckModifiedEffects(gameManager.GetComponent<GameLoop>().enemySlotPos, CardName.WITCH) == CardName.WITCH)
                {
                    witchPoison = true;
                }

                if (gameManager.GetComponent<GameLoop>().CheckModifiedEffects(gameManager.GetComponent<GameLoop>().enemySlotPos, CardName.WIZARD) == CardName.WIZARD)
                {
                    damageEffects++;
                }
                else if (cardID == CardName.ORC && gameManager.GetComponent<GameLoop>().playerSlotPos.Count > 0)
                {
                    if (gameManager.GetComponent<GameLoop>().playerSlotPos[0].GetComponent<Card>().canTarget)
                    {
                        gameManager.GetComponent<GameLoop>().playerSlotPos[0].GetComponent<Card>().defence -= 2 * damageEffects;
                        gameManager.GetComponent<GameLoop>().playerSlotPos[0].GetComponent<CardUI>().damageInfoText.color = Color.white;
                        gameManager.GetComponent<GameLoop>().playerSlotPos[0].GetComponent<CardUI>().damageInfoText.text = "-" + 2 * damageEffects;
                        gameManager.GetComponent<GameLoop>().playerSlotPos[0].GetComponent<Card>().isPoisoned = witchPoison;
                    }
                }
                else if (cardID == CardName.WYVERN)
                {
                    foreach (GameObject enemyCard in gameManager.GetComponent<GameLoop>().enemySlotPos)
                    {
                        if (enemyCard != gameObject)
                        {
                            enemyCard.GetComponent<Card>().defence -= 1;
                            enemyCard.GetComponent<CardUI>().damageInfoText.color = Color.white;
                            enemyCard.GetComponent<CardUI>().damageInfoText.text = "-" + 1 * damageEffects;
                        }
                    }
                }
                else if (cardID == CardName.PALADIN)
                {
                    Debug.Log("PALADIN called");

                    foreach (GameObject enemyCard in gameManager.GetComponent<GameLoop>().enemySlotPos)
                    {
                        if (enemyCard != gameObject)
                        {
                            enemyCard.GetComponent<Card>().defence += 2;
                            enemyCard.GetComponent<CardUI>().damageInfoText.color = Color.green;
                            enemyCard.GetComponent<CardUI>().damageInfoText.text = "+" + 0 + "+"+ 2;
                        }
                    }
                }
            }

            

            else if (playerID == player.GetComponent<Player>().id)
            {
                witchPoison = false;
                damageEffects = 1;

                if (gameManager.GetComponent<GameLoop>().CheckModifiedEffects(gameManager.GetComponent<GameLoop>().playerSlotPos, CardName.WITCH) == CardName.WITCH)
                {
                    witchPoison = true;
                }

                if (gameManager.GetComponent<GameLoop>().CheckModifiedEffects(gameManager.GetComponent<GameLoop>().playerSlotPos, CardName.WIZARD) == CardName.WIZARD)
                {
                    damageEffects++;
                }
                else if (cardID == CardName.ORC && gameManager.GetComponent<GameLoop>().enemySlotPos.Count > 0)
                {
                    if (gameManager.GetComponent<GameLoop>().playerSlotPos[0].GetComponent<Card>().canTarget)
                    {
                        gameManager.GetComponent<GameLoop>().enemySlotPos[0].GetComponent<Card>().defence -= 2 * damageEffects;
                        gameManager.GetComponent<GameLoop>().enemySlotPos[0].GetComponent<CardUI>().damageInfoText.color = Color.white;
                        gameManager.GetComponent<GameLoop>().enemySlotPos[0].GetComponent<CardUI>().damageInfoText.text = "-" + 2 * damageEffects;
                        gameManager.GetComponent<GameLoop>().enemySlotPos[0].GetComponent<Card>().isPoisoned = witchPoison;
                    }
                }
                else if (cardID == CardName.WYVERN)
                {
                    foreach (GameObject playerCard in gameManager.GetComponent<GameLoop>().playerSlotPos)
                    {
                        if(playerCard != gameObject)
                        {
                            playerCard.GetComponent<Card>().defence -= 1;
                            playerCard.GetComponent<CardUI>().damageInfoText.color = Color.white;
                            playerCard.GetComponent<CardUI>().damageInfoText.text = "-" + 1 * damageEffects;

                        }
                    }
                }
                else if (cardID == CardName.PALADIN)
                {
                    Debug.Log("PALADIN called");

                    foreach (GameObject playerCard in gameManager.GetComponent<GameLoop>().playerSlotPos)
                    {
                        if (playerCard != gameObject)
                        {
                            playerCard.GetComponent<Card>().defence += 2;
                            playerCard.GetComponent<CardUI>().damageInfoText.color = Color.green;
                            playerCard.GetComponent<CardUI>().damageInfoText.text = "+" + 0 + "+" + 2;
                        }
                    }
                }
            }
        }

        hurt = false;
    }

    public virtual void EndStep()
    {
        if (playerID == gameManager.GetComponent<GameLoop>().enemy.GetComponent<Player>().id)
        {
            if (cardID == CardName.NYMPH)
            {
                gameManager.GetComponent<GameLoop>().enemySlotPos[0].GetComponent<Card>().defence += 2;
                gameManager.GetComponent<GameLoop>().enemySlotPos[0].GetComponent<CardUI>().damageInfoText.color = Color.green;
                gameManager.GetComponent<GameLoop>().enemySlotPos[0].GetComponent<CardUI>().damageInfoText.text = "+" + 0 + "+" + 2;

            }
        }

        else if (playerID == player.GetComponent<Player>().id)
        {
            if (cardID == CardName.NYMPH)
            {
                gameManager.GetComponent<GameLoop>().playerSlotPos[0].GetComponent<Card>().defence += 2;
                gameManager.GetComponent<GameLoop>().playerSlotPos[0].GetComponent<CardUI>().damageInfoText.color = Color.green;
                gameManager.GetComponent<GameLoop>().playerSlotPos[0].GetComponent<CardUI>().damageInfoText.text = "+" + 0 + "+" + 2;

            }
        }
    }
}
