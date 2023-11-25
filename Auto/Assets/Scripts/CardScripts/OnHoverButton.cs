using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnHoverButton : MonoBehaviour
{
    Color orignal;
    bool clicked = false;
    bool hover = false;

    private void Start()
    {
        orignal = gameObject.GetComponent<Image>().color;
    }

    public void OnClicked()
    {
        if (!clicked)
        {
            gameObject.GetComponent<Image>().color = Color.blue;
            clicked = true;
        }
        else
        {
            clicked = false;
            gameObject.GetComponent<Image>().color = orignal;
        }
       
    }

    public void OnHoverExit()
    {
        if (!clicked)
        {
            gameObject.GetComponent<Image>().color = orignal;
        }
       
        hover = false;
    }

    public void OnHoverEnter()
    {
        hover = true;
        if (!clicked)
        {
            gameObject.GetComponent<Image>().color = Color.gray;
        }
    }
}
