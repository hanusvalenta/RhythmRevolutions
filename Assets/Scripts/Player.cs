using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;

    private Vector2 movement;

    public GameObject interactionBubble;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (interactionBubble != null)
        {
            interactionBubble.SetActive(false);
        }
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BasicEnemy") && interactionBubble != null)
        {
            interactionBubble.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BasicEnemy") && interactionBubble != null)
        {
            interactionBubble.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("BasicEnemy") && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("BasicEnemyFightScene");
        }
    }
}
