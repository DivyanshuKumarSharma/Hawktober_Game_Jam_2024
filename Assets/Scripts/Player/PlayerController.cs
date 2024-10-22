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
    [SerializeField]
    private Rigidbody rbody;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.15f;
    [SerializeField] private float jumpHeight = 10f;
    [HideInInspector] public Animator animator;
    [SerializeField] private bool isRunning;
    public float moveX;
    public float moveZ;
    public bool canRun = false;


    private PlayerHealth playerHealth;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player = GameObject.FindGameObjectWithTag("Player");
        rbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);
        move();
        jump();
    }

    void move(){
        //move
        if(isGrounded){
            moveX = Input.GetAxis("Horizontal"); 
            moveZ = Input.GetAxis("Vertical");    

            //play animations of sprites
            if(moveX > 0){
                animator.SetBool("isWalking_left", true);
            }else{
                animator.SetBool("isWalking_left", false);
            }

            if(moveZ > 0){
                animator.SetBool("isWalking", true);
            }else{
                animator.SetBool("isWalking", false);
            }

            if(moveX < 0){
                 animator.SetBool("isWalking_right", true);
            }else{
                 animator.SetBool("isWalking_right", false);
            }

            if(moveZ < 0){
                animator.SetBool("isWalking_back", true);
            }else{
                animator.SetBool("isWalking_back", false);
            }

            bool isRunningAttempt = Input.GetKey(KeyCode.LeftShift) && canRun;
            isRunning = isRunningAttempt && playerHealth.playerStamina > 0;

            if(isRunning){
                playerHealth.isRunning = true;
                moveSpeed = runSpeed;
            }else if(!isRunning){
                playerHealth.isRunning = false;
                moveSpeed = walkSpeed;
            }

            Vector3 input = new Vector3(moveX, 0, moveZ).normalized;
            player.transform.position = transform.position + input * Time.deltaTime * moveSpeed;
            
        }
    }

    void jump(){
        //jumnp
        if(isGrounded && Input.GetKeyDown(KeyCode.Space)){
            animator.SetTrigger("jump");
            rbody.velocity = new Vector3(moveX * jumpSpeed, jumpHeight, moveZ * jumpSpeed);
        }
    }
    
    public void setAnimIdle(){
        animator.SetBool("isWalking_left", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isWalking_right", false);
        animator.SetBool("isWalking_back", false);
    }

}
