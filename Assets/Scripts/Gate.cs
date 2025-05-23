using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    public string barrierText = "You need a key to pass here! Find the key first!";
    public string openText = "The gate opens!";
    public float textDisplayDuration = 2f;
    public int keyNumberToCheck;
    public Sprite openedGateSprite;

    private SpriteRenderer spriteRenderer;
    private Collider2D gateCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gateCollider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                bool hasRequiredKey = false;
                if (GameManager.Instance != null)
                {
                    string keyFieldName = "hasKey" + keyNumberToCheck;
                    var keyField = typeof(GameManager).GetField(keyFieldName);

                    if (keyField != null && (bool)keyField.GetValue(GameManager.Instance))
                    {
                        hasRequiredKey = true;
                    }
                }

                if (hasRequiredKey)
                {
                    StartCoroutine(OpenGate(player));
                }
                else
                {
                    StartCoroutine(HandleBarrierInteraction(player));
                }
            }
        }
    }

    IEnumerator HandleBarrierInteraction(Player player)
    {
        if (player.textBox != null)
        {
            player.textBox.ShowText(barrierText);
        }
        
        yield return new WaitForSeconds(textDisplayDuration);

        player.MoveToNearestRespawnPoint();
    }

    IEnumerator OpenGate(Player player)
    {
        if (player.textBox != null)
        {
            player.textBox.ShowText(openText);
        }

        if (spriteRenderer != null && openedGateSprite != null)
        {
            spriteRenderer.sprite = openedGateSprite;
        }

        if (gateCollider != null)
        {
            gateCollider.enabled = false;
        }

        yield return new WaitForSeconds(textDisplayDuration);
    }
}