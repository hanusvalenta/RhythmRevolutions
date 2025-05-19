using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioFader
{
    public static IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float targetVolume, float fadeTime)
    {
        audioSource.volume = 0;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.volume = targetVolume;
    }
}

public class Fade : MonoBehaviour
{
    public Animator fadeAnimator;
    public string fadeInTrigger = "Start";
    public string sceneToLoadName = "Game";
    public float fadeTime = 1f;
    public string gameSceneName = "Game";
    public AudioSource musicAudioSource;

    public void FadeToScene(string sceneToLoad)
    {
        this.sceneToLoadName = sceneToLoad;
        StartCoroutine(PerformFadeWithAudio(sceneToLoad));
    }

    IEnumerator PerformFadeWithAudio(string targetSceneName)
    {
        if (musicAudioSource != null)
        {
            StartCoroutine(AudioFader.FadeOut(musicAudioSource, fadeTime));
        }

        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger(fadeInTrigger);
        }

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

    public void TriggerFadeOutAudio(AudioSource audioSourceToFade, float duration)
    {
        if (audioSourceToFade != null)
        {
            StartCoroutine(AudioFader.FadeOut(audioSourceToFade, duration));
        }
    }

    public void TriggerFadeInAudio(AudioSource audioSourceToFade, float targetVolume, float duration)
    {
        if (audioSourceToFade != null)
        {
            StartCoroutine(AudioFader.FadeIn(audioSourceToFade, targetVolume, duration));
        }
    }

    public void FadeIn(string sceneToLoad)
    {
        FadeToScene(sceneToLoad);
    }
}