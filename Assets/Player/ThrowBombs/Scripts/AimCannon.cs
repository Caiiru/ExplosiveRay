using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;

public enum cannonStatus
{
    Idle,
    Aiming,
    Firing
}
public class AimCannon : MonoBehaviour
{
    public cannonStatus currentStatus = cannonStatus.Idle;
    public GameObject currentBomb;
    private Bomb bomb;
    #region Cannon Settings
    Vector3 throwVector;
    Vector3 mousePos;

    //Layer to raycast interact (simulating 2D)
    public LayerMask projectionLayer;

    //Bomb Spawn Position
    public Transform cannonPoint;
    //The pivot for the barrel
    public Transform barrelPivot;

    [Range(10, 50)] public int rotationSpeed = 10;
    public float force;

    #endregion

    [SerializeField] private Projection _projection;
    public Transform targetTransform;
    void Start()
    {
        bomb = currentBomb.GetComponent<Bomb>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckStatus();
        CalculateThrowVector();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowBomb();
        }

        //barrelPivot.Rotate(Vector3.left * rotationSpeed*Time.deltaTime);
        barrelPivot.LookAt(mousePos);

    }
    void CalculateThrowVector()
    {
        if (currentStatus == cannonStatus.Aiming)
        {
            float newForce = force; 
            if (force >= 10 && force <= 30)
            {
                newForce += Input.mouseScrollDelta.y*1.3f; 
                force = newForce<10?10:newForce>30?30:newForce;
            } 
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitinfo, float.MaxValue, projectionLayer))
            {
                mousePos = new Vector3(hitinfo.point.x, hitinfo.point.y, 0);
                Debug.DrawRay(cannonPoint.transform.position, mousePos, Color.red, 0.1f, false);
                
            }
            _projection.SimulateTrajectory(bomb, cannonPoint.transform.position, cannonPoint.forward * force);
        }
    }
    void ThrowBomb()
    {
        var throwedBomb = Instantiate(currentBomb, cannonPoint.position, quaternion.identity);
        throwedBomb.GetComponent<Bomb>().Init(cannonPoint.forward * force, false);
    }
    void CheckStatus()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("mouse click");
            switch (currentStatus)
            {
                case cannonStatus.Idle:
                    changeStatus(cannonStatus.Aiming); 
                    break;
                case cannonStatus.Aiming:
                    changeStatus(cannonStatus.Firing);
                    break;
                case cannonStatus.Firing:
                    changeStatus(cannonStatus.Idle);
                    break;
            }
        }
    }

    void changeStatus(cannonStatus status)
    {
        if (currentStatus != status)
        {
            currentStatus = status;
            switch (status)
            {
                case cannonStatus.Idle:
                    Cursor.visible = true;
                    break;
                case cannonStatus.Aiming:
                    Cursor.visible = false;
                    break;

                case cannonStatus.Firing:
                    break;

                default:
                    print("Error condition");
                    break;
            }
        }
    }


}
