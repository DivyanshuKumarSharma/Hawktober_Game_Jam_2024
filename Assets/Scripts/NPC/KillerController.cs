using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using UnityEngine;

public class KillerController : MonoBehaviour
{
    // Detection distances for different player states
    public float crouchDetectionRadius = 5f;  // Small radius for crouching detection
    public float walkDetectionRadius = 10f;   // Medium radius for walking detection
    public float runDetectionRadius = 20f;    // Large radius for running detection

    public float slowedSpeed = 1f;  // Killer's slowed movement speed
    public float slowDuration = 5f;  // How long the killer stays slowed
    private bool isSlowed = false;  // Is the killer currently slowed or frozen?
    private bool isFrozen = false;  // Is the killer frozen completely?

    private float currentSpeed;  // Killer's current movement speed
    private float currentDetectionRadius;  // Detection radius based on player movement state

    private Transform playerTransform;
    private PlayerController playerController;
    private FollowPlayer followPlayer;

    public float catchDistance = 2f;  // Distance at which the killer catches the player
    private bool hasCaughtPlayer = false;  // Has the killer caught the player?
    
    private PlayerHealth playerHealth;
    public float deathDelay = 2f;  // Time to delay before "killing" the player after the animation
    public string deathAnimationTrigger = "Die";  // Trigger for the player's death animation

    void Start()
    {
        followPlayer = GetComponent<FollowPlayer>();
        currentSpeed = followPlayer.speed;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerController = player.GetComponent<PlayerController>();
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (!isFrozen) // Only detect player when not frozen
        {
            AdjustDetectionRadius();
            DetectPlayer();

            // Check if the killer is close enough to catch the player
            if (!hasCaughtPlayer && Vector3.Distance(transform.position, playerTransform.position) <= catchDistance)
            {
                CatchPlayer();
            }
        }
    }

    // Adjust detection radius based on player's movement state
    void AdjustDetectionRadius()
    {
        if (playerController.isRunning)
        {
            currentDetectionRadius = runDetectionRadius;
        }
        else if (playerController.isCrouching)
        {
            currentDetectionRadius = crouchDetectionRadius;
        }
        else
        {
            currentDetectionRadius = walkDetectionRadius;
        }
    }

    // Detect if the player is within the killer's detection radius
    void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= currentDetectionRadius)
        {
            // Start chasing the player if within detection radius
            followPlayer.StartChasingPlayer(playerTransform.position);
            Debug.Log("Killer detected the player!");
        }
    }

    // Method to slow down the killer temporarily
    public void GetHitByThrowable()
    {
        if (!isSlowed)
        {
            StartCoroutine(SlowKiller());
        }
    }

    // Coroutine to slow the killer for a certain duration
    private IEnumerator SlowKiller()
    {
        isSlowed = true;
        followPlayer.speed = slowedSpeed;  // Apply slowed speed

        yield return new WaitForSeconds(slowDuration);

        // Restore the killer's speed after the slow duration ends
        followPlayer.speed = currentSpeed;
        isSlowed = false;
    }

    // Method to freeze the killer (stop movement temporarily)
    public void FreezeKiller(float freezeDuration)
    {
        if (!isFrozen)
        {
            StartCoroutine(FreezeKillerCoroutine(freezeDuration));
        }
    }

    private IEnumerator FreezeKillerCoroutine(float duration)
    {
        isFrozen = true;  // Stop all movement
        followPlayer.speed = 0;  // Stop the killer's movement
        // Play freeze animation or effects here
        yield return new WaitForSeconds(duration);
        isFrozen = false;  // Resume movement after the freeze duration
        followPlayer.speed = currentSpeed;  // Reset to normal speed
    }

    // When the killer catches the player
    private void CatchPlayer()
    {
        hasCaughtPlayer = true;

        // Trigger the player's death animation or logic
        playerHealth.waitForDeath = deathDelay;
        StartCoroutine(playerHealth.Die());

        // Play death animation for the player
        Debug.Log("Player caught by the killer!");
    }
}
