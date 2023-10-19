using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class littleEntity : MonoBehaviour
{
    private Rigidbody rb;    
    [Range(1,20)][SerializeField] private float accelerationForce = 10;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W)){
            rb.AddForce(Vector3.up*accelerationForce);
        }if(Input.GetKey(KeyCode.D)){
            rb.AddForce(Vector3.right*accelerationForce);
        }if(Input.GetKey(KeyCode.A)){
            rb.AddForce(Vector3.left*accelerationForce);
        }if(Input.GetKey(KeyCode.S)){
            rb.AddForce(Vector3.down*accelerationForce);
        }
        transform.position = new Vector3(transform.position.x,transform.position.y,0);
    }

    public void AddForce(Vector3 force){
        rb.AddForce(force, ForceMode.Acceleration);
    }
}
