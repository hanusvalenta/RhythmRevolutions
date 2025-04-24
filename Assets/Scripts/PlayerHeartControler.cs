using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeartControler : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        movement = movement.normalized;

        rb.velocity = movement * moveSpeed;
    }

    void FixedUpdate()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Barrier")
        {
            rb.velocity = Vector2.zero;

            Vector2 normal = collision.contacts[0].normal;

            Vector2 currentVelocity = rb.velocity;

            Vector2 slideVelocity = currentVelocity - Vector2.Dot(currentVelocity, normal) * normal;
            rb.velocity = slideVelocity;
        }
    }

      void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Barrier")
        {
             Vector2 normal = collision.contacts[0].normal;

            Vector2 currentVelocity = rb.velocity;

            Vector2 slideVelocity = currentVelocity - Vector2.Dot(currentVelocity, normal) * normal;
            rb.velocity = slideVelocity;
        }
    }
}
