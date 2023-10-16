using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSettings : MonoBehaviour
{
    #region Singleton

    public static SceneSettings instance;
    private void Awake()
    {
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Debug.Log("Settings Instance Already Exists!");
            Destroy(this);
        }
    }

    #endregion

    
}
