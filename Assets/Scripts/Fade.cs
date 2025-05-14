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
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void FadeIn(string sceneToLoadName)
    {
        this.sceneToLoadName = sceneToLoadName;
        StartCoroutine(PerformFade(sceneToLoadName));
    }

    IEnumerator PerformFade(string sceneToLoadName)
    {
        fadeAnimator.SetTrigger(fadeInTrigger);

        yield return new WaitForSeconds(fadeTime);

        SceneManager.LoadScene(sceneToLoadName);
    }
}
