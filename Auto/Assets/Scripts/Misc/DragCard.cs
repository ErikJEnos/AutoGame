using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCard : MonoBehaviour
{
    public List<GameObject> objectsEntered;
    public GameObject hand;
    public int pos = -1;
    public bool inArray = false;
    public bool moved = false;
    private bool callOnce = false;

    private void Update()
    {
        if (hand != null)
        {
            if (objectsEntered.Count <= 0 && !inArray)
            {
                hand.GetComponent<DragToHand>().SortHand();
            }

            else if (objectsEntered.Count > 0 && !inArray)
            {
                int highestNumber = 0;

                for (int x = 0; x < objectsEntered.Count; x++)
                {
                    if (highestNumber <= hand.GetComponent<DragToHand>().ReturnCardPos(objectsEntered[x]))
                    {
                        highestNumber = hand.GetComponent<DragToHand>().ReturnCardPos(objectsEntered[x]);
                    }
                }

                hand.GetComponent<DragToHand>().SplitHand(highestNumber);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.GetComponent<DragToHand>())
        {
            hand = collision.gameObject;
        }
        else if (collision.gameObject.GetComponent<DragCard>().inArray!)
        {
            objectsEntered.Add(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(hand != null)
        {
            if (gameObject.GetComponent<CardDrag>().released && !callOnce)
            {
                inArray = true;
                
                if(objectsEntered.Count > 0)
                {

                    int highestNumber = 0;

                    for (int x = 0; x < objectsEntered.Count; x++)
                    {
                        if (highestNumber <= hand.GetComponent<DragToHand>().ReturnCardPos(objectsEntered[x]) && objectsEntered[x].GetComponent<DragCard>().inArray)
                        {
                            highestNumber = hand.GetComponent<DragToHand>().ReturnCardPos(objectsEntered[x]);
                        }
                    }

                    hand.GetComponent<DragToHand>().AddToHand(gameObject, highestNumber);

                }
                else
                {
                    hand.GetComponent<DragToHand>().HandList.Add(gameObject);
                    hand.GetComponent<DragToHand>().SortHand();

                }

                objectsEntered.Remove(collision.gameObject);

                callOnce = true;
            }

            else if (hand.GetComponent<DragToHand>() && !gameObject.GetComponent<CardDrag>().released)
            {
                if (inArray == true)
                {
                    inArray = false;

                    hand.GetComponent<DragToHand>().RemovefromHand(gameObject);
                }

                callOnce = false;

            }
        } 
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DragToHand>())
        {
            hand.GetComponent<DragToHand>().SortHand();
            inArray = false;
            hand = null;
        }else 
        {
            objectsEntered.Remove(collision.gameObject);
        }
    }

}
