using UnityEngine;

public class Box : MonoBehaviour
{
    private bool isCarried = false;
    private Player player;
    private Transform playerTransform;
    public float interactionDistance = 2f;
    public AudioClip pickUpSound;
    [Range(0f, 1f)]
    public float pickUpVolume = 1f;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isCarried)
            {
                DropBox();
            }
            else
            {
                TryPickUpBox();
            }
        }

        if (isCarried && playerTransform != null)
        {
            transform.position = playerTransform.position;
        }
    }

    void TryPickUpBox()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            if (Vector3.Distance(transform.position, playerObject.transform.position) <= interactionDistance)
            {
                player = playerObject.GetComponent<Player>();
                playerTransform = playerObject.transform;

                if (player != null && !player.hasBox)
                {
                    isCarried = true;
                    player.hasBox = true;
                    transform.SetParent(playerTransform);
                    transform.localPosition = Vector3.zero;
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }

                    if (pickUpSound != null && audioSource != null)
                    {
                        audioSource.PlayOneShot(pickUpSound, pickUpVolume);
                    }
                }
            }
        }
    }

    void DropBox()
    {
        if (player != null)
        {
            player.hasBox = false;
        }
        isCarried = false;
        transform.SetParent(null);
        player = null;
        playerTransform = null;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.gravityScale = 0f;
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}