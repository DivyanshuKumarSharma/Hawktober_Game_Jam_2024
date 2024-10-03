using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public Camera camera;
    public int moveDistance = 10;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    void move(){
        Vector3 position = player.transform.position;
        if(Input.GetKeyDown(KeyCode.D)){
             position.x += moveDistance;
             player.transform.position = position;
        }
            if(Input.GetKeyDown(KeyCode.A)){
             position.x -= moveDistance;
             player.transform.position = position;
        }
            if(Input.GetKeyDown(KeyCode.W)){
             position.z += moveDistance;
             player.transform.position = position;
        }
            if(Input.GetKeyDown(KeyCode.S)){
             position.z -= moveDistance;
             player.transform.position = position;
        }
    }
}
