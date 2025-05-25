using UnityEngine;

 [System.Serializable]
 public class SpawnData
 {
     public float time;
     public int spawnerIndex;
     public bool holdNote;
     public float holdDuration;

     [Header("Text Event")]
     public bool isTextEvent;
     [TextArea]
     public string textToShow;
 }

 [CreateAssetMenu(fileName = "NewSpawnPattern", menuName = "Spawn Pattern", order = 1)]
 public class SpawnPattern : ScriptableObject
 {
     public SpawnData[] spawnData;
 }