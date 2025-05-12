using UnityEngine;
 using UnityEngine.SceneManagement;
 
 public class Spike : MonoBehaviour
 {
  void OnTriggerEnter2D(Collider2D other)
  {
  if (other.CompareTag("Player"))
  {
  SceneManager.LoadScene("DeathScene");
  }
  }
 }