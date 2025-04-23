using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class SpawnerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform[] spawners;
    public SpawnPattern spawnPattern;
    public Button[] buttonsToUnhide = new Button[2];
    public GameObject[] elementsToHide;

    private float elapsedTime = 0f;
    private int nextSpawnIndex = 0;
    private bool hasSpawnedAll = false;
    private float timeSinceLastSpawn = 0f;
    private const float delayBeforeUnhide = 5f;

    void Start()
    {
        buttonsToUnhide[0].gameObject.SetActive(false);
        buttonsToUnhide[1].gameObject.SetActive(false);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (!hasSpawnedAll)
        {
            while (nextSpawnIndex < spawnPattern.spawnData.Length &&
                   spawnPattern.spawnData[nextSpawnIndex].time <= elapsedTime)
            {
                SpawnProjectile(spawnPattern.spawnData[nextSpawnIndex]);
                nextSpawnIndex++;
                timeSinceLastSpawn = 0f;
            }

            if (nextSpawnIndex >= spawnPattern.spawnData.Length)
            {
                hasSpawnedAll = true;
            }
        }
        else
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= delayBeforeUnhide)
            {
                UnhideButtonsAndHideElements();
            }
        }
    }

    void SpawnProjectile(SpawnData data)
    {
        if (data.spawnerIndex >= 0 && data.spawnerIndex < spawners.Length)
        {
            Instantiate(projectilePrefab, spawners[data.spawnerIndex].position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Invalid spawner index: " + data.spawnerIndex);
        }
    }

    void UnhideButtonsAndHideElements()
    {
        buttonsToUnhide[0].gameObject.SetActive(true);
        buttonsToUnhide[1].gameObject.SetActive(true);

        foreach (var element in elementsToHide)
        {
            element.SetActive(false);
        }

        this.enabled = false;
    }
}