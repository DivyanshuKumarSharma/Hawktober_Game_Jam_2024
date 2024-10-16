using UnityEngine;

public class PlayerPickupThrow : MonoBehaviour
{
    public float pickupRange = 3f;  // The range in which the player can pick up objects
    public Transform handTransform;  // The position where the object will be held (e.g., player's hand)
    public float throwForce = 10f;  // The force with which the object will be thrown

    private GameObject pickedUpObject = null;  // The object that the player has picked up
    private Rigidbody objectRigidbody;  // The Rigidbody of the picked-up object

    void Update()
    {
        if (pickedUpObject == null)
        {
            // Attempt to pick up an object
            if (Input.GetKeyDown(KeyCode.E))  // Press 'E' to pick up
            {
                TryPickupObject();
            }
        }
        else
        {
            // If holding an object, allow throwing
            if (Input.GetMouseButtonDown(0))  // Left mouse button to throw
            {
                ThrowObject();
            }
        }
    }

    void TryPickupObject()
    {
        // Cast a ray from the player to detect nearby objects within range
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange))
        {
            // Check if the object is pickable (has a Rigidbody)
            if (hit.collider.GetComponent<Rigidbody>() != null)
            {
                PickupObject(hit.collider.gameObject);
            }
        }
    }

    void PickupObject(GameObject obj)
    {
        // Set picked-up object
        pickedUpObject = obj;
        objectRigidbody = pickedUpObject.GetComponent<Rigidbody>();

        // Disable Rigidbody physics while holding the object
        objectRigidbody.isKinematic = true;

        // Attach the object to the player's hand or holding position
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

            // Apply a throwing force
            objectRigidbody.AddForce(transform.forward * throwForce, ForceMode.Impulse);

            // Clear the picked-up object reference
            pickedUpObject = null;
            objectRigidbody = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the pickup range in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
