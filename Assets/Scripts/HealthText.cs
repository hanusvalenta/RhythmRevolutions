using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class HealthText : MonoBehaviour
{
    private TextMeshProUGUI healthText;
    public int playerHealth = 30;
    public string deathSceneName = "DeathScene";

    public GameObject fadeObject;

    public SpriteRenderer playerSpriteRenderer;
    private Vector3 originalPosition;
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 0.2f;

    public Sprite playerHeartDeadSprite;

    public AudioClip playerDieSound;

    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        UpdateHealthText();

        if (playerSpriteRenderer != null)
        {
            originalPosition = playerSpriteRenderer.transform.position;
        }
    }

    void UpdateHealthText()
    {
        healthText.text = "Health: " + playerHealth;
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        UpdateHealthText();

        if (playerSpriteRenderer != null)
        {
            StartCoroutine(Shake());
        }

        if (playerHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameObject playerHeart = GameObject.FindWithTag("Player");

        if (playerHeart != null)
        {
            SpriteRenderer heartRenderer = playerHeart.GetComponent<SpriteRenderer>();
            if (heartRenderer != null && playerHeartDeadSprite != null)
            {
                heartRenderer.sprite = playerHeartDeadSprite;
            }

            Rigidbody2D rb = playerHeart.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
            }

            MonoBehaviour[] scripts = playerHeart.GetComponents<MonoBehaviour>();
            foreach (var script in scripts)
            {
                if (script != this)
                {
                    script.enabled = false;
                }
            }
        }

        if (playerDieSound != null)
        {
            AudioSource.PlayClipAtPoint(playerDieSound, Camera.main.transform.position);
        }

        if (fadeObject != null)
        {
            Fade fade = fadeObject.GetComponent<Fade>();
            if (fade != null)
            {
                fade.FadeIn(deathSceneName);
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