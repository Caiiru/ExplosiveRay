using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public static WaveController instance;

    [Range(0.1f,3f)]
    public float amplitude = 1f;

    [Range(0.1f,3f)]
    public float length = 2f;
    
    [Range(0.1f,3f)]
    public float speed = 1f;
    
    private float offset = 0f;


    private void Awake() {
        if(instance == null){
            instance = this;
        }
        else if(instance != this){ 
            Destroy(this);
        }
    }
    private void Update() {
        offset += Time.deltaTime*speed;
    }
    public float GetWaveHeight(float _x){
        return amplitude * Mathf.Sin(_x / length + offset);
    }
}
