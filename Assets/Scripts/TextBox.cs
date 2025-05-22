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
    public float speakingVolume = 0.01f;
    private List<string> pages = new List<string>();
    private int currentPageIndex = 0;
    private bool isTyping = false;
    private bool isDialogueActive = false;
    public AudioClip typingSound;
    private AudioSource sfxAudioSource;

    void Start()
    {
        textBoxUI.SetActive(false);
        if (textBoxUI.TryGetComponent<CanvasGroup>(out var cg))
        {
            cg.blocksRaycasts = false;
        }
        else
        {
            var cgNew = textBoxUI.AddComponent<CanvasGroup>();
            cgNew.blocksRaycasts = false;
        }
        dialogueText.text = "";

        GameObject sfxManagerObject = GameObject.Find("SFXManager");
        if (sfxManagerObject != null)
        {
            sfxAudioSource = sfxManagerObject.GetComponent<AudioSource>();
        }
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
        if (isTyping && Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine("TypeText");
            dialogueText.text = pages[currentPageIndex];
            isTyping = false;
        }
    }

    public void ShowText(string fullText)
    {
        if (isTyping)
        {
             StopCoroutine("TypeText");
             dialogueText.text = pages[currentPageIndex];
             isTyping = false;
        }

        pages = SplitIntoPages(fullText, maxCharactersPerPage);
        currentPageIndex = 0;
        if (pages.Count > 0)
        {
            StartCoroutine(TypeText(pages[currentPageIndex]));
        }
        else
        {
            textBoxUI.SetActive(false);
            isDialogueActive = false;
            Time.timeScale = 1f; 
        }
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        isDialogueActive = true;
        textBoxUI.SetActive(true);
        dialogueText.text = "";

        if(Time.timeScale > 0f) Time.timeScale = 0f;

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            if (typingSound != null && sfxAudioSource != null)
            {
                sfxAudioSource.PlayOneShot(typingSound, speakingVolume);
            }
            else if (typingSound != null)
            {
                AudioSource.PlayClipAtPoint(typingSound, Camera.main.transform.position, speakingVolume);
            }
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
            dialogueText.text = "";
            Time.timeScale = 1f;
            isDialogueActive = false;
        }
    }

    private List<string> SplitIntoPages(string text, int maxChars)
    {
        List<string> result = new List<string>();
        if (string.IsNullOrEmpty(text)) return result;
        int currentIndex = 0;
        while (currentIndex < text.Length)
        {
            int length = Mathf.Min(maxChars, text.Length - currentIndex);
            int subEndIndex = currentIndex + length;
            if (subEndIndex < text.Length && !char.IsWhiteSpace(text[subEndIndex -1]) && !char.IsWhiteSpace(text[subEndIndex]))
            {
                int lastSpace = text.LastIndexOf(' ', subEndIndex -1 , length);
                if (lastSpace > currentIndex)
                {
                    length = lastSpace - currentIndex;
                }
            }
            result.Add(text.Substring(currentIndex, length).Trim());
            currentIndex += length;
            while(currentIndex < text.Length && char.IsWhiteSpace(text[currentIndex]))
            {
                currentIndex++;
            }
        }
        return result;
    }
}