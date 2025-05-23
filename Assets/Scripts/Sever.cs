using UnityEngine;

public class Sever : MonoBehaviour
{
    public bool isStateOneTriggered = false;
    public bool isStateTwoTriggered = false;
    public int collisionCount = 0;

    void Awake()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collisionCount++;

            if (collisionCount == 1)
            {
                isStateOneTriggered = true;
                isStateTwoTriggered = false;
            }
            else if (collisionCount >= 2)
            {
                isStateOneTriggered = false;
                isStateTwoTriggered = true;
            }
        }
    }

    public void ResetStates()
    {
        collisionCount = 0;
        isStateOneTriggered = false;
        isStateTwoTriggered = false;
    }
}