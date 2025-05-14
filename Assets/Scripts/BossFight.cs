using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class BossFight : MonoBehaviour
{
    public List<EnemySpawner> enemySpawners;
    public EnemySpawnEventList spawnEventsList;

    private float elapsedTime = 0f;
    private int nextEventIndex = 0;
    private bool fightStarted = false;
    private bool fightEnded = false;

    public GameObject fadeObject;

    void Update()
    {
        if (!fightStarted)
            return;

        elapsedTime += Time.deltaTime;

        if (nextEventIndex < spawnEventsList.spawnEvents.Count && elapsedTime >= spawnEventsList.spawnEvents[nextEventIndex].time)
        {
            SpawnEnemy(spawnEventsList.spawnEvents[nextEventIndex]);
            nextEventIndex++;
        }

        if (nextEventIndex >= spawnEventsList.spawnEvents.Count && !fightEnded)
        {
            fightEnded = true;
            fightStarted = false;
            StartCoroutine(EndFightSequence());
        }
    }

    public void StartBossFight()
    {
        fightStarted = true;
        fightEnded = false;
        elapsedTime = 0f;
        nextEventIndex = 0;
    }

    void SpawnEnemy(EnemySpawnEvent spawnEvent)
    {
        if (spawnEvent.spawnerIndex >= 0 && spawnEvent.spawnerIndex < enemySpawners.Count)
        {
            enemySpawners[spawnEvent.spawnerIndex].SpawnEnemy(spawnEvent.enemyPrefabIndex, Quaternion.Euler(spawnEvent.rotation));
        }
    }

    private IEnumerator EndFightSequence()
    {
        yield return new WaitForSeconds(5f);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.pattelSpared = false;
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