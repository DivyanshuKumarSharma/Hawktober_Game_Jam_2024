using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 2f;
    private Inventory inventory;

    public GameObject playerInteractionUI;
    [SerializeField] private TextMeshProUGUI interactText;
    [HideInInspector]public bool isInteracting = false;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        playerInteractionUI.SetActive(false);
    }

    void Update()
    {   
        getInteractUI();
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactionDistance);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact(transform);
                    if (collider.TryGetComponent(out Item item))
                    {
                        inventory.AddItem(item);
                        Debug.Log("Picked up: " + item.itemName);
                    }
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
