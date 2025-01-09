using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float spawnInterval = 2f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnProjectile), 0f, spawnInterval);
    }

    void SpawnProjectile()
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }
}