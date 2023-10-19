using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePirate : Unit
{
    public override void Awake()
    {
        base.Awake();
    }
    public override bool TakeDamage(int damage, bool isGhost)
    {
        return base.TakeDamage(damage,isGhost);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.transform.name == "Earth"){
            TakeDamage(99,false);
        }
    }
}
