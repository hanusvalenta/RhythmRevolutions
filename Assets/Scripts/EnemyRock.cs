using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyRock : MonoBehaviour
{
    public float speed = 3f;
    public float overshootDistance = 1f;
    private Transform playerHeart;
    private Vector2 targetPosition;
    private Vector2 overshootPosition;
    private bool isOvershooting = false;

    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;


    void Start()
    {
        GameObject playerHeartObject = GameObject.FindGameObjectWithTag("Player");
        if (playerHeartObject != null)
        {
            playerHeart = playerHeartObject.transform;
            targetPosition = playerHeart.position;
            CalculateOvershootPosition();
        }

        StartCoroutine(DestroyAndSpawn());
    }

    IEnumerator DestroyAndSpawn()
    {
        yield return new WaitForSeconds(1f);

        if (prefab1 != null && spawnPoint1 != null)
        {
            GameObject obj1 = Instantiate(prefab1, spawnPoint1.position, Quaternion.identity);
            obj1.transform.localScale *= 0.5f;
        }
        if (prefab2 != null && spawnPoint2 != null)
        {
            GameObject obj2 = Instantiate(prefab2, spawnPoint2.position, Quaternion.identity);
            obj2.transform.localScale *= 0.5f;
        }
        if (prefab3 != null && spawnPoint3 != null)
        {
            GameObject obj3 = Instantiate(prefab3, spawnPoint3.position, Quaternion.identity);
            obj3.transform.localScale *= 0.5f;
        }

        Destroy(gameObject);
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