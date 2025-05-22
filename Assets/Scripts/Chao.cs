using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chao : MonoBehaviour
{
    public string fadeOutText = "You've been defeated by Chao! Returning to checkpoint...";
    public float fadeDuration = 1f;
    public string noBoxText = "You need the box to defeat me! Go find it!";

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                if (player.hasBox)
                {
                    return;
                }
                StartCoroutine(HandlePlayerDefeat(player));
            }
        }
    }

    IEnumerator HandlePlayerDefeat(Player player)
    {
        player.canMove = false;

        if (!player.hasBox)
        {
            if (player.textBox != null)
            {
                player.textBox.ShowText(noBoxText);
            }
            yield return new WaitForSeconds(fadeDuration);
            player.MoveToNearestRespawnPoint();
            player.canMove = true;
            yield break;
        }

        if (player.textBox != null)
        {
            player.textBox.ShowText(fadeOutText);
        }
        
        yield return new WaitForSeconds(fadeDuration);

        player.MoveToNearestRespawnPoint();

        player.canMove = true;
    }
}