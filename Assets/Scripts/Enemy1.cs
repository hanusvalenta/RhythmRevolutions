 using UnityEngine;
 

 public class Enemy : MonoBehaviour
 {
  public float speed = 3f;
 

  private Transform playerHeart;
  private Vector2 targetPosition;
 

  void Start()
  {
  GameObject playerHeartObject = GameObject.FindGameObjectWithTag("Player");
    if (playerHeartObject != null)
    {
    playerHeart = playerHeartObject.transform;
    targetPosition = playerHeart.position;
    }
  }
 

  public virtual void Move()
  {
    if (playerHeart != null)
    {
    Vector3 newPosition = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    newPosition.z = -3.5f;
    transform.position = newPosition;
    }
  }
 

  void Update()
  {
    Move();
  }
 }
