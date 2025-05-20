using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public List<GameObject> interactionBubbles = new List<GameObject>();
    public List<string> allowedMovementTags = new List<string>() { "Ground" };
    private bool canMove = true;
    public GameObject fadeObject;
    private Vector2 currentVelocity;
    public TextBox textBox;
    private Collider2D playerCollider;
    public SpriteRenderer playerSpriteRenderer;
    public Sprite idleSprite;
    public Sprite moveUpSprite1;
    public Sprite moveUpSprite2;
    public Sprite moveUpSprite3;
    public Sprite moveDownSprite1;
    public Sprite moveDownSprite2;
    public Sprite moveDownSprite3;
    public Sprite moveLeftSprite1;
    public Sprite moveLeftSprite2;
    public Sprite moveLeftSprite3;
    public Sprite moveRightSprite1;
    public Sprite moveRightSprite2;
    public Sprite moveRightSprite3;
    private float animationTimer = 0f;
    public float animationSpeed = 0.1f;
    private int currentFrame = 0;
    private Vector2 lastMovement = Vector2.zero;
    public string gameSceneName = "Game";

    public AudioClip backgroundMusicClip;
    private AudioSource audioSource;

    public float loopVolume = 0.01f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        
        if (playerSpriteRenderer == null)
        {
            playerSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        audioSource = GetComponent<AudioSource>();

        if (backgroundMusicClip != null && audioSource != null)
        {
            audioSource.clip = backgroundMusicClip;
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.volume = loopVolume;
            audioSource.Play();
        }

        if (GameManager.Instance != null && GameManager.lastPlayerPosition.HasValue)
        {
            if (SceneManager.GetActiveScene().name == gameSceneName &&
                GameManager.lastPlayerScene == gameSceneName)
            {
                transform.position = GameManager.lastPlayerPosition.Value;
                
                GameManager.lastPlayerPosition = null;
                GameManager.lastPlayerScene = null;
            }
            else if (GameManager.lastPlayerScene != gameSceneName)
            {
                GameManager.lastPlayerPosition = null;
                GameManager.lastPlayerScene = null;
            }
        }

        if (interactionBubbles != null)
        {
            foreach (var bubble in interactionBubbles)
            {
                if (bubble != null)
                    bubble.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (canMove)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            movement = Vector2.zero;
        }

        movement = movement.normalized;

        if (Camera.main != null)
        {
            Camera.main.transform.position = Vector3.Lerp(
                Camera.main.transform.position,
                new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z),
                Time.deltaTime * 5f
            );
        }

        if (playerSpriteRenderer != null)
        {
            if (movement.magnitude > 0)
            {
                animationTimer += Time.deltaTime;

                if (movement != lastMovement)
                {
                    animationTimer = 0f;
                    currentFrame = 0;
                    lastMovement = movement;
                }

                if (animationTimer >= animationSpeed)
                {
                    animationTimer = 0f;
                    currentFrame = (currentFrame + 1) % 3; 

                    if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y)) 
                    {
                        if (movement.x < 0) 
                        {
                            if (currentFrame == 0) playerSpriteRenderer.sprite = moveLeftSprite1;
                            else if (currentFrame == 1) playerSpriteRenderer.sprite = moveLeftSprite2;
                            else playerSpriteRenderer.sprite = moveLeftSprite3;
                        }
                        else 
                        {
                            if (currentFrame == 0) playerSpriteRenderer.sprite = moveRightSprite1;
                            else if (currentFrame == 1) playerSpriteRenderer.sprite = moveRightSprite2;
                            else playerSpriteRenderer.sprite = moveRightSprite3;
                        }
                    }
                    else 
                    {
                        if (movement.y > 0) 
                        {
                            if (currentFrame == 0) playerSpriteRenderer.sprite = moveUpSprite1;
                            else if (currentFrame == 1) playerSpriteRenderer.sprite = moveUpSprite2;
                            else playerSpriteRenderer.sprite = moveUpSprite3;
                        }
                        else 
                        {
                            if (currentFrame == 0) playerSpriteRenderer.sprite = moveDownSprite1;
                            else if (currentFrame == 1) playerSpriteRenderer.sprite = moveDownSprite2;
                            else playerSpriteRenderer.sprite = moveDownSprite3;
                        }
                    }
                }
            }
            else
            {
                playerSpriteRenderer.sprite = idleSprite; 
                currentFrame = 0;
                animationTimer = 0f;
                lastMovement = Vector2.zero;
            }
        }
    }

    void FixedUpdate()
    {
        rb.velocity = Vector2.SmoothDamp(rb.velocity, movement * moveSpeed, ref currentVelocity, 0.05f); 
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!allowedMovementTags.Contains(collision.tag))
        {
            canMove = false; 
            ResolveCollision(collision); 
        }

        if (allowedMovementTags.Contains(collision.tag) && interactionBubbles != null)
        {
            foreach (var bubble in interactionBubbles)
            {
                if (bubble != null)
                    bubble.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!allowedMovementTags.Contains(collision.tag))
        {
            canMove = true;
        }

        if (allowedMovementTags.Contains(collision.tag) && interactionBubbles != null)
        {
            foreach (var bubble in interactionBubbles)
            {
                if (bubble != null)
                    bubble.SetActive(false);
            }
        }
    }
    
    void ResolveCollision(Collider2D collision)
    {
        if (playerCollider == null || collision == null) return;

        ColliderDistance2D colliderDistance = playerCollider.Distance(collision);

        if (colliderDistance.isOverlapped)
        {
            Vector2 directionToPush = (Vector2)transform.position - colliderDistance.pointB; 
            if (directionToPush == Vector2.zero) 
            {
                directionToPush = Vector2.up; 
            }
            transform.Translate(directionToPush.normalized * 0.01f); 
        }
        rb.velocity = Vector2.zero; 
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("MatthewPatel") && Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.Instance.PatelFought == true || GameManager.Instance.PatelSpared == true)
            {
                if (textBox != null)
                {
                    textBox.ShowText("What do you want, Scott? I already fought you.");
                }
            }

            else
            {
                if (textBox != null)
                {
                    textBox.ShowText("So, you’ve come to fight me? Let's see what you've got.");
                }

                if (fadeObject != null)
                {
                    Fade fade = fadeObject.GetComponent<Fade>();
                    if (fade != null)
                    {
                        fade.FadeIn("Patel");
                    }
                }
            }
        }

        if (collision.CompareTag("Wallace") && Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.Instance.WallaceSpared == true || GameManager.Instance.WallaceFought == true)
            {
                if (textBox != null)
                {
                    textBox.ShowText("Were done here, Scott. Now go find Ramona.");
                }
            }

            else
            {
                if (textBox != null)
                {
                    textBox.ShowText("Hey, Scott! Im here to help you out. Lets learn some moves to find Ramona.");
                }

                if (fadeObject != null)
                {
                    Fade fade = fadeObject.GetComponent<Fade>();
                    if (fade != null)
                    {
                        fade.FadeIn("Wallace");
                    }
                }
            }
        }

        if (collision.CompareTag("Knives") && Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.Instance.KnivesSpared == true || GameManager.Instance.KnivesFought == true)
            {
                if (textBox != null)
                {
                    textBox.ShowText("You defeated me, Scott. I won't fight you again.");
                }
            }

            else
            {
                if (textBox != null)
                {
                    textBox.ShowText("You wont get past me this time, Scott! Ramona is mine!");
                }

                if (fadeObject != null)
                {
                    Fade fade = fadeObject.GetComponent<Fade>();
                    if (fade != null)
                    {
                        fade.FadeIn("Knives");
                    }
                }
            }
        }

        if (collision.CompareTag("Lucas") && Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.Instance.LucasSpared == true || GameManager.Instance.LucasFought == true)
            {
                if (textBox != null)
                {
                    textBox.ShowText("I admit youre cooler than me, Scott. But I won't fight you again.");
                }
            }

            else
            {
                if (textBox != null)
                {
                    textBox.ShowText("Ready to fight, Scott? I am the coolest of them all!");
                }

                if (fadeObject != null)
                {
                    Fade fade = fadeObject.GetComponent<Fade>();
                    if (fade != null)
                    {
                        fade.FadeIn("Lucas");
                    }
                }
            }
        }
        
        if (collision.CompareTag("Cat") && Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.Instance.CatInteracted == true)
            {
                if (textBox != null)
                {
                    textBox.ShowText("Meaow. Again...");
                }
            }

            else
            {
                if (textBox != null)
                {
                    textBox.ShowText("Meaow.");
                }

                GameManager.Instance.CatInteracted = true;
            }
        }
    }
}