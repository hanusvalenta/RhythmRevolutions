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
        rb.velocity = movement * moveSpeed;
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
        transform.Translate(direction * pushDistance);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("MatthewPatel") && Input.GetKeyDown(KeyCode.E))
        {
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