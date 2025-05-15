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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerSpriteRenderer = other.GetComponent<SpriteRenderer>();
            originalPosition = other.transform.position;
            shakeDuration = 0.3f;
            shakeIntensity = 0.1f;
            StartCoroutine(Shake());

            GameObject fadeObject = GameObject.Find("FadeAndLoader");

            if (playerDieSound != null)
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
        }
    }

    IEnumerator Shake()
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;

            playerSpriteRenderer.transform.position = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        playerSpriteRenderer.transform.position = originalPosition;
    }
}