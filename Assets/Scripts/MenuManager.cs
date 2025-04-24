using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject fightButton;
    public GameObject spareButton;

    public RectTransform fightSquare;
    public Vector2 expandedSquareSize = new Vector2(200, 200);
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

        squareToExpand.sizeDelta = expandedSquareSize;

        if (playerHeartPrefab != null)
        {
            Vector3 spawnPosition = heartSpawnPoint != null ? heartSpawnPoint.position : squareToExpand.position;
            Instantiate(playerHeartPrefab, spawnPosition, Quaternion.identity);
        }
    }
}