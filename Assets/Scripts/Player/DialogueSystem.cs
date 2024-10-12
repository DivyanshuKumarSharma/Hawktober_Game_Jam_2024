using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialoguePanel; 
    public TextMeshProUGUI dialogueText; 
    private Queue<string> sentences;
    [HideInInspector] public bool isDone = false;
    public bool isAutoText = false;
    [HideInInspector] public bool alreadyStarted = false;
    [SerializeField] private int waitTime = 2;
    

    void Start() 
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false); 
    }

    public void StartDialogue(string[] dialogue)
    {
        alreadyStarted = true;
        isDone = false;
        sentences.Clear();

        foreach (string sentence in dialogue)
        {
            sentences.Enqueue(sentence);
        }

        dialoguePanel.SetActive(true); 
        
        if(isAutoText){
            StartCoroutine(displayAuto());
        }else{
            DisplayNextSentence();
        }
    }

    IEnumerator displayAuto(){

        while(sentences.Count != 0){
            string sentence = sentences.Dequeue();
            dialogueText.text = sentence;
            yield return new WaitForSeconds(waitTime);
        }

        EndDialogue();
        isAutoText = false;
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    public void EndDialogue()
    {
        isDone = true;
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
        alreadyStarted = false;
    }

}
