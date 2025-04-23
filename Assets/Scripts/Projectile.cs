using UnityEngine;
 using UnityEngine.SceneManagement;

 public class Projectile : MonoBehaviour
 {
  public float speed = 5f;
  private LineRenderer holdIndicatorLine;
  private float holdDuration;
  private float holdTimer = 0f;
  private bool isHoldNote = false;
  private Vector3 startPosition;

  void Start()
  {
   startPosition = transform.position;
  }

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

   // Create a simple white material in code
   Material lineMaterial = new Material(Shader.Find("Unlit/Color")); // Use Unlit/Color shader
   lineMaterial.color = Color.white;
   holdIndicatorLine.material = lineMaterial;
  }

  void Update()
  {
   transform.Translate(Vector2.down * speed * Time.deltaTime);

   if (transform.position.x < -10f)
   {
    LoadDeathScene();
   }

   if (isHoldNote)
   {
    UpdateHoldIndicator();
   }
  }

  void UpdateHoldIndicator()
  {
   holdTimer += Time.deltaTime;
   float lineLength = holdDuration * speed;
   Vector3 endPosition = startPosition + Vector3.down * Mathf.Clamp01(holdTimer / holdDuration) * lineLength;

   holdIndicatorLine.SetPosition(0, startPosition);
   holdIndicatorLine.SetPosition(1, endPosition);
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