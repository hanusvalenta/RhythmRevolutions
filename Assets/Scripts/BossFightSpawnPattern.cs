using UnityEngine;

 [System.Serializable]
 public class EnemySpawnEvent
 {
  public float time;
  public int spawnerIndex;
  public int enemyPrefabIndex;
  public Vector3 rotation;
 }