using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptBalls : MonoBehaviour
{
    Vector3 startPos;
    void Start()
    {
        startPos=transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            transform.position = startPos;
        }
    }
}
