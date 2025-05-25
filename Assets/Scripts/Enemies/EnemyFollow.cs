using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy2 : MonoBehaviour
{
  public float speed = 5f;
  public float lifeTime = 3f;

  private Transform playerHeart;
  private float timer;

  void Start()
  {
    GameObject playerHeartObject = GameObject.FindGameObjectWithTag("Player");
    if (playerHeartObject != null)
    {
      playerHeart = playerHeartObject.transform;
    }

    timer = 0f;
  }

  void Update()
  {
    Move();
    timer += Time.deltaTime;

    if (timer >= lifeTime)
    {
      Destroy(gameObject);
    }
  }

  public virtual void Move()
  {
    if (playerHeart != null)
    {
      Vector3 newPosition = Vector2.MoveTowards(transform.position, playerHeart.position, speed * Time.deltaTime);
      newPosition.z = -3.5f;
      transform.position = newPosition;
    }
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