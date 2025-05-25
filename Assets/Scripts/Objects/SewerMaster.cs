using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewerMaster : MonoBehaviour
{
    public List<Sever> severs;
    public TextBox textBox;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Kontroluje, zda jsou všechny "severy" ve stavu jedna, případně udělí klíč
    void Update()
    {
        CheckAllSeversInStateOne();
    }

     void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ResetAllSevers();
        }
    }

    // Resetuje všechny "severy" do výchozího stavu
    public void ResetAllSevers()
    {
        foreach (Sever sever in severs)
        {
            sever.ResetStates();
        }
    }

    // Kontroluje, zda jsou všechny "severy" ve stavu jedna, pokud ano, udělí hráči klíč
    private void CheckAllSeversInStateOne()
    {
        bool allInStateOne = true;
        foreach (Sever sever in severs)
        {
            if (!sever.isStateOneTriggered)
            {
                allInStateOne = false;
                break;
            }
        }

        if (allInStateOne && gameManager != null && !gameManager.hasKey1)
        {
            gameManager.hasKey1 = true;
            if (textBox != null)
            {
                textBox.ShowText("You got a key!");
            }
            this.enabled = false;
        }
    }
}