using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManagerScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogue;
    [SerializeField] private string[] lines;
    public float textSpeed;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        dialogue.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartDialogue()
    {
        index = 0;
        startCouroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        //Animation texte
        foreach (char c in lines[index].ToCharArray())
        {
            dialogue.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

    }
}
