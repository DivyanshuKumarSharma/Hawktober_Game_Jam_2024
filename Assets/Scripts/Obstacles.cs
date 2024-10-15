using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public float damage = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player"){
            GameObject player = other.gameObject;
            PlayerHealth PlayerHealth =  player.GetComponent<PlayerHealth>();
            PlayerHealth.TakeDamage(damage);
            Debug.Log("Damage-Taken");
        }
    }
}
