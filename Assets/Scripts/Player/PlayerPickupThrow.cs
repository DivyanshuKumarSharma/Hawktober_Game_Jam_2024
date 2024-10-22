using System.Collections;
using UnityEngine;

public class PlayerPickupThrow : MonoBehaviour
{
    public float pickupRange = 3f;  // The range in which the player can pick up objects
    public Transform handTransform;  // The position where the object will be held (e.g., player's hand)
    public float throwForce = 10f;  // The force with which the object will be thrown

    private GameObject pickedUpObject = null;  // The object that the player has picked up
    private Rigidbody objectRigidbody;  // The Rigidbody of the picked-up object
    private PlayerInteraction playerInteraction;
    private PlayerController  playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>(); 
       playerInteraction = GetComponent<PlayerInteraction>(); 
    }

    void Update()
    {
        if (pickedUpObject != null)
        {
            if (Input.GetMouseButtonDown(0))  // Left mouse button to throw
            {
                ThrowObject();
            }
        }
    }

    public void PickupObject(GameObject obj)
    {
        Debug.Log("Pickup Object");
        // Set picked-up object
        pickedUpObject = obj;
        objectRigidbody = pickedUpObject.GetComponent<Rigidbody>();

        // Disable Rigidbody physics while holding the object
        objectRigidbody.isKinematic = true;

        // Attach the object to the player's hand or holding position
        pickedUpObject.GetComponent<Collider>().enabled = false;
        pickedUpObject.transform.SetParent(handTransform);
        pickedUpObject.transform.localPosition = Vector3.zero;  // Align the object with the hand's position
        pickedUpObject.transform.localRotation = Quaternion.identity;  // Reset rotation
    }

void ThrowObject()
{
    if (pickedUpObject != null)
    {
        // Detach the object from the player
        pickedUpObject.transform.SetParent(null);

        // Enable Rigidbody physics
        objectRigidbody.isKinematic = false;

        // Apply a throwing force in the appropriate direction
        Vector3 throwDirection = new Vector3(0, 0.5f, 0); // Default upward push
        
        if (playerController.moveX > 0) // Walking right
        {
            Debug.Log("Moving right");
            throwDirection = transform.right + new Vector3(0, 0.5f, 0); // Right with an upward push
        }
        else if (playerController.moveX < 0) // Walking left
        {
            Debug.Log("Moving left");
            throwDirection = -transform.right + new Vector3(0, 0.5f, 0); // Left with an upward push
        }

        // Ignore collision between the player and the object temporarily
        Collider playerCollider = GetComponent<Collider>();
        Collider objectCollider = pickedUpObject.GetComponent<Collider>();
        
        if (playerCollider != null && objectCollider != null)
        {
            Physics.IgnoreCollision(playerCollider, objectCollider, true);
        }

        // Apply the throw force
        objectRigidbody.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);

        // Re-enable the collider after the throw
        if (objectCollider != null)
        {
            objectCollider.enabled = true;
        }

        // Re-enable collision between player and object after a delay
        StartCoroutine(ReEnableCollision(playerCollider, objectCollider, 0.5f));

        // Clear the picked-up object reference
        pickedUpObject = null;
        objectRigidbody = null;
        playerInteraction.isInteracting = false;
    }
}

    private IEnumerator ReEnableCollision(Collider playerCollider, Collider objectCollider, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (playerCollider != null && objectCollider != null)
        {
            Physics.IgnoreCollision(playerCollider, objectCollider, false);
        }
    }


    private void OnDrawGizmosSelected()
    {
        // Visualize the pickup range in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
