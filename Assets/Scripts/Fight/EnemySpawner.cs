using UnityEngine;
 using System.Collections.Generic;
 

 public class EnemySpawner : MonoBehaviour
 {
  public List<GameObject> enemyPrefabs;
 

  public GameObject SpawnEnemy(int enemyIndex, Quaternion rotation)
  {
    if (enemyIndex >= 0 && enemyIndex < enemyPrefabs.Count)
    {
    Vector3 spawnPosition = transform.position;
    spawnPosition.z = -3.5f;
    return Instantiate(enemyPrefabs[enemyIndex], spawnPosition, rotation);
    }
  return null;
  }
 }