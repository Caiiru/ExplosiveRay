using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBodySimulation : MonoBehaviour
{
    [SerializeField] GravityBody[] bodies;
    static NBodySimulation instance;

    public bool isActive;


    private void Awake()
    {
        bodies = FindObjectsOfType<GravityBody>();
        Time.fixedDeltaTime = Universe.physicsTimeStep;
        Debug.Log("Settings fixedDeltaTime to: " + Universe.physicsTimeStep);
    }

    private void FixedUpdate()
    {
        if (isActive)
        {
            for (int i = 0; i < bodies.Length; i++)
            {
                Vector3 acceleration = CalculateAcceleration(bodies[i].Position, bodies[i]);
                bodies[i].UpdateVelocity(acceleration, Universe.physicsTimeStep);
            }
            for (int i = 0; i < bodies.Length; i++)
            {
                bodies[i].UpdatePosition(Universe.physicsTimeStep);
            }
        }
    }

    public static Vector3 CalculateAcceleration(Vector3 point, GravityBody ignoreBody = null)
    {
        Vector3 acceleration = Vector3.zero;
        if(ignoreBody.cantBeAtracted){
            return acceleration;
        }
        foreach (var body in Instance.bodies)
        {
            if (body != ignoreBody)
            {

                if (point.sqrMagnitude < ((body.Position - point).normalized * body.gravityRadius).sqrMagnitude)
                {
                    Debug.Log("is inside " + ignoreBody.bodyName);
                    float sqrDst = (body.Position - point).sqrMagnitude;
                    Vector3 forceDir = (body.Position - point).normalized;
                    acceleration += forceDir * Universe.gravitationalConstant * body.mass / sqrDst;
                }
                else
                {
                    acceleration = Vector3.zero;
                    Debug.Log("is ouside " + ignoreBody.bodyName);
                }

                Debug.DrawRay(body.Position, body.velocity, Color.green);
            }

        }
        return acceleration;
    }

    static NBodySimulation Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NBodySimulation>();
            }
            return instance;
        }
    }
}