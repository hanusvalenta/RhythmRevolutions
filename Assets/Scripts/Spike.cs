using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    
    public AudioClip playerDieSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject fadeObject = GameObject.Find("FadeAndLoader");

            if (playerDieSound != null)
            {
                AudioSource.PlayClipAtPoint(playerDieSound, Camera.main.transform.position);
            }

            if (fadeObject != null)
            {
                Fade fade = fadeObject.GetComponent<Fade>();
                if (fade != null)
                {
                    fade.FadeIn("DeathScene");
                }
            }
        }
    }
}