using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class FadeInTMPText : MonoBehaviour 
{
    public float delayBeforeFade = 0.5f;
    public float fadeDuration = 2f;
    public float delayBeforeButton = 0.5f;
    public GameObject buttonToShow;
    public AudioClip soundEffect;
    public float volume = 1f;
    private TextMeshProUGUI tmpText;

    // Inicializace textu "You Died", nastavuje průhlednost a tlačítko
    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        if (tmpText != null) SetTextAlpha(0f);
        if (buttonToShow != null)
        {
            buttonToShow.SetActive(false);
        }
    }

    // Spustí animaci zobrazení textu a přehraje zvuk
    void Start()
    {
        StartCoroutine(FadeInText());
        if (soundEffect != null)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = soundEffect;
            audioSource.volume = volume;
            audioSource.playOnAwake = false;
            SettingsManager settingsManager = FindObjectOfType<SettingsManager>();
            if (settingsManager != null && settingsManager.masterMixer != null)
            {
                AudioMixerGroup[] groups = settingsManager.masterMixer.FindMatchingGroups("Master");
                if (groups.Length > 0)
                {
                    audioSource.outputAudioMixerGroup = groups[0];
                }
            }
            audioSource.Play();
        }
    }

    // Animuje postupné zobrazení textu a následně tlačítka
    IEnumerator FadeInText()
    {
        yield return new WaitForSeconds(delayBeforeFade);
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            if (tmpText != null) SetTextAlpha(alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (tmpText != null) SetTextAlpha(1f);
        yield return new WaitForSeconds(delayBeforeButton);
        if (buttonToShow != null)
        {
            buttonToShow.SetActive(true);
        }
    }

    // Nastaví průhlednost textu
    void SetTextAlpha(float alpha)
    {
        if (tmpText == null) return;
        Color c = tmpText.color;
        c.a = alpha;
        tmpText.color = c;
    }
}