using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float PlayerHealth = 30;

    public bool PatelSpared = false;
    public bool PatelFought = false;

    public bool WallaceSpared = false;
    public bool WallaceFought = false;

    public static Vector3? lastPlayerPosition;
    public static string lastPlayerScene;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FoughtBoss()
    {
        string bossName = SceneManager.GetActiveScene().name;

        var sparedField = GetType().GetField(bossName + "Spared");
        var foughtField = GetType().GetField(bossName + "Fought");
        if (sparedField != null)
            sparedField.SetValue(this, false);
        if (foughtField != null)
            foughtField.SetValue(this, true);
    }

    public void SparedBoss()
    {
        string bossName = SceneManager.GetActiveScene().name;

        var sparedField = GetType().GetField(bossName + "Spared");
        var foughtField = GetType().GetField(bossName + "Fought");
        if (sparedField != null)
        {
            sparedField.SetValue(this, true);
        }
            
        if (foughtField != null)
        {
            foughtField.SetValue(this, false);
        }
    }
}