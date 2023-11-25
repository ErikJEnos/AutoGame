using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToHand : MonoBehaviour
{

    public List<GameObject> HandList;

    public void AddToHand(GameObject card, int pos)
    {
        SortHand();

        HandList.Insert(pos, card);

        SortHand();
    }

    public void RemovefromHand(GameObject card)
    {
        
        HandList.Remove(card);

        SortHand();
    }

    public void RemoveAllfromHand() 
    {
        for (int x = HandList.Count; x > 0; x--)
        {
            HandList.RemoveAt(0);
        }
    }

    public void SortHand()
    {
        Debug.Log("Sort");

        for (int x = 0; x < HandList.Count; x++)
        {
            if(HandList[x] == null)
            {
                HandList.RemoveAt(x);
            }
            else
            {
                Debug.Log("overlapped");
                HandList[x].GetComponent<DragCard>().pos = x;
                HandList[x].transform.position = new Vector3(gameObject.transform.position.x - 2.0f * x, gameObject.transform.position.y, 0.0f);
                HandList[x].GetComponent<DragCard>().moved = false;

                for (int y = x + 1; y < HandList.Count; y++)
                {
                    if (HandList[x] == HandList[y])
                    {
                        RemovefromHand(HandList[y]);
                    }
                }
            }

          
        }
    }

    public void SplitHand(int pos)
    {

        SortHand();

        for (int x = pos; x < HandList.Count; x++)
        {
            HandList[x].transform.position = new Vector3(HandList[x].transform.position.x - 2.0f, gameObject.transform.position.y, 0.0f);
            HandList[x].GetComponent<DragCard>().moved = true;
        }
    }

    public int ReturnCardPos(GameObject cardToReturn)
    {
        for(int x = 0; x < HandList.Count; x++)
        {
            if(HandList[x].gameObject == cardToReturn)
            {
                return x;
            }
        }

        return 0;

    }

    public void AddToPlayerDeck()
    {
        GameObject player1 = GameObject.Find("Player1"); 


        for(int x = 0; x < HandList.Count; x++)
        {
            player1.GetComponent<Player>().deckOrdered.Add(HandList[x]);
        }


        RemoveAllfromHand();

    }
}
