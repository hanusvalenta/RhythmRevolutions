using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{
    public GameObject textBoxUI;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;
    public int maxCharactersPerPage = 200;

    private List<string> pages = new List<string>();
    private int currentPageIndex = 0;
    private bool isTyping = false;
    private bool isDialogueActive = false;

    void Start()
    {
        textBoxUI.SetActive(false);
    }

    void Update()
    {
        if (isDialogueActive && !isTyping)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ShowNextPage();
            }
        }
    }

    public void ShowText(string fullText)
    {
        if (!isTyping)
        {
            pages = SplitIntoPages(fullText, maxCharactersPerPage);
            currentPageIndex = 0;
            StartCoroutine(TypeText(pages[currentPageIndex]));
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        isDialogueActive = true;
        textBoxUI.SetActive(true);
        dialogueText.text = "";

        Time.timeScale = 0f;

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
    }

    void ShowNextPage()
    {
        currentPageIndex++;

        if (currentPageIndex < pages.Count)
        {
            StartCoroutine(TypeText(pages[currentPageIndex]));
        }
        else
        {
            textBoxUI.SetActive(false);
            Time.timeScale = 1f;
            isDialogueActive = false;
        }
    }

    private List<string> SplitIntoPages(string text, int maxChars)
    {
        List<string> result = new List<string>();
        int currentIndex = 0;

        while (currentIndex < text.Length)
        {
            int length = Mathf.Min(maxChars, text.Length - currentIndex);

            int lastSpaceSearchIndex = Mathf.Min(currentIndex + length, text.Length - 1);
            int lastSpace = text.LastIndexOf(' ', lastSpaceSearchIndex);
            if (lastSpace > currentIndex && lastSpace < currentIndex + length)
            {
                length = lastSpace - currentIndex;
            }

            result.Add(text.Substring(currentIndex, length).Trim());
            currentIndex += length;
        }

        return result;
    }
}
