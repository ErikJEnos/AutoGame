using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CanvasObject : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject LoginPanel;
    public static int logined;


    // Update is called once per frame
    void Update()
    {
        if (logined > 0)
        {
            LoginPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
            logined++;
            
            if (logined > 5)
            {
                logined = 0;
            }
        }
    }
 
}
