using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Pomocná třída pro plynulé zeslabení a zesílení hudby
public class AudioFader
{
    public static IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        if (audioSource == null || audioSource.volume == 0) yield break;
        float startVolume = audioSource.volume;
        if (fadeTime <= 0) fadeTime = 0.001f;
        while (audioSource.volume > 0.001f)
        {
            audioSource.volume -= startVolume * Time.unscaledDeltaTime / fadeTime;
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.Stop();
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float targetVolume, float fadeTime)
    {
        if (audioSource == null) yield break;
        audioSource.volume = 0;
        if (!audioSource.isPlaying)
        {
            if(audioSource.clip != null) audioSource.Play();
        }
        if (fadeTime <= 0) fadeTime = 0.001f;
        targetVolume = Mathf.Clamp01(targetVolume);
        while (audioSource.volume < targetVolume - 0.001f)
        {
            audioSource.volume += targetVolume * Time.unscaledDeltaTime / fadeTime;
            yield return null;
        }
        audioSource.volume = targetVolume;
    }
}

// Spravuje animace přechodů scén a hudby
public class Fade : MonoBehaviour
{
    public Animator fadeAnimator;
    public string fadeInAnimatorTriggerName = "Start";
    public string sceneToLoadName;
    public float defaultFadeDuration = 1f;
    public string gameSceneName = "Game";
    public AudioSource musicAudioSource;

    public void FadeToSceneWithDefaultDuration(string sceneName)
    {
        FadeToScene(sceneName, defaultFadeDuration);
    }
    
    public void FadeToScene(string sceneName, float duration)
    {
        this.sceneToLoadName = sceneName;
        StartCoroutine(PerformFadeSequence(duration));
    }

    public void FadeIn(string sceneToLoad)
    {
        FadeToScene(sceneToLoad, defaultFadeDuration);
    }

    IEnumerator PerformFadeSequence(float fadeDuration)
    {
        if (musicAudioSource != null && musicAudioSource.isPlaying)
        {
            StartCoroutine(AudioFader.FadeOut(musicAudioSource, fadeDuration));
        }
        else
        {
            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
            if (allAudioSources != null)
            {
                foreach (AudioSource audioSrc in allAudioSources)
                {
                    if (audioSrc.isPlaying)
                    {
                       StartCoroutine(AudioFader.FadeOut(audioSrc, fadeDuration));
                    }
                }
            }
        }
        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger(fadeInAnimatorTriggerName);
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
        float actualFadeTime = Mathf.Max(0.001f, fadeDuration);
        yield return new WaitForSeconds(actualFadeTime);
        if (!string.IsNullOrEmpty(sceneToLoadName))
        {
            SceneManager.LoadScene(sceneToLoadName);
        }
    }

    public void TriggerFadeOutSpecificAudio(AudioSource audioSourceToFade, float duration)
    {
        if (duration <= 0) duration = 0.001f;
        if (audioSourceToFade != null)
        {
            StartCoroutine(AudioFader.FadeOut(audioSourceToFade, duration));
        }
    }

    public void TriggerFadeInSpecificAudio(AudioSource audioSourceToFade, float targetVolume, float duration)
    {
        if (duration <= 0) duration = 0.001f;
        if (audioSourceToFade != null)
        {
            StartCoroutine(AudioFader.FadeIn(audioSourceToFade, targetVolume, duration));
        }
    }
}