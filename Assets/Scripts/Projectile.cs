using UnityEngine;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour
{
    public float speed = 5f; // Speed of the projectile

    void Update()
    {
        // Move the projectile leftward
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Destroy if it goes off-screen
        if (transform.position.x < -10f)
        {
            LoadDeathScene();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Target"))
        {
            Target target = other.GetComponent<Target>();
            if (target != null && target.IsActive)
            {
                Destroy(gameObject); // Destroy projectile if target is active
            }
            else
            {
                LoadDeathScene();
            }
        }
    }

    void LoadDeathScene()
    {
        // Replace "DeathScene" with your actual death scene name
        SceneManager.LoadScene("DeathScene");
    }
}
