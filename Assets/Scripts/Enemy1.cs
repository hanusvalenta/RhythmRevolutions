using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public int health = 100;

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void Move()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    void Update()
    {
        Move();
    }
}