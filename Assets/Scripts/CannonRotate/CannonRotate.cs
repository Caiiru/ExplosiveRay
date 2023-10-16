using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerCondition
{
    Idle,
    Aiming,
    Firing
}
public class CannonRotate : MonoBehaviour
{
    public PlayerCondition playerCondition;
    public Vector3 targetPosition;
    [SerializeField] Transform cannonSpawn;
    [SerializeField] private LayerMask projectionLayer;
    public Transform barrel;

    #region Rotate Settings
    [Range(0,90)] public float rotationSpeed = 30f;
    [Range(0,360)] public float Rotate = 30f;
    

    #endregion

    void Start()
    {
    }

    public Vector3 RotateStatus;
    void Update()
    {

         
        {
            switch (playerCondition)
            {
                case PlayerCondition.Idle:
                    playerCondition = PlayerCondition.Aiming;
                    break;
                case PlayerCondition.Aiming:
                    print("fire");
                    playerCondition = PlayerCondition.Idle;
                    break;
                default:
                    print("error condition");
                    break;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            switch (playerCondition)
            {
                case PlayerCondition.Idle: 
                    break;
                case PlayerCondition.Aiming:
                    playerCondition = PlayerCondition.Idle;
                    break;
                default:
                    print("error condition");
                    break;
            }
        }
        if (playerCondition == PlayerCondition.Aiming)
        {
            Cursor.visible=false;
            #region Raycast
            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray2, out RaycastHit info, float.MaxValue, projectionLayer))
            {
                targetPosition = new Vector3(info.point.x, info.point.y, cannonSpawn.transform.position.z);
                Debug.DrawLine(cannonSpawn.transform.position,
                targetPosition,
                Color.green,
                0.2f,
                false);
            }
            #endregion

            #region Rotation
            float currentAngle = Mathf.Atan2(targetPosition.y,targetPosition.x) * Mathf.Rad2Deg;
            barrel.Rotate(Vector3.right *rotationSpeed * Time.fixedDeltaTime,Space.Self);

                

            #endregion
        }
        if(playerCondition == PlayerCondition.Idle){
            Cursor.visible=true;
        }
    }


}
