using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FadeInTMPText : MonoBehaviour
{
    public float delayBeforeFade = 0.5f;
    public float fadeDuration = 2f;
    public float delayBeforeButton = 0.5f;
    public GameObject buttonToShow;
    public AudioClip soundEffect;
    public float volume = 1f;

    private TextMeshProUGUI tmpText;

    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        SetTextAlpha(0f);

        if (buttonToShow != null)
        {
            buttonToShow.SetActive(false);
        }
    }

    void Start()
    {
        StartCoroutine(FadeInText());

        if (soundEffect != null)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = soundEffect;
            audioSource.volume = volume;
            audioSource.Play();
        }
    }

    IEnumerator FadeInText()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            SetTextAlpha(alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetTextAlpha(1f);

        yield return new WaitForSeconds(delayBeforeButton);

        if (buttonToShow != null)
        {
            buttonToShow.SetActive(true);
        }
    }

    void SetTextAlpha(float alpha)
    {
        Color c = tmpText.color;
        c.a = alpha;
        tmpText.color = c;
    }
}