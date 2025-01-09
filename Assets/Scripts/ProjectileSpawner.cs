using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform[] spawners;
    public SpawnPattern spawnPattern;

    private float elapsedTime = 0f;
    private int nextSpawnIndex = 0;

    void Update()
    {
        elapsedTime += Time.deltaTime;

        while (nextSpawnIndex < spawnPattern.spawnData.Length &&
               spawnPattern.spawnData[nextSpawnIndex].time <= elapsedTime)
        {
            SpawnProjectile(spawnPattern.spawnData[nextSpawnIndex]);
            nextSpawnIndex++;
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
}
