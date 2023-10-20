using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class gravityBomb : Bomb
{
    public float radius;
    public float gravityForce;
    public bool cantBeAtracted = false;
    public Vector3 velocity;
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    public override void Explode()
    {
        base.Explode();
    }
    public override void Init(Vector3 velocity, bool isSimulated)
    {   
        //NBombSimulation.Instance.bombInited(this);
        _rb.isKinematic = true; 
        base.Init(velocity, isSimulated);
        velocity = _rb.velocity;
    }

    public void UpdateVelocity (Vector3 acceleration, float timeStep){
        velocity += acceleration * timeStep;
    }

    public void UpdatePosition(float timeStep){
        _rb.MovePosition(_rb.position + velocity * timeStep);
    }

    public Vector3 Position{
        get{return _rb.position;}
    }
}
