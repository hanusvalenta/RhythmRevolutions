using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyInteraction : MonoBehaviour
{
    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby)
        {
            Debug.Log("Player is nearby, waiting for E key...");
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Interacting with Enemy");
                SceneManager.LoadScene("MusicFightScene");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Trigger Entered by: {other.name}");
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Player is near the enemy");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"Trigger Exited by: {other.name}");
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Player left the enemy's area");
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log($"Trigger Stay with: {other.name}");
    }
}
