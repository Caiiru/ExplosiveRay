using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Im: " + gameObject.name + " and my scene is: "+ gameObject.scene.name );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
