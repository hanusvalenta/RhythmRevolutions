using UnityEngine;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    private LineRenderer holdIndicatorLine;
    private float holdDuration;
    private float holdTimer = 0f;
    private bool isHoldNote = false;
    private Vector3 endPosition;

    // Inicializace projektilu
    void Start()
    {
        endPosition = transform.position;
    }

    // Nastaví projektil jako "hold note" s daným trváním
    public void InitializeHold(float duration)
    {
        isHoldNote = true;
        holdDuration = duration;
        holdIndicatorLine = gameObject.AddComponent<LineRenderer>();
        holdIndicatorLine.positionCount = 2;
        holdIndicatorLine.startWidth = 0.1f;
        holdIndicatorLine.endWidth = 0.1f;
        holdIndicatorLine.startColor = Color.white;
        holdIndicatorLine.endColor = Color.white;
        holdIndicatorLine.useWorldSpace = true;

        Material lineMaterial = new Material(Shader.Find("Unlit/Color"));
        lineMaterial.color = Color.white;
        holdIndicatorLine.material = lineMaterial;
    }

    // Hlavní smyčka pohybu projektilu a logika pro "hold note"
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.x < -10f)
        {
            PlayerTakeDamage();
        }

        if (isHoldNote)
        {
            UpdateHoldIndicator();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isHoldNote)
        {
            HoldSuccessful();
        }
    }

    void UpdateHoldIndicator()
    {
        holdTimer += Time.deltaTime;
        float lineLength = holdDuration * speed;
        Vector3 startPosition = endPosition + Vector3.up * Mathf.Clamp01(holdTimer / holdDuration) * lineLength;

        holdIndicatorLine.SetPosition(0, startPosition);
        holdIndicatorLine.SetPosition(1, endPosition);

        endPosition = transform.position;
    }

    void HoldSuccessful()
    {
        holdIndicatorLine.enabled = false;
        isHoldNote = false;
    }

    // Detekuje zásah cíle, vyhodnocuje úspěch a případně způsobí poškození hráči
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
                PlayerTakeDamage();
                Destroy(gameObject);
            }
        }
    }

    // Odebere hráči životy při zásahu projektilu
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