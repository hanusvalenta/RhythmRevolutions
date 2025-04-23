using UnityEngine;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

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
                Destroy(gameObject);
            }
            else
            {
                LoadDeathScene();
            }
        }
    }

    void LoadDeathScene()
    {
        SceneManager.LoadScene("DeathScene");
    }
}
