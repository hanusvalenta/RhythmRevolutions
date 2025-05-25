using UnityEngine;
 using System.Collections.Generic;

 [CreateAssetMenu(fileName = "NewEnemySpawnEvents", menuName = "Enemy Spawn Events", order = 1)]
 public class EnemySpawnEventList : ScriptableObject
 {
  public List<EnemySpawnEvent> spawnEvents;
 }