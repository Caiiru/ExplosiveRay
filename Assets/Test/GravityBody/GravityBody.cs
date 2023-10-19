using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour
{
    public float radius;
    public float surfaceGravity;
    public float gravityRadius;
    public Vector3 initialVelocity;
    public bool cantBeAtracted;
    Transform meshHolder;
    public Vector3 velocity {get;private set;}
    public float mass {get; private set;}
    Rigidbody rb;
    public string bodyName;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic=true;
        rb.mass = mass;
        velocity = initialVelocity;
    }

    public void UpdateVelocity(GravityBody[] allBodies, float timeStep){
        foreach(var otherBody in allBodies){
            if(otherBody != this){
                float sqrDst = (otherBody.rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.rb.position - rb.position).normalized;

                Vector3 acceleration = forceDir * Universe.gravitationalConstant * otherBody.mass/sqrDst;
                velocity += acceleration * timeStep;
            }
        }
    }

    public void UpdateVelocity (Vector3 acceleration, float timeStep){
        velocity += acceleration*timeStep;
    }
    public void UpdatePosition (float timeStep){
        rb.MovePosition(rb.position+velocity * timeStep);
    }
    private void OnValidate() {
        mass = surfaceGravity* radius * radius / Universe.gravitationalConstant;
        meshHolder = transform.GetChild(0);
        meshHolder.localScale = Vector3.one*radius;
        gameObject.name = bodyName;
    }

    public Rigidbody Rigidbody{
        get{
            return rb;

        }
    }
    public Vector3 Position{
        get{
            return rb.position;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, gravityRadius);
    }
}
