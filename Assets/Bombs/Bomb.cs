using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Vector3 throwVector;
    Rigidbody2D rb;
    LineRenderer line;
    CircleCollider2D circleCollider;

    public float throwForce = 100f;
    public int steps = 100;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void OnMouseDown(){
        CalculateThrowVector();
        Path.StartVisualizingPath(this.gameObject);
    }
    private void OnMouseDrag() {
        CalculateThrowVector();
        Path.VisualizePath(this.gameObject, throwVector,steps);
    }
    private void OnMouseUp() {
        Path.StopVisualizingPath(this.gameObject);
        Throw();
    }
    void CalculateThrowVector(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 distance = mousePos-transform.position;
        throwVector = -distance.normalized*throwForce;
    }
    void Throw(){
        rb.AddForce(throwVector);
    }
 
}
