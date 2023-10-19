using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackholeBomb : Bomb
{
    public int range = 5;
    public float duration = 10;
    [Range(0.1f,1f)] public float bombStregth;
    public bool isActive = false;
    public Collider[] colliders;
    [SerializeField] LayerMask bombLayer;
    public override void Explode()
    {
        isActive = true;
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.zero;
        _rb.useGravity = false;

        GetComponent<SphereCollider>().enabled=false;

        var simulationScene = SceneManager.GetSceneByName("Simulation");
        var ghostBomb = Instantiate(this.gameObject);

        foreach (Transform childs in ghostBomb.transform)
        {
            childs.GetComponent<MeshRenderer>().enabled = false;
        }
        var ghostBlackHole = ghostBomb.GetComponent<BlackholeBomb>();
        ghostBlackHole.duration = duration;
        ghostBlackHole.isActive=true;
        ghostBlackHole._isActivated=false;
        SceneManager.MoveGameObjectToScene(ghostBomb, simulationScene);
        base.Explode();
    }
    public override void Init(Vector3 velocity, bool isSimulated)
    {
        base.Init(velocity, isSimulated);
        bombLayer = LayerMask.NameToLayer("BombLayer"); 
    }
    public override void Update()
    {
        if (isActive)
        {
            if (duration >= 0)
            {
                colliders = Physics.OverlapSphere(transform.position, range);
                foreach (Collider obj in colliders)
                {
                    Vector3 forceToAdd = (transform.position - obj.transform.position) * bombStregth;
                    /*
                    if (obj.CompareTag("Enemy") || obj.CompareTag("Bomb") && obj != this)
                    {
                        obj.transform.position += (this.transform.position - obj.transform.position) * bombStregth;
                    }
                    */
                    if(obj.CompareTag("Bomb")){
                        obj.GetComponent<Bomb>().AddForce(forceToAdd, this.gameObject.name);
                    }

                }
                duration -= 1 * Time.deltaTime;
            }
            else
            {
                isActive = false;
                Destroy(gameObject);
            }

        }

        base.Update();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
