using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentBomb : Bomb
{
    
    void Start()
    {
        bombType = BombType.Timer;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void Activate(float timer){
        base.Activate(timer);
    } 

    public override void OnCollisionEnter2D(Collision2D other){
        base.OnCollisionEnter2D(other);
    }

}
