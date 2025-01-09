using UnityEngine;

public class Target : MonoBehaviour
{
    public bool IsActive { get; private set; } = false; // Is the target activated?
    public KeyCode activationKey; // Assign specific key for this target
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the Target object!");
        }
        else
        {
            UpdateColor(); // Set the initial color
        }
    }

    void Update()
    {
        // Activate the target when the key is pressed
        if (Input.GetKeyDown(activationKey))
        {
            Activate();
        }
    }

    void Activate()
    {
        IsActive = true;
        UpdateColor();

        // Deactivate after a short time
        Invoke(nameof(Deactivate), 0.5f); // Adjust duration if necessary
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
            // Set color based on the activation state
            spriteRenderer.color = IsActive ? Color.green : Color.red;
        }
    }
}