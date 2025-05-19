using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

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

    void Start()
    {
        if (textBox == null)
        {
            textBox = FindObjectOfType<TextBox>();
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
        if (!fightStarted || fightEnded || endingSequenceHasBegun)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        while (nextEventIndex < spawnEventsList.spawnEvents.Count &&
               elapsedTime >= spawnEventsList.spawnEvents[nextEventIndex].time)
        {
            ProcessSpawnEvent(spawnEventsList.spawnEvents[nextEventIndex]);
            nextEventIndex++;
        }

        if (!endingSequenceHasBegun)
        {
            bool shouldStartEndSequence = false;

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

            if (shouldStartEndSequence)
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

        originalBossPosition = bossSpriteRenderer.transform.localPosition;
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
        fightStarted = false;

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
        }
    }
}