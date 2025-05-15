using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public GameObject interactionBubble;

    public List<string> allowedMovementTags = new List<string>() { "Ground" };

    private bool canMove = true;

    public GameObject fadeObject;

    private Vector2 currentVelocity;

    public TextBox textBox;

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
        if (canMove)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            movement = Vector2.zero;
        }

        movement = movement.normalized;

        Camera.main.transform.position = Vector3.Lerp(
            Camera.main.transform.position,
            new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z),
            Time.deltaTime * 5f
        );
    }

    void FixedUpdate()
    {
        rb.velocity = Vector2.SmoothDamp(rb.velocity, movement * moveSpeed, ref currentVelocity, 0.1f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!allowedMovementTags.Contains(collision.tag))
        {
            canMove = false;
            ResolveCollision(collision);
        }

        if (collision.CompareTag("MatthewPatel") && interactionBubble != null)
        {
            interactionBubble.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!allowedMovementTags.Contains(collision.tag))
        {
            canMove = true;
        }

        if (collision.CompareTag("MatthewPatel") && interactionBubble != null)
        {
            interactionBubble.SetActive(false);
        }
    }

    void ResolveCollision(Collider2D collision)
    {
        Vector2 direction = (transform.position - collision.transform.position).normalized;
        float pushDistance = 0.1f;
        int maxIterations = 10;
        int currentIteration = 0;

        while (collision.IsTouching(GetComponent<Collider2D>()) && currentIteration < maxIterations)
        {
            transform.Translate(direction * pushDistance);
            currentIteration++;
        }

        if (currentIteration >= maxIterations)
        {
            Debug.LogWarning("Player.ResolveCollision:  Maximum push iterations exceeded. Player may still be stuck.");
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("MatthewPatel") && Input.GetKeyDown(KeyCode.E))
        {
            if (textBox != null && !textBox.gameObject.activeSelf)
            {
                textBox.gameObject.SetActive(true);
            }
            textBox.ShowText("So, you’ve come to fight me? Let's see what you've got.");

            if (fadeObject != null)
            {
                Fade fade = fadeObject.GetComponent<Fade>();
                if (fade != null)
                {
                    fade.FadeIn("MatthewPatel");
                }
            }
        }
    }
}