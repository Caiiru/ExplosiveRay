using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public enum cannonStatus
{
    Idle,
    Aiming,
    Firing
}


public class AimCannon : MonoBehaviour
{
    public cannonStatus currentStatus = cannonStatus.Idle;
    #region Cannon Settings
    Vector3 mousePos;
    public Slider slider;

    //Layer to raycast interact (simulating 2D)
    public LayerMask projectionLayer;

    //Bomb Spawn Position
    public Transform cannonPoint;
    //The pivot for the barrel
    public Transform barrelPivot;

    public int[] possibleTimers;
    public int currentTimerIndex = 0;

    [Range(10, 50)] public int rotationSpeed = 10;
    public float force;
    private float minForce = 500;
    private float maxForce = 2500;

    #endregion
    private Projection _projection;
    public GameObject explosionVFX;
    void Start()
    {
        slider.minValue = minForce;
        slider.maxValue = maxForce;
        possibleTimers = new int[5];
        _projection = GetComponent<Projection>();
        for (int i = 0; i < 5; i++)
        {
            possibleTimers[i] = i;
        }
        UiManager.getInstance().changeCurrentTimer(possibleTimers[currentTimerIndex]);
        explosionVFX.SetActive(false);
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
            slider.value = force;
            if (force >= minForce && force <= maxForce)
            {
                newForce += Input.mouseScrollDelta.y * 100;
                force = newForce < minForce ? minForce : newForce > maxForce ? maxForce : newForce;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitinfo, float.MaxValue, projectionLayer))
            {
                mousePos = new Vector3(hitinfo.point.x, hitinfo.point.y, 0);
                explosionVFX.transform.position = cannonPoint.transform.position;

            }
            _projection.SimulateTrajectory(PlayerInventory.getInstance().GetCurrentBomb(), cannonPoint.transform.position, cannonPoint.forward * force, 2);
        }
    }
    void ThrowBomb()
    {
        var throwedBomb = Instantiate(PlayerInventory.getInstance().GetCurrentBomb(), cannonPoint.position, quaternion.identity);
        throwedBomb.GetComponent<Bomb>().Init(cannonPoint.forward * force, false, possibleTimers[currentTimerIndex]);
        
        explosionVFX.SetActive(true);

        for (int i = 0; i < explosionVFX.transform.childCount; i++)
        {
            explosionVFX.transform.GetChild(i).GetComponent<ParticleSystem>().Play();
        }
        //explosionVFX.SetActive(false);

    }
    void CheckStatus()
    {
        if (Input.GetMouseButtonDown(0))
        {
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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentTimerIndex = currentTimerIndex + 1 > possibleTimers[possibleTimers[possibleTimers.Length - 1]] ? possibleTimers[0] : currentTimerIndex + 1;
            UiManager.getInstance().changeCurrentTimer(possibleTimers[currentTimerIndex]);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentTimerIndex = currentTimerIndex - 1 < possibleTimers[0] ? possibleTimers[possibleTimers[possibleTimers.Length - 1]] : currentTimerIndex - 1;
            UiManager.getInstance().changeCurrentTimer(possibleTimers[currentTimerIndex]);
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

    public void changeForce(float value)
    {
        value = value <= minForce ? minForce : value;
        value = value >= maxForce ? maxForce : value;

        force = value;
    }


}
