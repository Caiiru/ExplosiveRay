using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeBomb : Bomb

{
    public int expansionNumber = 5;
    public float timeToSecondExplosion = 3;
    public bool littleActive;
    public float range;
    public float bombStregth;
    public int bombDamage = 1;
    int bombTextIndex;
    public GameObject vfxExplosion;
    public GameObject vfxLittles;
    public override void Init(Vector3 velocity, bool isSimulated, int InitialTimer)
    {
        vfx = vfxExplosion;
        base.Init(velocity, isSimulated, InitialTimer);
        if (isSimulated == false)
        {
            bombTextIndex = WorldText.instance.createWorldText(Vector3.zero, "2", textFontSize.Small);
        }
    }
    public override void Update()
    {
        base.Update();
        if (littleActive)
        {
            if (timeToSecondExplosion > 0)
            {
                timeToSecondExplosion -= 1 * Time.deltaTime;
            }
            else
            {
                subExplode();
            }
        }
    }
    private void LateUpdate()
    {
        if (_isActivated && !isGhost)
        {
            if (isOnContact)
                WorldText.getInstance().UpdateText(bombTextIndex, "!!", transform.position);
            else
                WorldText.getInstance().UpdateText(bombTextIndex, _timerUntilExplodes.ToString("F1"), transform.position);
        }
    }
    public override void Explode()
    {
        base.Explode();
        var VfxObj = Instantiate(vfxExplosion, transform.position, Quaternion.identity);
        for (int i = 0; i < expansionNumber; i++)
        {
            var little = Instantiate(this.gameObject, transform.position, Quaternion.identity);
            little.GetComponent<CompositeBomb>().littleActive = true;
            little.GetComponent<CompositeBomb>().range = 1f;
        }
        if (!isGhost)
            WorldText.getInstance().DeleteText(bombTextIndex);
        Destroy(gameObject);
    }
    public void subExplode()
    {
        littleActive = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider obj in colliders)
        {
            var VfxObj = Instantiate(vfxLittles, transform.position, Quaternion.identity);
            if (obj.GetComponent<SimplePirate>() != null)
            {

                obj.GetComponent<Rigidbody>().AddForce((
                    obj.transform.position - transform.position).normalized * bombStregth,
                    ForceMode.Impulse);
                obj.GetComponent<Unit>().TakeDamage(bombDamage, false);
            }
        }

        Destroy(gameObject);

    }
}
