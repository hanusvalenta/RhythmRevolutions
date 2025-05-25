using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossBarrier : MonoBehaviour
{
    public string barrierText = "You can't pass here yet! Defeat or spare the boss first!";
    public float textDisplayDuration = 2f;
    public string bossNameToCheck;

    // Kontroluje, zda hráč splnil podmínky pro průchod bariérou (porazil/omilostnil bosse)
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                bool bossDefeatedOrSpared = false;
                if (GameManager.Instance != null && !string.IsNullOrEmpty(bossNameToCheck))
                {
                    var sparedField = typeof(GameManager).GetField(bossNameToCheck + "Spared");
                    var foughtField = typeof(GameManager).GetField(bossNameToCheck + "Fought");

                    if (sparedField != null && (bool)sparedField.GetValue(GameManager.Instance))
                    {
                        bossDefeatedOrSpared = true;
                    }

                    if (foughtField != null && (bool)foughtField.GetValue(GameManager.Instance))
                    {
                        bossDefeatedOrSpared = true;
                    }
                }

                if (bossDefeatedOrSpared)
                {
                    Destroy(gameObject);
                    return;
                }
                StartCoroutine(HandleBarrierInteraction(player));
            }
        }
    }

    // Zobrazí text hráči při pokusu o průchod bariérou
    IEnumerator HandleBarrierInteraction(Player player)
    {
        if (player.textBox != null)
        {
            player.textBox.ShowText(barrierText);
        }
        
        yield return new WaitForSeconds(textDisplayDuration);

        player.MoveToNearestRespawnPoint();
    }
}