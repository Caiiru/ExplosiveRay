using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Unit : MonoBehaviour
{
    [SerializeField] int currentHP,maxHP;
    private Rigidbody rb; 
    public virtual void Start()
    {
        currentHP = maxHP;
    }

    public virtual bool TakeDamage(int damage){
        //if returns true = this Unit is dead
        //if returns false = this unit is alive
        currentHP-=damage;
        if(currentHP<0){
        
            return true; 
        }
        return false;
    }
}
