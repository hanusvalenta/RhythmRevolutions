using UnityEngine;

[System.Serializable]
public class EnemySpawnEvent
{
    public float time;
    public bool isTextEvent;
    [TextArea]
    public string textToShow;
    public int spawnerIndex;
    public int enemyPrefabIndex;
    public Vector3 rotation;
}