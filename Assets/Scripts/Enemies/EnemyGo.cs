using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public float overshootDistance = 1f;
    private Transform playerHeart;
    private Vector2 targetPosition;
    private Vector2 overshootPosition;
    private bool isOvershooting = false;

    void Start()
    {
        GameObject playerHeartObject = GameObject.FindGameObjectWithTag("Player");
        if (playerHeartObject != null)
        {
            playerHeart = playerHeartObject.transform;
            targetPosition = playerHeart.position;
            CalculateOvershootPosition();
        }
    }

    void CalculateOvershootPosition()
    {
        if (playerHeart != null)
        {
            Vector2 direction = (playerHeart.position - transform.position).normalized;
            overshootPosition = targetPosition + direction * overshootDistance;
        }
    }

    public virtual void Move()
    {
        if (playerHeart != null)
        {
            if (!isOvershooting)
            {
                Vector3 newPosition = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                newPosition.z = -3.5f;
                transform.position = newPosition;

                if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
                {
                    isOvershooting = true;
                }
            }
            else
            {
                Vector3 newPosition = Vector2.MoveTowards(transform.position, overshootPosition, speed * Time.deltaTime);
                newPosition.z = -3.5f;
                transform.position = newPosition;

                 if (Vector2.Distance(transform.position, overshootPosition) < 0.1f)
                {
                    Destroy(gameObject);
                }
            }
        }
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