using UnityEngine;
 using System.Collections.Generic;

 public class BossFight : MonoBehaviour
 {
  public List<EnemySpawner> enemySpawners;
  public EnemySpawnEventList spawnEventsList;

  private float elapsedTime = 0f;
  private int nextEventIndex = 0;
  private bool fightStarted = false;

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

   if (nextEventIndex >= spawnEventsList.spawnEvents.Count)
   {
    // Fight is complete
    fightStarted = false;
    Debug.Log("Boss fight over");
   }
  }

  public void StartBossFight()
  {
   fightStarted = true;
   elapsedTime = 0f;
   nextEventIndex = 0;
  }

  void SpawnEnemy(EnemySpawnEvent spawnEvent)
  {
   if (spawnEvent.spawnerIndex >= 0 && spawnEvent.spawnerIndex < enemySpawners.Count)
   {
    enemySpawners[spawnEvent.spawnerIndex].SpawnEnemy(spawnEvent.enemyPrefabIndex, Quaternion.Euler(spawnEvent.rotation));
   }
   else
   {
    Debug.LogError("Invalid spawner index: " + spawnEvent.spawnerIndex);
   }
  }
 }