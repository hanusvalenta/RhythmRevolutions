using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Spike : MonoBehaviour
{
    public AudioClip playerDieSound;
    private SpriteRenderer playerSpriteRenderer;
    private Vector3 originalPosition;
    private float shakeDuration;
    private float shakeIntensity;
    private AudioSource sfxAudioSource;

    // Inicializace, najde audio source pro zvuk smrti hráče
    void Start()
    {
        GameObject sfxManagerObject = GameObject.Find("SFXManager");
        if (sfxManagerObject != null)
        {
            sfxAudioSource = sfxManagerObject.GetComponent<AudioSource>();
        }
    }

    // Detekuje kolizi s hráčem, spustí animaci otřesu, zvuk a případně respawn
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerSpriteRenderer = other.GetComponent<SpriteRenderer>();
            originalPosition = other.transform.position;
            shakeDuration = 0.3f;
            shakeIntensity = 0.1f;
            StartCoroutine(Shake(other.transform));

            GameObject fadeObject = GameObject.Find("FadeAndLoader");

            if (playerDieSound != null && sfxAudioSource != null)
            {
                sfxAudioSource.PlayOneShot(playerDieSound);
            }
            else if (playerDieSound != null)
            {
                AudioSource.PlayClipAtPoint(playerDieSound, Camera.main.transform.position);
            }

            if (fadeObject != null)
            {
                Fade fade = fadeObject.GetComponent<Fade>();
                if (fade != null)
                {
                    fade.FadeIn("DeathScene");
                }
            }
            PlayerHeartControler playerMovement = other.GetComponent<PlayerHeartControler>();
            if(playerMovement != null) playerMovement.enabled = false;

            GameManager.respawnAtCheckpoint = true;
        }
    }

    // Animuje otřes objektu při zásahu
    IEnumerator Shake(Transform objectToShake)
    {
        Vector3 shakeOriginalPosition = objectToShake.position;
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            objectToShake.position = shakeOriginalPosition + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        objectToShake.position = shakeOriginalPosition;
    }
}