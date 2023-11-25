using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFunctions : Card
{

    Card thisCard; 

    public static class CardName
    {

        public const int ZOMBIE = 0;
        public const int SKELETON = 1;
        public const int GAINTRAT = 2;
        public const int GOBLIN = 3;
        public const int GAINTBAT = 4;

    }


    // Start is called before the first frame update
    void Start()
    {
        thisCard = gameObject.GetComponent<Card>();
    }

    public void SetUpCardFunctions()
    {

        //if (thisCard.cardID == CardName.ZOMBIE)
        //{
        //    thisCard.cardTitle.text = "Zombie";
        //    thisCard.infoText.text = "On death spawn 1-1 crawler";
        //    thisCard.isMonster = true;
        //    thisCard.canPoison = false;
        //    thisCard.canBleed = false;
        //    thisCard.canTarget = true;
        //    thisCard.spawn = true;
        //    thisCard.OrignalAttack = thisCard.attack = 1;
        //    thisCard.OrignalDefence = thisCard.defence = 2;
        //    thisCard.attackText.text = thisCard.attack.ToString();
        //    thisCard.defenceText.text = thisCard.defence.ToString();
        //}
        //else if (thisCard.cardID == CardName.SKELETON)
        //{
        //    thisCard.cardTitle.text = "Skeleton";
        //    thisCard.infoText.text = "On death deal 1 damge to the back row enemy";
        //    thisCard.isMonster = true;
        //    thisCard.canPoison = false;
        //    thisCard.canBleed = false;
        //    thisCard.canTarget = true;
        //    thisCard.OrignalAttack = thisCard.attack = 2;
        //    thisCard.OrignalDefence = thisCard.defence = 1;
        //    thisCard.attackText.text = thisCard.attack.ToString();
        //    thisCard.defenceText.text = thisCard.defence.ToString();
        //}
        //else if (card.cardID == CardName.GAINTRAT)
        //{
        //    card.cardTitle.text = "Giant Rat";
        //    card.infoText.text = "Posions enemy on hit. poison lasts forever";
        //    card.isMonster = true;
        //    card.canPoison = true;
        //    card.canBleed = false;
        //    card.canTarget = true;
        //    card.OrignalAttack = card.attack = 1;
        //    card.OrignalDefence = card.defence = 3;
        //    card.attackText.text = card.attack.ToString();
        //    card.defenceText.text = card.defence.ToString();
        //}
        //else if (card.cardID == CardName.GOBLIN)
        //{
        //    card.cardTitle.text = "Goblin";
        //    card.infoText.text = "Get +0+1 for each friendly Goblin";
        //    card.isMonster = true;
        //    card.canPoison = false;
        //    card.canBleed = false;
        //    card.canTarget = true;
        //    card.OrignalAttack = card.attack = 2;
        //    card.OrignalDefence = card.defence = 1;
        //    card.attackText.text = card.attack.ToString();
        //    card.defenceText.text = card.defence.ToString();
        //}
        //else if (card.cardID == CardName.GAINTBAT)
        //{
        //    card.cardTitle.text = "Giant Bat";
        //    card.infoText.text = "Bleed stacking";
        //    card.isMonster = true;
        //    card.canPoison = false;
        //    card.canBleed = true;
        //    card.canTarget = true;
        //    card.OrignalAttack = card.attack = 2;
        //    card.OrignalDefence = card.defence = 2;
        //    card.attackText.text = card.attack.ToString();
        //    card.defenceText.text = card.defence.ToString();
        //}
        //else if (cardID == 5)
        //{
        //    cardTitle.text = "Human";
        //    infoText.text = "Get +0+1 for each friendly Human";
        //    isMonster = true;
        //    canPoison = false;
        //    canBleed = false;
        //    canTarget = true;
        //    OrignalAttack = attack = 1;
        //    OrignalDefence = defence = 1;
        //    attackText.text = attack.ToString();
        //    defenceText.text = defence.ToString();
        //}
        //else if (cardID == 6)
        //{
        //    cardTitle.text = "Slime";
        //    infoText.text = "";
        //    isMonster = true;
        //    canPoison = false;
        //    canBleed = false;
        //    canTarget = true;
        //    OrignalAttack = attack = 1;
        //    OrignalDefence = defence = 1;
        //    attackText.text = attack.ToString();
        //    defenceText.text = defence.ToString();
        //}
        //else if (cardID == 7)
        //{
        //    cardTitle.text = "Wolves";
        //    infoText.text = "Get +1+0 for each kill this monster does";
        //    isMonster = true;
        //    canPoison = false;
        //    canBleed = false;
        //    canTarget = true;
        //    OrignalAttack = attack = 2;
        //    OrignalDefence = defence = 2;
        //    attackText.text = attack.ToString();
        //    defenceText.text = defence.ToString();
        //}
        //else if (cardID == 8)
        //{
        //    cardTitle.text = "Gnomes";
        //    infoText.text = "Can't be targeted by spells";
        //    isMonster = true;
        //    canPoison = false;
        //    canBleed = false;
        //    canTarget = false;
        //    OrignalAttack = attack = 1;
        //    OrignalDefence = defence = 1;
        //    attackText.text = attack.ToString();
        //    defenceText.text = defence.ToString();
        //}
        //else if (cardID == 9)
        //{
        //    cardTitle.text = "Vampire";
        //    infoText.text = "Gains +0+1 when enemy is killed";
        //    isMonster = true;
        //    canPoison = false;
        //    canBleed = false;
        //    canTarget = false;
        //    OrignalAttack = attack = 3;
        //    OrignalDefence = defence = 2;
        //    attackText.text = attack.ToString();
        //    defenceText.text = defence.ToString();
        //}
        //else if (cardID == 10)
        //{
        //    cardTitle.text = "Gravedigger";
        //    infoText.text = "On set summons a zombie behind";
        //    isMonster = true;
        //    canPoison = false;
        //    canBleed = false;
        //    canTarget = false;
        //    OrignalAttack = attack = 1;
        //    OrignalDefence = defence = 3;
        //    attackText.text = attack.ToString();
        //    defenceText.text = defence.ToString();
        //}
        //else if (cardID == 11)
        //{
        //    cardTitle.text = "Farmer";
        //    infoText.text = "on death gives monster behind +2+1";
        //    isMonster = true;
        //    canPoison = false;
        //    canBleed = false;
        //    canTarget = false;
        //    OrignalAttack = attack = 2;
        //    OrignalDefence = defence = 1;
        //    attackText.text = attack.ToString();
        //    defenceText.text = defence.ToString();
        //}
        //else if (cardID == 12)
        //{
        //    cardTitle.text = "Hobgoblin";
        //    infoText.text = "gets +1+1 for each goblin type on the field";
        //    isMonster = true;
        //    canPoison = false;
        //    canBleed = false;
        //    canTarget = false;
        //    OrignalAttack = attack = 3;
        //    OrignalDefence = defence = 3;
        //    attackText.text = attack.ToString();
        //    defenceText.text = defence.ToString();
        //}
        //else if (cardID == 13)
        //{
        //    cardTitle.text = "Mimic";
        //    infoText.text = "Sets attack to enemy's defence";
        //    isMonster = true;
        //    canPoison = false;
        //    canBleed = false;
        //    canTarget = false;
        //    OrignalAttack = attack = 0;
        //    OrignalDefence = defence = 1;
        //    attackText.text = attack.ToString();
        //    defenceText.text = defence.ToString();
        //}
        //else if (cardID == 14)
        //{
        //    cardTitle.text = "Knight";
        //    infoText.text = "Gives all humans +3+0";
        //    isMonster = true;
        //    canPoison = false;
        //    canBleed = false;
        //    canTarget = false;
        //    OrignalAttack = attack = 2;
        //    OrignalDefence = defence = 1;
        //    attackText.text = attack.ToString();
        //    defenceText.text = defence.ToString();
        //}

        //else if (cardID == 912)
        //{
        //    cardTitle.text = "Spell";
        //    isMonster = false;
        //    canPoison = false;
        //    OrignalAttack = attack = 0;
        //    OrignalDefence = defence = 0;
        //    attackText.text = "";
        //    defenceText.text = "";
        //    infoText.text = "Give '1' defence to monster in front if possible else deal '1' damage to monster on enemy side";
        //}
        //else if (cardID == 103)
        //{
        //    cardTitle.text = "Spell";
        //    isMonster = false;
        //    canPoison = false;
        //    OrignalAttack = attack = 0;
        //    OrignalDefence = defence = 0;
        //    attackText.text = "";
        //    defenceText.text = "";
        //    infoText.text = "If you control a monster deal '2' damage to front enemy monster, else spawn a 0-1 monster ";
        //}
        //else if (cardID == 99)
        //{
        //    cardTitle.text = "Monster";
        //    isMonster = true;
        //    canPoison = false;
        //    canTarget = false;
        //    OrignalAttack = attack = 0;
        //    OrignalDefence = defence = 1;
        //    attackText.text = attack.ToString();
        //    defenceText.text = defence.ToString();
        //    infoText.text = "Slime";

        //}
        //else if (cardID == 98)
        //{
        //    cardTitle.text = "Crawler";
        //    infoText.text = "";
        //    isMonster = true;
        //    canPoison = false;
        //    canTarget = false;
        //    OrignalAttack = attack = 1;
        //    OrignalDefence = defence = 1;
        //    attackText.text = attack.ToString();
        //    defenceText.text = defence.ToString();
        //}
    }

    public override void OnSetTrigger()
    {
        base.OnSetTrigger();

        if (thisCard.cardID == CardName.ZOMBIE)
        {
            Debug.Log("ZOMBIE OVERRIDE WORKS");
        }
        else if (thisCard.cardID == CardName.SKELETON)
        {
            Debug.Log("SKELETON OVERRIDE WORKS");
        }

    }

}
