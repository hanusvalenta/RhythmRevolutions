using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FadeInTMPText : MonoBehaviour
{
    public float delayBeforeFade = 0.5f;
    public float fadeDuration = 2f;

    private TextMeshProUGUI tmpText;

    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        SetTextAlpha(0f);
    }

    public AudioClip soundEffect;

    void Start()
    {
        StartCoroutine(FadeInText());

        if (soundEffect != null)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = soundEffect;
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
    }

    void SetTextAlpha(float alpha)
    {
        Color c = tmpText.color;
        c.a = alpha;
        tmpText.color = c;
    }
}
