using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public GameObject currentBomb; //bomb selected

    #region Components
    private CircleCollider2D _collider;
    #endregion

    #region Throw settings
    Vector3 throwVector;
    [Range(400,1000)]
    public float force;

    #region Pathline

    LineRenderer line;
    public Material pathLineMaterial;


    [Range(10, 150)]
    public int steps;

    #endregion


    #endregion


    private void Start()
    {
        if (!gameObject.GetComponent<LineRenderer>()) line = gameObject.AddComponent<LineRenderer>();
        _collider = GetComponent<CircleCollider2D>();
        line.material = pathLineMaterial;

    }
    Vector3 CalculateThrowVector()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 distance = mousePos - transform.position;
        return throwVector = -distance.normalized * force;
         
    }
    void ThrowBomb()
    {
         var throwedBomb = Instantiate(currentBomb.gameObject,transform.position,Quaternion.identity);
         throwedBomb.GetComponent<Rigidbody2D>().AddForce(throwVector);
    }

    private void OnMouseDown()
    { 
        currentBomb.transform.position = transform.position;
        CalculateThrowVector();
        Path.StartVisualizingPath(currentBomb);
    }
    private void OnMouseDrag()
    {
        
        CalculateThrowVector();
        Path.VisualizePath(currentBomb,CalculateThrowVector(), steps, pathLineMaterial);
        
    }
    private void OnMouseUp()
    {
        Path.StopVisualizingPath(currentBomb);
        ThrowBomb();
    }



}
