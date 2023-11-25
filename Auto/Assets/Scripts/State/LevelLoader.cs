using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public Animator transition;
    public int x;
    public float time = 1f;

    public static GameObject ob;
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

    public void LoadNextLevel()
    {

        x = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine("LoadLevel");
    }

    public void LoadMainMenuLevel()
    {

        x = SceneManager.GetActiveScene().buildIndex - 1;
        StartCoroutine("LoadLevel");
    }


    IEnumerator LoadLevel()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(x);

        yield return new WaitForSeconds(time);

        transition.SetTrigger("End");
    }

    IEnumerator DestroyOnLoad()
    {
       yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
