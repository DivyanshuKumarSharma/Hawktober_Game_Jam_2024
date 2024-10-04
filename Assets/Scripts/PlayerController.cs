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
    public Camera camera;
    public int moveSpeed = 10;
    [SerializeField]
    private Rigidbody rbody;
    [SerializeField]
    private BoxCollider collider;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.15f;
    [SerializeField] private float jumpHeight = 10f;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
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
            float moveX = Input.GetAxis("Horizontal"); 
            float moveZ = Input.GetAxis("Vertical");    
            Debug.Log(moveX + " " + moveZ);

            //play animations of sprites

            Vector3 input = new Vector3(moveX, 0, moveZ).normalized;
            player.transform.position = transform.position + input * Time.deltaTime * moveSpeed;
            
        }
    }

    void jump(){
        //jumnp
        if(isGrounded && Input.GetKeyDown(KeyCode.Space)){
            rbody.velocity = new Vector3(0, jumpHeight, 0);
        }
    }

    

    


}
