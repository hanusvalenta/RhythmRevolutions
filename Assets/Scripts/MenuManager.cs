using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject fightButton;
    public GameObject spareButton;

    public GameObject fightSquare;
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
    }
}