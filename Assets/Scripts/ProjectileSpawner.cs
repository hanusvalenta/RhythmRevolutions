using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class SpawnerController : MonoBehaviour
{
       public GameObject projectilePrefab;
       public Transform[] spawners;
       public SpawnPattern spawnPattern;
       public Button[] buttonsToUnhide = new Button[2];
       public GameObject[] elementsToHide;

       private float elapsedTime = 0f;
       private int nextSpawnIndex = 0;
       private bool hasSpawnedAll = false;
       private float timeSinceLastSpawn = 0f;
       private const float delayBeforeUnhide = 5f;

       private GameObject currentProjectile;
       private float holdStartTime;
       private float holdEndTime;
       private bool isHolding = false;

       public TextBox textBox;

       void Start()
       {
              buttonsToUnhide[0].gameObject.SetActive(false);
              buttonsToUnhide[1].gameObject.SetActive(false);

              if (textBox == null)
              {
                     GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                     if (playerObject != null)
                     {
                            textBox = playerObject.GetComponent<TextBox>();
                     }
              }
       }

       void Update()
       {
              elapsedTime += Time.deltaTime;

              if (!hasSpawnedAll)
              {
                     while (nextSpawnIndex < spawnPattern.spawnData.Length &&
                            spawnPattern.spawnData[nextSpawnIndex].time <= elapsedTime)
                     {
                            SpawnEvent(spawnPattern.spawnData[nextSpawnIndex]);
                            nextSpawnIndex++;
                            timeSinceLastSpawn = 0f;
                     }

                     if (nextSpawnIndex >= spawnPattern.spawnData.Length || GameManager.Instance.skipNotes)
                     {
                            hasSpawnedAll = true;
                     }
              }
              else
              {
                     timeSinceLastSpawn += Time.deltaTime;

                     if (timeSinceLastSpawn >= delayBeforeUnhide)
                     {
                            UnhideButtonsAndHideElements();
                     }
              }

              if (isHolding)
              {
                     if (elapsedTime >= holdEndTime)
                     {
                            EndHold();
                     }
              }
       }

       void SpawnEvent(SpawnData data)
       {
              if (data.isTextEvent)
              {
                     if (textBox != null)
                     {
                            textBox.ShowText(data.textToShow);
                     }
              }
              else
              {
                     if (data.spawnerIndex >= 0 && data.spawnerIndex < spawners.Length)
                     {
                            currentProjectile = Instantiate(projectilePrefab, spawners[data.spawnerIndex].position, Quaternion.identity);

                            if (data.holdNote)
                            {
                                   isHolding = true;
                                   holdStartTime = elapsedTime;
                                   holdEndTime = elapsedTime + data.holdDuration;
                                   Projectile projectileComponent = currentProjectile.GetComponent<Projectile>();
                                   if (projectileComponent != null)
                                   {
                                          projectileComponent.InitializeHold(data.holdDuration);
                                   }
                            }
                     }
              }
       }


       void EndHold()
       {
              isHolding = false;
              currentProjectile = null;
       }

       void UnhideButtonsAndHideElements()
       {
              buttonsToUnhide[0].gameObject.SetActive(true);
              buttonsToUnhide[1].gameObject.SetActive(true);

              foreach (var element in elementsToHide)
              {
                     element.SetActive(false);
              }

              this.enabled = false;
       }
}