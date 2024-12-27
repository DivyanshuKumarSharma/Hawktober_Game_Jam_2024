using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
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
    private AudioSource audioSource;
    public GameObject[] objectsToEnable; 
    private PlayerController playerController;
    private PlayerInteraction playerInteraction;
    private Animator animator;

    void Start()
    {
        // Ensure the DialogueSystem is set up properly
        GameObject player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        animator.StartPlayback();
        if (player != null)
        {
            dialogueSystem = player.GetComponent<DialogueSystem>();
            playerController = player.GetComponent<PlayerController>();
            inventory = player.GetComponent<Inventory>();
            playerInteraction = player.GetComponent<PlayerInteraction>();
        }
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(false);
        }
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        // Optionally handle any updates needed for NPC here
        if(dialogueSystem.isDone){
            playerController.enabled = true; 
            playerInteraction.isInteracting = false;
            if(objectsToEnable != null){
                foreach (GameObject obj in objectsToEnable)
                {
                    obj.SetActive(true);
                }
                objectsToEnable = null;
            }
        }
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
        playerController.setAnimIdle();
        playerController.enabled = false; 
        dialogueSystem.isAutoText = true;
        dialogueSystem.StartDialogue(dialogues);
        isStarted = true;
        yield return new WaitForSeconds(waitTime);
        if(!item.IsUnityNull() && !inventory.HasItem(item.itemName)){
            inventory.AddItem(item);
        }
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
