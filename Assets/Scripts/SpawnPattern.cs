using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public float time;
    public int spawnerIndex;
}

[CreateAssetMenu(fileName = "NewSpawnPattern", menuName = "Spawn Pattern", order = 1)]
public class SpawnPattern : ScriptableObject
{
    public SpawnData[] spawnData;
}
