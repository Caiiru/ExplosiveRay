using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackholeBomb : Bomb
{
    public int radius = 5;
    public float duration = 10;
    [Range(0.1f, 5f)] public float bombStregth;
    public bool isActive = false;
    public Collider[] colliders;
    [SerializeField] LayerMask bombLayer;
    public Scene currentScene;
    public int textIndex;

    public override void Explode()
    {
        isActive = true;
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.zero;
        _rb.useGravity = false;

        GetComponent<SphereCollider>().enabled = false;

        //NBombSimulation.Instance.CreateBlackholeBomb();

        var simulationScene = SceneManager.GetSceneByName("Simulation");
        var ghostBomb = Instantiate(this.gameObject);

        foreach (Transform childs in ghostBomb.transform)
        {
            childs.GetComponent<MeshRenderer>().enabled = false;
        }
        var ghostBlackHole = ghostBomb.GetComponent<BlackholeBomb>();
        ghostBlackHole.duration = duration;
        ghostBlackHole.tag = "Bomb";
        ghostBlackHole.isGhost = true;
        ghostBlackHole.isActive = true;
        ghostBlackHole._isActivated = false;
        SceneManager.MoveGameObjectToScene(ghostBomb, simulationScene);
        base.Explode();
    }
    public override void Init(Vector3 velocity, bool isSimulated, int InitialTimer)
    {
        base.Init(velocity, isSimulated, InitialTimer);
        bombLayer = LayerMask.NameToLayer("BombLayer");
        if (isSimulated == false)
            textIndex = WorldText.getInstance().createWorldText(transform.position, _timerUntilExplodes.ToString("F1"), textFontSize.Small);

    }
    public override void Update()
    {
        if (_isActivated)
        {
            if (isOnContact)
                WorldText.getInstance().UpdateText(textIndex, "!!",transform.position);
            else
                WorldText.getInstance().UpdateText(textIndex, _timerUntilExplodes.ToString("F1"),transform.position);
        }
        if (isActive)
        {
            if (duration >= 0)
            {
                WorldText.getInstance().UpdateText(textIndex, duration.ToString("F1"), transform.position);
                currentScene = gameObject.scene;
                colliders = Physics.OverlapSphere(transform.position, radius, bombLayer);
                foreach (Collider obj in colliders)
                {
                    Vector3 forceToAdd = (transform.position - obj.transform.position) * bombStregth;

                    if (obj.CompareTag("Enemy") || obj.CompareTag("Bomb") && obj != this)
                    {
                        obj.transform.position += (this.transform.position - obj.transform.position) * bombStregth / 2;
                    }
                    Debug.Log(obj.gameObject.name);
                    if (obj.CompareTag("Bomb"))
                    {
                        //obj.GetComponent<Bomb>().AddForce(forceToAdd, this.gameObject.name);
                        obj.GetComponent<Rigidbody>().AddForce(forceToAdd, ForceMode.Acceleration);
                    }

                }
                duration -= 1 * Time.deltaTime;
            }
            else
            {
                if (!isGhost)
                    WorldText.getInstance().DeleteText(textIndex);
                isActive = false;
                Destroy(gameObject);
            }


        }
        base.Update();
    }
    public Transform objective;
    public Vector3 CalculateForceAboveOther(Vector3 currentOtherPosition, float step)
    {
        Vector3 acceleration = Vector3.zero;
        if (this.isActive)
        {/* 
            Debug.Log("Other Position: " + currentOtherPosition + " sqr: " + currentOtherPosition.sqrMagnitude);
            Debug.Log("Objective " + ((transform.position - currentOtherPosition).normalized * radius).sqrMagnitude); */
            Vector3 direction = currentOtherPosition - transform.position;
            if (direction.magnitude > radius)
            {
                direction = (direction.normalized * radius);


                if (direction.magnitude < radius)
                {
                    Debug.DrawLine(transform.position, transform.position + direction);
                    acceleration += ((transform.position - currentOtherPosition).normalized * bombStregth) * Time.deltaTime;

                }
            }


        }
        return acceleration;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);

    }
}
