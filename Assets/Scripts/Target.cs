using UnityEngine;

public class Target : MonoBehaviour
{
    public bool IsActive { get; private set; } = false;
    public KeyCode activationKey;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the Target object!");
        }
        else
        {
            UpdateColor();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(activationKey))
        {
            Activate();
        }
    }

    void Activate()
    {
        IsActive = true;
        UpdateColor();

        Invoke(nameof(Deactivate), 0.2f);
    }

    void Deactivate()
    {
        IsActive = false;
        UpdateColor();
    }

    void UpdateColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = IsActive ? Color.gray : Color.white;
        }
    }
}