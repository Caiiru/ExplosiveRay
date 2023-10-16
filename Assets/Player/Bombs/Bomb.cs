using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bomb : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] public float _timerUntilExplodes = 4f;
    public bool _isActivated;
    private GameObject bombHolder;
    public virtual void Init(Vector3 velocity, bool isSimulated)
    {
        bombHolder = transform.GetChild(0).gameObject;
        _rb =GetComponent<Rigidbody>();
        _rb.AddForce(velocity, ForceMode.Impulse); 
        _isActivated=true;
    }
    public virtual void Explode()
    {
        _isActivated=false;
        print("Explosion");
        transform.position = new Vector3(0,-50,0);
        bombHolder.GetComponent<MeshRenderer>().enabled=false;
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
}
