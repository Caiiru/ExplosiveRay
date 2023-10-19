using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Bomb : MonoBehaviour
{
    public Rigidbody _rb;
    [SerializeField] public float _timerUntilExplodes = 4f;
    public bool _isActivated;
    private GameObject bombHolder;
    public bool isGhost = false;

    public Sprite bombSprite;
    public virtual void Init(Vector3 velocity, bool isSimulated)
    {
        this.gameObject.layer = LayerMask.NameToLayer("BombLayer"); 
        bombHolder = transform.GetChild(0).gameObject;
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(velocity, ForceMode.Impulse); 
        _isActivated=true;
    }
    public virtual void Explode()
    {
        _isActivated=false;
        print("Explosion");
    }
    public virtual void Update()
    {
        if (transform.position.y < -10)
        {
            _rb.isKinematic = true;

        }
        if(_isActivated){
            _timerUntilExplodes -=1f*Time.deltaTime;
            if(_timerUntilExplodes <=0 ){
                Explode();
            }
        }
       
    }
    public Rigidbody getRigidbody(){
        return _rb;
    }

    public void AddForce(Vector3 force, string name){
        _rb.AddForce(force, ForceMode.Acceleration);
        Debug.Log("Adding force: " +force + "in " + gameObject.name + " from: " + name);
    }
}
