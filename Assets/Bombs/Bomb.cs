using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BombType
{
    Impact,
    Timer
}
public abstract class Bomb : MonoBehaviour
{
    public BombType bombType;
    #region Impact Option Settings

    #endregion
    #region Timer Option Settigns
    [SerializeField] float currentTimer;
    private float activateTimer;

    #endregion
    public bool wasActivated = false;
    public virtual void Update()
    {
        if (wasActivated && bombType == BombType.Timer)
        {
            if (currentTimer >= activateTimer)
            {
                Debug.Log("Explosion Timer");
                Destroy(this.gameObject);
            }
            currentTimer += 1 * Time.deltaTime;

        }



    }

    public virtual void Activate(float timer)
    {
        activateTimer = timer;
        currentTimer = 0;
        wasActivated = true;
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (bombType == BombType.Impact && wasActivated)
        {
            Debug.Log("Explosion Impact");
            Destroy(this.gameObject);
        }
    }
}
