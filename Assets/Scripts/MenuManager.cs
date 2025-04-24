using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class MenuManager : MonoBehaviour
{
    public GameObject fightButton;
    public GameObject spareButton;

    public GameObject fightSquare;
    public float fightSquareFadeDuration = 3f;
    public GameObject playerHeartPrefab;
    public Transform heartSpawnPoint;

    public void HideButtons()
    {
        fightButton.SetActive(false);
        spareButton.SetActive(false);
    }

    public void SparePattel()
    {
        HideButtons();
        GameManager.Instance.pattelSpared = true;
    }

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
    }
}