using UnityEngine;

public class Throwable : MonoBehaviour
{
    public float freezeDuration = 2f;  // How long the killer is frozen
    public bool shouldFreeze = false;  // Does this throwable freeze the killer?

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
}
