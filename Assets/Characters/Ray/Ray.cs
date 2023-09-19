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

    public float activationTimer = 2f; //Time To activate the bomb

    #region Pathline
 
    public Material pathLineMaterial;


    [Range(10, 150)]
    public int steps;

    #endregion


    #endregion


    private void Start()
    {   
        _collider = GetComponent<CircleCollider2D>(); 

    }
    public Vector3 CalculateThrowVector()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 distance = mousePos - transform.position;
        Debug.DrawLine(transform.position,-distance.normalized * force,Color.red,0.1f);
        return throwVector = -distance.normalized * force;
         
    }
    void ThrowBomb()
    {
         var throwedBomb = Instantiate(currentBomb.gameObject,transform.position,Quaternion.identity);
         throwedBomb.GetComponent<Rigidbody2D>().AddForce(throwVector);
         throwedBomb.GetComponent<Bomb>().Activate(activationTimer);
    }

    private void OnMouseDown()
    { 
        currentBomb.transform.position = transform.position;
        CalculateThrowVector();
        Path.StartVisualizingPath(currentBomb);
    }
    public void OnMouseDrag()
    { 
        Path.VisualizePath(currentBomb,CalculateThrowVector(), steps, pathLineMaterial);
        
    }
    private void OnMouseUp()
    {
        Path.StopVisualizingPath(currentBomb);
        ThrowBomb();
    }

    public void setForce(float force){
        this.force = force;
    }

}
