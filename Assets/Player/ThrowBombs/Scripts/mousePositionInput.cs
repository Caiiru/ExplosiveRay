using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mousePositionInput : MonoBehaviour
{
    public Vector3 mouse;
    public Camera _camera;
    public bool checkRay;
    public Transform Cannon;
    public LayerMask projectionLayer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (checkRay)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycast,float.MaxValue, projectionLayer))
            {
                mouse =  new Vector3(raycast.point.x, raycast.point.y,0);
                Debug.DrawRay(Cannon.transform.position, mouse, Color.white,0.1f, false);
            }

        }
    }
}
