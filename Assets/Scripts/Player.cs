using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public List<GameObject> interactionBubbles = new List<GameObject>();
    public List<string> allowedMovementTags = new List<string>() { "Ground" };
    public bool canMove = true;
    public GameObject fadeObject;
    private Vector2 currentVelocity;
    public TextBox textBox;
    private Collider2D playerCollider;
    public SpriteRenderer playerSpriteRenderer;
    public Sprite idleSprite;
    public Sprite moveUpSprite1, moveUpSprite2, moveUpSprite3;
    public Sprite moveDownSprite1, moveDownSprite2, moveDownSprite3;
    public Sprite moveLeftSprite1, moveLeftSprite2, moveLeftSprite3;
    public Sprite moveRightSprite1, moveRightSprite2, moveRightSprite3;
    private float animationTimer = 0f;
    public float animationSpeed = 0.1f;
    private int currentFrame = 0;
    private Vector2 lastMovement = Vector2.zero;
    public string gameSceneName = "Game";
    public AudioClip backgroundMusicClip;
    private AudioSource audioSource;
    [Range(0f,1f)]
    public float loopVolume = 0.01f;
    public List<GameObject> barrierRespawnPoints = new List<GameObject>();

    public bool hasBox = false;

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
            if (!audioSource.isPlaying)
            {
                 audioSource.Play();
            }
        }
        if (GameManager.Instance != null && GameManager.lastPlayerPosition.HasValue)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            if (currentSceneName == gameSceneName && GameManager.lastPlayerScene == gameSceneName)
            {
                if (GameManager.spawnPlayerPosition.HasValue)
                {
                    transform.position = GameManager.spawnPlayerPosition.Value;
                    GameManager.spawnPlayerPosition = null;
                }
                else
                {
                     transform.position = GameManager.lastPlayerPosition.Value;
                }
                GameManager.lastPlayerPosition = null; 
                GameManager.lastPlayerScene = null;
            }
            else if (GameManager.lastPlayerScene != currentSceneName)
            {
                GameManager.lastPlayerPosition = null;
                GameManager.lastPlayerScene = null;
                 if (GameManager.spawnPlayerPosition.HasValue && currentSceneName == gameSceneName)
                {
                     transform.position = GameManager.spawnPlayerPosition.Value;
                     GameManager.spawnPlayerPosition = null;
                }
            }
        }
        if (interactionBubbles != null)
        {
            foreach (var bubble in interactionBubbles)
            {
                if (bubble != null) bubble.SetActive(false);
            }
        }
        if (GameManager.Instance != null && !GameManager.intoPlayed && !GameManager.Instance.skipIntro)
        {
            if (textBox != null)
            {
                textBox.ShowText("XNot so long ago...in the mysterious land...of Toronto, Canada...Scott Pilgrim was dating a highschooler.");
            }
            GameManager.intoPlayed = true;
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
        if (SceneManager.GetActiveScene().name == gameSceneName && Input.GetKeyDown(KeyCode.Escape))
        {
            if (fadeObject != null)
            {
                Fade fade = fadeObject.GetComponent<Fade>();
                if (fade != null)
                {
                    GameManager.lastPlayerPosition = transform.position;
                    GameManager.lastPlayerScene = SceneManager.GetActiveScene().name;
                    fade.FadeIn("MainMenu");
                }
            }
        }
        if (Camera.main != null)
        {
            Camera.main.transform.position = Vector3.Lerp(
                Camera.main.transform.position,
                new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z),
                Time.deltaTime * 5f
            );
        }
        AnimatePlayer();
    }
    
    void AnimatePlayer()
    {
        if (playerSpriteRenderer == null) return;
        if (movement.magnitude > 0)
        {
            animationTimer += Time.deltaTime;
            if (movement != lastMovement)
            {
                animationTimer = 0f;
                currentFrame = 0;
            }
            lastMovement = movement;
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

    void FixedUpdate()
    {
        rb.velocity = Vector2.SmoothDamp(rb.velocity, movement * moveSpeed, ref currentVelocity, 0.05f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrier"))
        {
            MoveToNearestRespawnPoint();
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
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!allowedMovementTags.Contains(collision.gameObject.tag) && !collision.gameObject.CompareTag("Barrier"))
        {
            rb.velocity = Vector2.zero;
            ResolveCollision(collision.collider);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!allowedMovementTags.Contains(collision.gameObject.tag) && !collision.gameObject.CompareTag("Barrier"))
        {
             rb.velocity = Vector2.zero;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!allowedMovementTags.Contains(collision.gameObject.tag) && !collision.gameObject.CompareTag("Barrier"))
        {
        }
    }

    public void MoveToNearestRespawnPoint()
    {
        if (barrierRespawnPoints == null || barrierRespawnPoints.Count == 0)
        {
            return;
        }
        GameObject closestPoint = null;
        float minDistance = float.MaxValue;
        foreach(GameObject point in barrierRespawnPoints)
        {
            if (point != null)
            {
                float distance = Vector2.Distance(transform.position, point.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoint = point;
                }
            }
        }
        if (closestPoint != null)
        {
            Vector3 newPosition = closestPoint.transform.position;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
            rb.velocity = Vector2.zero;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
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
            Vector2 directionToPush = colliderDistance.normal * -1;
            if (directionToPush == Vector2.zero)
            {
                directionToPush = ((Vector2)transform.position - (Vector2)collision.transform.position).normalized;
                 if (directionToPush == Vector2.zero) directionToPush = Vector2.up;
            }
            transform.Translate(directionToPush * (colliderDistance.distance + 0.01f));
        }
        rb.velocity = Vector2.zero;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            string sceneToLoad = null;
            string dialogue = null;
            bool conditionMet = false;
            switch (collision.tag)
            {
                case "MatthewPatel":
                    conditionMet = GameManager.Instance != null && (GameManager.Instance.PatelFought || GameManager.Instance.PatelSpared);
                    dialogue = conditionMet ? "What do you want, Scott? I already fought you." : "So, you’ve come to fight me? Let's see what you've got.";
                    if (!conditionMet) sceneToLoad = "Patel";
                    break;
                case "Wallace":
                    conditionMet = GameManager.Instance != null && (GameManager.Instance.WallaceFought || GameManager.Instance.WallaceSpared);
                    dialogue = conditionMet ? "Were done here, Scott. Now go find Ramona." : "Hey, Scott! Im here to help you out. Youre late to meet Ramona in the park. Lets learn some moves before you go tho You might need em.";
                    if (!conditionMet) sceneToLoad = "Wallace";
                    break;
                case "Knives":
                    conditionMet = GameManager.Instance != null && (GameManager.Instance.KnivesFought || GameManager.Instance.KnivesSpared);
                    dialogue = conditionMet ? "You defeated me, Scott. I won't fight you again." : "You wont get past me this time, Scott! Ramona is mine!";
                    if (!conditionMet) sceneToLoad = "Knives";
                    break;
                case "Lucas":
                     conditionMet = GameManager.Instance != null && (GameManager.Instance.LucasFought || GameManager.Instance.LucasSpared);
                    dialogue = conditionMet ? "I admit youre cooler than me, Scott. But I won't fight you again." : "Ready to fight, Scott? I am the coolest of them all!";
                    if (!conditionMet) sceneToLoad = "Lucas";
                    break;
                case "Gideon":
                    conditionMet = GameManager.Instance != null && (GameManager.Instance.GideonFought || GameManager.Instance.GideonSpared);
                    dialogue = conditionMet ? "I lost it all. Especially my money." : "Im the richest and most powerful of them all! You will never defeat me, Scott!";
                    if (!conditionMet) sceneToLoad = "Gideon";
                    break;
                case "Ramona":
                    dialogue = "You did it Scott! You fought all of my exes! Now we can be together! Mwah!";
                    sceneToLoad = "Win";
                    break;
                case "Cat":
                    conditionMet = GameManager.Instance != null && GameManager.Instance.CatInteracted;
                    dialogue = conditionMet ? "Meaow. Again..." : "Meaow.";
                    if (GameManager.Instance != null && !conditionMet) GameManager.Instance.CatInteracted = true;
                    break;
            }
            if (dialogue != null && textBox != null)
            {
                textBox.ShowText(dialogue);
            }
            if (sceneToLoad != null && fadeObject != null)
            {
                if( !(collision.CompareTag("Cat") && conditionMet) ) 
                {
                    GameManager.lastPlayerPosition = transform.position;
                    GameManager.lastPlayerScene = SceneManager.GetActiveScene().name;
                    Fade fade = fadeObject.GetComponent<Fade>();
                    if (fade != null)
                    {
                        fade.FadeIn(sceneToLoad);
                    }
                }
            }
        }
    }
}