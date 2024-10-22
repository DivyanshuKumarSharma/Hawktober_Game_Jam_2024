using UnityEngine;

public class Throwable : MonoBehaviour, IInteractable
{
    public float freezeDuration = 2f;  // How long the killer is frozen
    public bool shouldFreeze = false;  // Does this throwable freeze the killer?
    public string objectName;
    private PlayerPickupThrow playerPickupThrow;

    public void Interact(Transform interacterTransform)
    {
        GameObject player = interacterTransform.gameObject;
        playerPickupThrow = player.GetComponent<PlayerPickupThrow>();
        playerPickupThrow.PickupObject(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if we hit the killer
        if (collision.gameObject.TryGetComponent<KillerController>(out KillerController killer))
        {
            if (shouldFreeze)
            {
                // Freeze the killer
                killer.FreezeKiller(freezeDuration);
            }
            else
            {
                // Slow down the killer
                killer.GetHitByThrowable();
            }

            // Destroy the throwable after it hits the killer
            Destroy(gameObject);
        }
    }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }

    public string getInteractText()
    {
        return "Pick up " + objectName;
    }
}
