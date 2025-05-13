using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthText : MonoBehaviour
{
    private TextMeshProUGUI healthText;
    public int playerHealth = 30;
    public string deathSceneName = "DeathScene";

    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        UpdateHealthText();
    }

    void UpdateHealthText()
    {
        healthText.text = "Health: " + playerHealth;
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        UpdateHealthText();

        if (playerHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SceneManager.LoadScene(deathSceneName);
    }
}