using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 2f;
    private Inventory inventory;

    public GameObject playerInteractionUI;
    [SerializeField] private TextMeshProUGUI interactText;
    public bool isInteracting = false;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        // playerInteractionUI.SetActive(false);
    }

    void Update()
    {   
        getInteractUI();
        
        if (Input.GetKeyDown(KeyCode.E) && !isInteracting)
        {
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactionDistance);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out IInteractable interactable))
                {
                    isInteracting = true; // Start interaction
                    interactable.Interact(transform); // Trigger dialogue or interaction

                    if (collider.TryGetComponent(out Item item))
                    {
                        inventory.AddItem(item);
                        Debug.Log("Picked up: " + item.itemName);
                    }
                    if (collider.TryGetComponent(out Throwable throwable))
                    {
                        Debug.Log("Picked up throwable: " + throwable.objectName);
                    }

                    // Interaction ends automatically in DialogueSystem now, no need to reset isInteracting here.
                }
            }
        }
    }


    public IInteractable GetInteractable()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactionDistance);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                return interactable;
            }
        }
        return null;
    }

    private void getInteractUI(){

        if (isInteracting) 
        {
            // If the player is interacting, hide the UI
            playerInteractionUI.SetActive(false);
            return;
        }

        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactionDistance);
        bool interactableFound = false;

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                playerInteractionUI.SetActive(true);
                interactText.text = interactable.getInteractText();
                interactableFound = true;

            }

            if(!interactableFound){
                playerInteractionUI.SetActive(false);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}
