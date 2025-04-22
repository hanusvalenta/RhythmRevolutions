using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public AudioClip MenuMusic;
    public float MusicVolume = 0.5f;

    private void Start()
    {
        if (MenuMusic != null)
        {
            AudioSource MenuMusicSource = gameObject.AddComponent<AudioSource>();
            MenuMusicSource.clip = MenuMusic;
            MenuMusicSource.loop = true;
            MenuMusicSource.volume = MusicVolume;
            MenuMusicSource.Play();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
