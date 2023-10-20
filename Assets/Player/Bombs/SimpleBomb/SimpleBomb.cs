using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleBomb : Bomb
{
    [SerializeField][Range(1, 3)] int bombDamage = 2;
    [SerializeField][Range(3, 100)] int bombStregth = 6;
    [SerializeField][Range(1, 6)] int bombRange = 3;

    public Collider[] colliders;

    int bombTextIndex;

    private void Start()
    {
    }
    public override void Init(Vector3 velocity, bool isSimulated, int InitialTimer)
    {
        base.Init(velocity, isSimulated, InitialTimer);
        if (isSimulated == false)
        {
            if (isOnContact)
                bombTextIndex = WorldText.instance.createWorldText(Vector3.zero, "!!", textFontSize.Small);
            else
                bombTextIndex = WorldText.instance.createWorldText(Vector3.zero, "2", textFontSize.Small);
        }
        colliders = new Collider[10];
    }
    public override void Update()
    {
        base.Update();
    }
    private void LateUpdate()
    {
        if (_isActivated && !isGhost)
        {
            WorldText.getInstance().UpdateTextsPositions(bombTextIndex, new Vector3(transform.position.x, transform.position.y + 1f));
            
            if (isOnContact) 
                WorldText.getInstance().UpdateText(bombTextIndex, "!!");
            else
                WorldText.getInstance().UpdateText(bombTextIndex, _timerUntilExplodes.ToString("F1"));
            
        }
    }

    public override void Explode()
    {   
        var VfxObj = Instantiate(vfx,transform.position,Quaternion.identity);
        base.Explode();
        colliders = Physics.OverlapSphere(transform.position, bombRange);

        foreach (Collider obj in colliders)
        {
            if (obj.GetComponent<SimplePirate>() != null)
            {

                obj.GetComponent<Rigidbody>().AddForce((
                    obj.transform.position - transform.position).normalized * bombStregth,
                    ForceMode.Impulse);
                obj.GetComponent<Unit>().TakeDamage(bombDamage, false);
            }
        }
        if (!isGhost)
            WorldText.getInstance().DeleteText(bombTextIndex);

        transform.position = new Vector3(0, -50, 0);
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bombRange);
    }
}
