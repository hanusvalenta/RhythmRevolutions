using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class BossFight : MonoBehaviour
{
    public List<EnemySpawner> enemySpawners;
    public EnemySpawnEventList spawnEventsList;
    public TextBox textBox;
    public GameObject fadeObject;
    public SpriteRenderer bossSpriteRenderer;
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 0.5f;
    private Vector3 originalBossPosition;
    private float elapsedTime = 0f;
    private int nextEventIndex = 0;
    private bool fightStarted = false;
    private bool fightEnded = false;
    private float lastEnemySpawnTime = -1f;
    private bool endingSequenceHasBegun = false;
    public AudioClip bossMusic;
    [Range(0f, 1f)]
    public float musicVolume = 0.75f;
    private AudioSource audioSource;

    void Start()
    {
        if (textBox == null)
        {
            textBox = FindObjectOfType<TextBox>();
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        if (bossMusic != null && audioSource != null)
        {
            audioSource.clip = bossMusic;
            audioSource.loop = true;
            audioSource.volume = musicVolume;
            if (!audioSource.isPlaying && fightStarted)
            {
            }
        }
    }

    public void StartBossFight()
    {
        fightStarted = true;
        fightEnded = false;
        elapsedTime = 0f;
        nextEventIndex = 0;
        endingSequenceHasBegun = false;
        if (bossSpriteRenderer != null)
        {
            originalBossPosition = bossSpriteRenderer.transform.localPosition;
        }
        if (audioSource != null && bossMusic != null && !audioSource.isPlaying)
        {
            audioSource.clip = bossMusic;
            audioSource.loop = true;
            audioSource.volume = musicVolume;
            audioSource.Play();
        }
        lastEnemySpawnTime = -1f;
        if (spawnEventsList != null && spawnEventsList.spawnEvents != null)
        {
            foreach (EnemySpawnEvent spawnEvent in spawnEventsList.spawnEvents)
            {
                if (!spawnEvent.isTextEvent)
                {
                    lastEnemySpawnTime = Mathf.Max(lastEnemySpawnTime, spawnEvent.time);
                }
            }
        }
    }

    void Update()
    {
        if (!fightStarted || fightEnded)
        {
            return;
        }
        if (!endingSequenceHasBegun)
        {
            elapsedTime += Time.deltaTime;
            while (spawnEventsList != null && nextEventIndex < spawnEventsList.spawnEvents.Count &&
                   elapsedTime >= spawnEventsList.spawnEvents[nextEventIndex].time)
            {
                ProcessSpawnEvent(spawnEventsList.spawnEvents[nextEventIndex]);
                nextEventIndex++;
            }
        }
        if (!endingSequenceHasBegun)
        {
            bool shouldStartEndSequence = false;
            if (spawnEventsList != null)
            {
                if (lastEnemySpawnTime >= 0)
                {
                    if (elapsedTime >= lastEnemySpawnTime && nextEventIndex >= spawnEventsList.spawnEvents.Count)
                    {
                        shouldStartEndSequence = true;
                    }
                }
                else
                {
                    if (nextEventIndex >= spawnEventsList.spawnEvents.Count)
                    {
                        shouldStartEndSequence = true;
                    }
                }
            }
            else
            {
                 shouldStartEndSequence = true;
            }
            if (shouldStartEndSequence || (GameManager.Instance != null && GameManager.Instance.skipFight))
            {
                endingSequenceHasBegun = true;
                StartCoroutine(InitiateEndFightDelay(3.0f));
            }
        }
    }

    void ProcessSpawnEvent(EnemySpawnEvent spawnEvent)
    {
        if (spawnEvent.isTextEvent)
        {
            if (textBox != null)
            {
                textBox.ShowText(spawnEvent.textToShow);
            }
        }
        else
        {
            if (spawnEvent.spawnerIndex >= 0 && spawnEvent.spawnerIndex < enemySpawners.Count)
            {
                if (enemySpawners[spawnEvent.spawnerIndex] != null)
                {
                    enemySpawners[spawnEvent.spawnerIndex].SpawnEnemy(spawnEvent.enemyPrefabIndex, Quaternion.Euler(spawnEvent.rotation));
                }
            }
        }
    }

    private IEnumerator ShakeBoss()
    {
        if (bossSpriteRenderer == null)
        {
            yield break;
        }
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            bossSpriteRenderer.transform.localPosition = originalBossPosition + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        bossSpriteRenderer.transform.localPosition = originalBossPosition;
    }

    private IEnumerator InitiateEndFightDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (bossSpriteRenderer != null)
        {
            StartCoroutine(ShakeBoss());
        }
        fightEnded = true;
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.FoughtBoss();
        }
        if (fadeObject != null)
        {
            Fade fade = fadeObject.GetComponent<Fade>();
            if (fade != null)
            {
                fade.FadeIn("Game"); 
            }
            else
            {
                 SceneManager.LoadScene("Game");
            }
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
    }
}