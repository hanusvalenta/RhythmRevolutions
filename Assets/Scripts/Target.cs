using UnityEngine;


public class Target : MonoBehaviour
{
    public static float deactivationSuppressedUntil = 0f;
    public bool IsActive { get; private set; } = false;
    public KeyCode activationKey;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateColor();
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
        float now = Time.time;
        if (now < deactivationSuppressedUntil)
        {
            float delay = deactivationSuppressedUntil - now;
            Invoke(nameof(Deactivate), delay);
            return;
        }
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