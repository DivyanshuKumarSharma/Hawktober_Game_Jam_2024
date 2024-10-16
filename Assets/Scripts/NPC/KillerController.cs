using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerController : MonoBehaviour
{
    public float slowedSpeed = 1f;  // Killer's slowed movement speed
    public float slowDuration = 5f;  // How long the killer stays slowed
    private bool isSlowed = false;  // Is the killer currently slowed or frozen?

    private float currentSpeed;  // Killer's current movement speed
    private bool isFrozen = false;  // If the killer is frozen completely

    private Transform playerTransform;
    private FollowPlayer followPlayer;

    public float catchDistance = 2f;  // The distance at which the killer catches the player
    private bool hasCaughtPlayer = false;
    private PlayerController playerController;  // Reference to the player's controller (movement, etc.)
    private Animator playerAnimator;  // Reference to the player's Animator for death animation

    public float deathDelay = 2f;  // Time to delay before "killing" the player after the animation
    public string deathAnimationTrigger = "Die"; 
    private PlayerHealth playerHealth;

    void Start()
    {
        followPlayer = GetComponent<FollowPlayer>();
        currentSpeed = followPlayer.speed;
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerController = player.GetComponent<PlayerController>();
            playerAnimator = player.GetComponent<Animator>();
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if(!isFrozen){
            if (!hasCaughtPlayer && Vector3.Distance(transform.position, playerTransform.position) <= catchDistance)
            {
                CatchPlayer();
            }
        }
        
    }


    // This method will be called when the killer gets hit by a throwable
    public void GetHitByThrowable()
    {
        if (!isSlowed)
        {
            StartCoroutine(SlowKiller());
        }
    }

    // Coroutine to slow the killer down for a certain duration
    private IEnumerator SlowKiller()
    {
        isSlowed = true;
        followPlayer.speed = slowedSpeed;  // Apply slowed speed

        yield return new WaitForSeconds(slowDuration);

        // After the slow duration ends, restore the normal speed
        followPlayer.speed = currentSpeed ;
        isSlowed = false;
    }

    // This method can be called to freeze the killer (e.g., for a complete stop)
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
        followPlayer.speed = 0;
        //PLAY ANIMATIONS
        //PLAY SOUNDS
        yield return new WaitForSeconds(duration);
        isFrozen = false;  // Resume movement after the freeze duration
        followPlayer.speed = currentSpeed;
    }

    private void CatchPlayer()
    {
        hasCaughtPlayer = true;

        // Kill the player after the death animation plays
        playerHealth.waitForDeath = deathDelay;
        StartCoroutine(playerHealth.Die());
    }


}
