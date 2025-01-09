using UnityEngine;

public class Target : MonoBehaviour
{
    public bool IsActive { get; private set; } = false; // Is the target activated?
    public KeyCode activationKey; // Assign specific key for this target

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

        // Deactivate after a short time
        Invoke(nameof(Deactivate), 0.5f); // Adjust duration if necessary
    }

    void Deactivate()
    {
        IsActive = false;
    }
}
