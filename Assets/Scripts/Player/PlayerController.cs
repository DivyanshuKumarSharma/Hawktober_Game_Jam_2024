using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public int moveSpeed;
    public int jumpSpeed = 5;
    public int runSpeed = 30;
    public int walkSpeed = 10;
    public int crouchSpeed = 5;
    [SerializeField] private Rigidbody rbody;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.15f;
    [SerializeField] private float jumpHeight = 10f;
    [HideInInspector] public Animator animator;
    public bool isRunning;
    public float moveX;
    public float moveZ;
    public bool canRun = false;
    public bool isCrouching = false;  // New variable to track crouching

    private PlayerHealth playerHealth;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player = GameObject.FindGameObjectWithTag("Player");
        rbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
        move();
        jump();
    }

    void move()
    {
        if (isGrounded)
        {
            moveX = Input.GetAxis("Horizontal"); 
            moveZ = Input.GetAxis("Vertical");    

            bool isRunningAttempt = Input.GetKey(KeyCode.LeftShift) && canRun;
            isRunning = isRunningAttempt && playerHealth.playerStamina > 0;

            // Crouch logic
            if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
            {
                isCrouching = true;
                moveSpeed = crouchSpeed;
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                isCrouching = false;
            }

            if (isRunning)
            {
                playerHealth.isRunning = true;
                moveSpeed = runSpeed;
            }
            else if (!isRunning && !isCrouching)
            {
                playerHealth.isRunning = false;
                moveSpeed = walkSpeed;
            }

            Vector3 input = new Vector3(moveX, 0, moveZ).normalized;
            player.transform.position = transform.position + input * Time.deltaTime * moveSpeed;

            // Update the player's animations based on movement state
            UpdateAnimations(moveX, moveZ);
        }
    }

    void UpdateAnimations(float moveX, float moveZ)
    {
        //play animations of sprites
        animator.SetBool("isWalking_left", moveX > 0);
        animator.SetBool("isWalking_right", moveX < 0);
        animator.SetBool("isWalking", moveZ > 0);
        animator.SetBool("isWalking_back", moveZ < 0);
    }

    void jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("jump");
            rbody.velocity = new Vector3(moveX * jumpSpeed, jumpHeight, moveZ * jumpSpeed);
        }
    }

    public void setAnimIdle()
    {
        animator.SetBool("isWalking_left", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isWalking_right", false);
        animator.SetBool("isWalking_back", false);
    }
}

