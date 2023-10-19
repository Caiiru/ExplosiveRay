using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    public float gravityForce;
    [Range(1f,20f)]public float gravityRange;
    private Rigidbody rb;
    public Collider[] colliders;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity=false;
        rb.mass = 100000000f;
    }

    // Update is called once per frame
    void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, gravityRange);
        foreach(Collider obj in colliders){
            if(obj.GetComponent<littleEntity>() != null){
                //obj.gameObject.transform.position+=(transform.position - obj.transform.position) * gravityForce;
                obj.GetComponent<littleEntity>().AddForce((transform.position - obj.transform.position) * gravityForce);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, gravityRange);
    }
}
