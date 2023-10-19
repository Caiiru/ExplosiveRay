using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Unit : MonoBehaviour
{
    [SerializeField] int currentHP,maxHP;
    private Rigidbody rb; 
    public virtual void Awake()
    {
        currentHP = maxHP;
    }

    public virtual bool TakeDamage(int damage, bool isGhost){
        //if returns true = this Unit is dead
        //if returns false = this unit is alive
        currentHP-=damage;
        if(currentHP<=0){
            this.transform.tag = "Untagged";
            transform.position = new Vector3(0,500);
            GetComponent<Rigidbody>().useGravity=false;
            
            if(!isGhost)
                GameManager.getInstance().UpdateEnemyList();
            return true; 
        }
        return false;
    }
    public virtual bool isAlive(){
        if(currentHP>0){
            return true;
        }
        return false;
    }
}
