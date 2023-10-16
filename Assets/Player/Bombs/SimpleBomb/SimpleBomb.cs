using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleBomb : Bomb
{
    [SerializeField][Range(1, 6)] int bombStregth = 3;
    [SerializeField][Range(1, 6)] int bombRange = 3;

    public Collider[] colliders;

    int bombTextIndex;

    private void Start()
    {
    }
    public override void Init(Vector3 velocity, bool isSimulated)
    {
        base.Init(velocity, isSimulated);
        if (isSimulated == false)
        {
            bombTextIndex = WorldText.instance.createWorldText(Vector3.zero, "2", textFontSize.Small);
        }
        colliders = new Collider[5];
    }
    public override void Update()
    {
        base.Update();
    }
    private void LateUpdate()
    {
        if (_isActivated)
        {
            WorldText.getInstance().UpdateTextsPositions(bombTextIndex, new Vector3(transform.position.x, transform.position.y + 1f));
            WorldText.getInstance().UpdateText(bombTextIndex, _timerUntilExplodes.ToString("F1"));
        }
    }

    public override void Explode()
    {

       
        colliders = Physics.OverlapSphere(transform.position, bombRange);
        
        foreach (Collider obj in colliders)
        {
            if (obj.GetComponent<SimplePirate>() != null)
            {

                obj.GetComponent<Rigidbody>().AddForce((
                    obj.transform.position - transform.position).normalized * bombStregth,
                    ForceMode.Impulse);
                obj.GetComponent<Unit>().TakeDamage(bombStregth);
            }
        }
        WorldText.getInstance().DeleteText(bombTextIndex);
         base.Explode();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,bombRange);
    }
}
