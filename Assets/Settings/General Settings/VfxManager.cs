using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VFX_TYPE
{
    cannon_shoot,
    bombSimple_explosion,
    compositeBomb_explosion,
    littleBombs_explosion,
    blackholeBomb_explosion
}
public class VfxManager : MonoBehaviour
{
    public static VfxManager instance;
    public GameObject[] vfxList;
    public List<GameObject> activiesVfx = new List<GameObject>();


    public VFX_TYPE vfx_type;

    private void Awake() {
        instance=this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    } 
    public void DestroyAnotherGameObject(GameObject obj, float timer){
        timer-=1*Time.fixedDeltaTime;
        if(timer  < 0){
            Destroy(obj);
        }
    }
 

    public static VfxManager Instance{
        get{ if (instance = null){
            instance = FindObjectOfType<VfxManager>();
        }
        return instance;}
    }
}
