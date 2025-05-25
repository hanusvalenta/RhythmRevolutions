using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy4 : MonoBehaviour
{
    public float speed = 3f;

    void Start()
    {
        
    }

    // Pohybuje nepřítelem směrem nahoru
    public virtual void Move()
    {
        Vector3 newPosition = transform.position;
        newPosition.y += speed * Time.deltaTime;
        newPosition.z = -3.5f;
        transform.position = newPosition;
    }

    void Update()
    {
        Move();
    }

    // Detekuje kolizi s hráčem, způsobí poškození a zničí nepřítele
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTakeDamage();
            Destroy(gameObject);
        }
    }

    // Odebere hráči životy při zásahu
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