using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class CardUI : MonoBehaviour
{

    public Text textinfo;
    public TMP_Text damageInfoText;
    public TMP_Text cardTitle;
    public TMP_Text attackText;
    public TMP_Text defenceText;
    public TMP_Text infoText;
    public TMP_Text levelText;

    public GameObject pDebuff;
    public GameObject bDebuff;

    void Update()
    {
        if (gameObject.GetComponent<Card>().isPoisoned)
        {
            pDebuff.SetActive(true);
        }
        else if (gameObject.GetComponent<Card>().isPoisoned == false)
        {
            pDebuff.SetActive(false);
        }

        if (gameObject.GetComponent<Card>().isBleeding)
        {
            bDebuff.SetActive(true);
        }
        else if (gameObject.GetComponent<Card>().isBleeding == false)
        {
            bDebuff.SetActive(false);
        }
    }
}
