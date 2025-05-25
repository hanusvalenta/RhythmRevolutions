using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameObject fightButton;
    public GameObject spareButton;
    public GameObject startButton;

    public GameObject fightSquare;
    public float fightSquareFadeDuration = 3f;
    public GameObject playerHeartPrefab;
    public GameObject heartSpawnPoint;

    public GameObject BossFight;

    public string sceneName = "Game";

    public GameObject fadeObject;

    public string textToShowOnSpare;

    public TextBox textBox;

    // Inicializace menu, nastavuje tlačítka a texty
    void Start()
    {
        if (textBox == null)
        {
            textBox = FindObjectOfType<TextBox>();
        }

        if (GameManager.intoPlayed == true)
        {
            setContinueButton();
        }
    }

    // Nastaví tlačítko pro pokračování ve hře
    public void setContinueButton()
    {
        if (startButton != null)
        {
            TextMeshProUGUI buttonText = startButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "Continue";
                buttonText.fontSize -= 2;
            }
        }
    }

    // Skryje tlačítka v menu
    public void HideButtons()
    {
        fightButton.SetActive(false);
        spareButton.SetActive(false);
    }

    // Spustí akci "spare" pro bosse Patel
    public void SparePattel()
    {
        HideButtons();

        if (textBox != null && !string.IsNullOrEmpty(textToShowOnSpare))
        {
            textBox.ShowText(textToShowOnSpare);
        }

        GameManager.Instance.SparedBoss();

        if (fadeObject != null)
        {
            Fade fade = fadeObject.GetComponent<Fade>();
            if (fade != null)
            {
                fade.FadeIn("Game");
            }
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
    }

    // Spustí boj s bossem
    public void StartFight()
    {
        HideButtons();
        fightSquare.SetActive(true);

        SpriteRenderer sr = fightSquare.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            StartCoroutine(FadeInSquare(sr, fightSquareFadeDuration));
        }
    }

    // Animuje postupné zobrazení čtverce v menu
    private IEnumerator FadeInSquare(SpriteRenderer sr, float duration)
    {
        Color color = sr.color;
        color.a = 0;
        sr.color = color;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / duration);
            sr.color = color;
            yield return null;
        }

        color.a = 1f;
        sr.color = color;

        SpawnPlayerHeart();
    }

    void SpawnPlayerHeart()
    {
        if (playerHeartPrefab != null && heartSpawnPoint != null)
        {
            Instantiate(playerHeartPrefab, heartSpawnPoint.transform.position, Quaternion.identity);
            if (BossFight != null && BossFight.GetComponent<BossFight>() != null)
            {
                BossFight.GetComponent<BossFight>().StartBossFight();
            }
        }
    }

    public void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Discord()
    {
        Application.OpenURL("http://discordapp.com/users/1088489927739703336");
    }
    
    public void ItchIO()
    {
        Application.OpenURL("https://hanusvalenta.itch.io/understory");
    }
}