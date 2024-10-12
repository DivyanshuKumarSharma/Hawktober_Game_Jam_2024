using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class ThinkingDialogueManager : MonoBehaviour
{
    public string[] dialogues;
    private DialogueSystem dialogueSystem;
    private GameObject player;
    [SerializeField] private float waitTime = 3f;
    [HideInInspector] public bool isStarted = false;
    private TaskManager taskManager;
    [SerializeField] private string currentTask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Player"){
            Debug.Log("player found");
            player = other.gameObject;
            dialogueSystem = player.GetComponent<DialogueSystem>();
            taskManager = player.GetComponent<TaskManager>();
            taskManager.UpdateTask(currentTask);
            StartCoroutine(displayDialogues());
       } 
    }

    private IEnumerator displayDialogues(){
        if (!isStarted)
        {
            dialogueSystem.StartDialogue(dialogues);
            isStarted = true;
        }

        while(isStarted){
            yield return new WaitForSeconds(waitTime);
            dialogueSystem.DisplayNextSentence();

            if (dialogueSystem.isDone)
            {
                isStarted = false;
                // Call the method to handle the reveal and scene transition
            }
        }

        this.gameObject.SetActive(false);
    }
}
