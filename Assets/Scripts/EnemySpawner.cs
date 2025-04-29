using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;

    public GameObject SpawnEnemy(int enemyIndex, Quaternion rotation)
    {
        if (enemyIndex >= 0 && enemyIndex < enemyPrefabs.Count)
        {
            return Instantiate(enemyPrefabs[enemyIndex], transform.position, rotation);
        }
        else
        {
            Debug.LogError("Invalid enemy index: " + enemyIndex + " for spawner: " + gameObject.name);
            return null;
        }
    }
}