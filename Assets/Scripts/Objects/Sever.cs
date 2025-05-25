using UnityEngine;

public class Sever : MonoBehaviour
{
    public bool isStateOneTriggered = false;
    public bool isStateTwoTriggered = false;
    public int collisionCount = 0;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collisionCount++;

            if (collisionCount == 1)
            {
                isStateOneTriggered = true;
                isStateTwoTriggered = false;
                DimSpriteStateOne();
            }
            else if (collisionCount >= 2)
            {
                isStateOneTriggered = false;
                isStateTwoTriggered = true;
                DimSpriteStateTwo();
            }
        }
    }

    public void ResetStates()
    {
        collisionCount = 0;
        isStateOneTriggered = false;
        isStateTwoTriggered = false;
        ResetSpriteDimming();
    }

    private void DimSpriteStateOne()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.8f, 0.8f, 0.8f, 1f);
        }
    }

    private void DimSpriteStateTwo()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
    }

    private void ResetSpriteDimming()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}