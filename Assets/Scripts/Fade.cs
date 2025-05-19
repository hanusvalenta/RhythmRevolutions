using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{

    public Animator fadeAnimator;

    public string fadeInTrigger = "Start";
    public string sceneToLoadName = "Game";

    public float fadeTime = 1f;
    
    public string gameSceneName = "Game";

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void FadeIn(string sceneToLoad)
    {
        this.sceneToLoadName = sceneToLoad;
        StartCoroutine(PerformFade(sceneToLoad));
    }

    IEnumerator PerformFade(string targetSceneName)
    {
        fadeAnimator.SetTrigger(fadeInTrigger);

        if (GameManager.Instance != null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null && SceneManager.GetActiveScene().name == gameSceneName) 
            {
                GameManager.lastPlayerPosition = playerObject.transform.position;
                GameManager.lastPlayerScene = SceneManager.GetActiveScene().name;
            }
        }

        yield return new WaitForSeconds(fadeTime);

        SceneManager.LoadScene(targetSceneName);
    }
}