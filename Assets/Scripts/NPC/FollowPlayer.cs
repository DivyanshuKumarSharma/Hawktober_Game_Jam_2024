using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    public Transform[] patrolPoints;  // Array of points where the killer will patrol
    private int currentPatrolIndex = 0;
    private NavMeshAgent agent;
    public float speed = 3f;
    public bool isChasingPlayer = false;  // Track if the killer is currently chasing the player
    private GameObject player;  // Cached reference to the player
    private KillerController killerController;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>().gameObject;  // Cache the player reference
        killerController = GetComponent<KillerController>();  // Get the reference to KillerController
        Patrol();  // Start the patrol routine
    }

    void Update()
    {
        if (isChasingPlayer)
        {
            ChasePlayer();  // Continuously update chase logic
        }
        else
        {
            Patrol();
        }
    }

    // Continuously update chase logic when chasing the player
    private void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // If the player escapes far enough, stop chasing and resume patrol
        if (distanceToPlayer > killerController.runDetectionRadius)
        {
            StopChasingPlayer();
            Debug.Log("Player escaped, resuming patrol.");
        }
        else
        {
            // Continuously update the killer's destination to the player's current position
            agent.SetDestination(player.transform.position);
        }
    }

    // Patrol between waypoints
    public void Patrol()
    {
        if (patrolPoints.Length == 0) return;  // If no patrol points are set, do nothing

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // Move to the next patrol point when near the current one
            GoToNextPatrolPoint();
        }
    }

    // Move to the next patrol point in the list
    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        // Set the agent's destination to the next patrol point
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;  // Loop through patrol points
    }

    // Start chasing the player
    public void StartChasingPlayer(Vector3 playerPosition)
    {
        isChasingPlayer = true;
        agent.SetDestination(playerPosition);  // Set initial player position as the destination
    }

    // Stop chasing and return to patrol
    public void StopChasingPlayer()
    {
        isChasingPlayer = false;
        Patrol();  // Resume patrolling after losing the player
    }
}
