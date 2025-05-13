using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy5 : MonoBehaviour
{
    public float speed = 3f;

    void Start()
    {
        
    }

    public virtual void Move()
    {
        Vector3 newPosition = transform.position;
        newPosition.y -= speed * Time.deltaTime;
        newPosition.z = -3.5f;
        transform.position = newPosition;
    }

    void Update()
    {
        Move();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTakeDamage();
            Destroy(gameObject);
        }
    }

    void PlayerTakeDamage()
    {
        GameObject playerHealthTextObject = GameObject.Find("PlayerHealth");
        if (playerHealthTextObject != null)
        {
            HealthText healthText = playerHealthTextObject.GetComponent<HealthText>();
            if (healthText != null)
            {
                healthText.TakeDamage(10);
            }
        }
    }
}