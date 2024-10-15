using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [HideInInspector] public float health = 100f;
    public string interactText;
    public string[] dialogues;
    private DialogueSystem dialogueSystem;
    [HideInInspector] public bool isStarted = false;
    [SerializeField] private Item item;
    public float rotationSpeed = 2f;
    private Inventory inventory;
    [SerializeField]private int waitTime = 10;
    [SerializeField]private int itemWaitTime = 5;
    private AudioSource audioSource;

    void Start()
    {
        // Ensure the DialogueSystem is set up properly
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            dialogueSystem = player.GetComponent<DialogueSystem>();
            inventory = player.GetComponent<Inventory>();
        }
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        // Optionally handle any updates needed for NPC here
    }

    public void Interact(Transform interacterTransform)
    {
        //PLAY AUDIO
        if (dialogueSystem == null)
        {
            Debug.LogWarning("DialogueSystem is not assigned.");
            return;
        }

        Vector3 direction = (interacterTransform.position - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction * Time.deltaTime * rotationSpeed);

        if (!isStarted)
        {
            StartCoroutine(startDialogues());
        }
    }

    IEnumerator startDialogues(){
        dialogueSystem.isAutoText = true;
        dialogueSystem.StartDialogue(dialogues);
        isStarted = true;
        yield return new WaitForSeconds(itemWaitTime);
        if(!item.IsUnityNull() && !inventory.HasItem(item.itemName)){
            inventory.AddItem(item);
        }
        yield return new WaitForSeconds(waitTime);
        isStarted = false;
    }

    public string getInteractText()
    {
        return interactText;
    }

    public void EndInteraction()
    {
        isStarted = false;
    }

    public void UpdateDialogues(string[] newDialogues)
    {
        dialogues = newDialogues;
    }
}
