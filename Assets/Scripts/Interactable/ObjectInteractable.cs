using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractable : MonoBehaviour, IInteractable
{
    public string objectName;
    private AudioSource audioSource;

    [SerializeField] private string[] interactionDialogues;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    //Interacting with the object, when the player presses E to interact with the object
    public void Interact(Transform interacterTransform)
    {
        GameObject player = interacterTransform.gameObject;
        if(player != null && player.tag == "Player"){
            DialogueSystem dialogueSystem = player.GetComponent<DialogueSystem>();
            dialogueSystem.isAutoText = true;
            dialogueSystem.StartDialogue(interactionDialogues);
        }
    }

    public string getInteractText()
    {
        return " " + objectName;
    }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }
}
