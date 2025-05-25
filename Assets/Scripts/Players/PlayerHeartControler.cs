using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeartControler : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    public float minX = -2f;
    public float maxX = 2f;
    public float minY = -3.6f;
    public float maxY = -0.15f;

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

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            transform.position.z
        );
    }
}