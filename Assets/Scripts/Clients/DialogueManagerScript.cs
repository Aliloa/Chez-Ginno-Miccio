using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManagerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogue;
    [SerializeField] private string[] lines;
    [SerializeField] private float textSpeed;
    [SerializeField] private AudioSource speakSound;

    private int index;

    public bool isDialogueActive = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        dialogue.text = string.Empty;
        //StartDialogue();
    }

    public void StartDialogue(string[] newLines)
    {
        lines = newLines;
        isDialogueActive = true;
        dialogue.text = string.Empty;
        gameObject.SetActive(true);
        index = 0;
        StartCoroutine(TypeLine());
        speakSound.Play();
    }

    IEnumerator TypeLine()
    {
        //Animate the text
        foreach (char c in lines[index].ToCharArray())
        {
            dialogue.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

    }

    public void NextLine()
    {
        if (dialogue.text != lines[index])
        {
            StopAllCoroutines();
            dialogue.text = lines[index];
        }
        else if (index < lines.Length - 1)
        {
            index++;
            dialogue.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            isDialogueActive = false;
            gameObject.SetActive(false);
        }
    }
}
