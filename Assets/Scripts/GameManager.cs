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

    public bool KnivesSpared = false;
    public bool KnivesFought = false;

    public bool LucasSpared = false;
    public bool LucasFought = false;

    public bool GideonSpared = false;
    public bool GideonFought = false;

    public bool CatInteracted = false;

    public static Vector3? lastPlayerPosition;
    public static string lastPlayerScene;

    public static Vector3? spawnPlayerPosition;

    public static bool intoPlayed = false;

    public bool skipNotes = false;
    public bool skipFight = false;

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