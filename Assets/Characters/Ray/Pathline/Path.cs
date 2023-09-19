using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    static GameObject clone = null;

    public static void StartVisualizingPath(GameObject throwable)
    {
        throwable.GetComponent<Rigidbody2D>().simulated = false;
        throwable.GetComponent<CircleCollider2D>().enabled = false;

        clone = Instantiate(throwable, throwable.transform.position, Quaternion.identity);
        clone.GetComponent<Rigidbody2D>().simulated = true;
        clone.GetComponent<CircleCollider2D>().enabled = true;

        Physics2D.simulationMode = SimulationMode2D.Script;
    }
    public static void VisualizePath(GameObject throwable, Vector3 force, int steps, Material lineMaterial)
    {
        clone.transform.position = throwable.transform.position;
        Rigidbody2D cloneRigidbody = clone.GetComponent<Rigidbody2D>();
        cloneRigidbody.velocity = Vector3.zero;
        cloneRigidbody.AddForce(force);

        List<Vector3> pathPoints = new List<Vector3>();
        int simulationSteps = steps; 
        for (int i = 1; i < simulationSteps; i++)
        {
            Physics2D.Simulate(Time.fixedDeltaTime);
            pathPoints.Add(cloneRigidbody.transform.position);

        }
        LineRenderer line = cloneRigidbody.GetComponent<LineRenderer>();
        line.enabled = true;
        line.positionCount = pathPoints.Count;
        line.startColor = Color.white;
        line.endColor = Color.red;
        line.widthMultiplier = 0.15f;
        line.material = lineMaterial;

        line.SetPositions(pathPoints.ToArray());

    }
    public static void StopVisualizingPath(GameObject throwable)
    {
        throwable.GetComponent<CircleCollider2D>().enabled = true;
        throwable.GetComponent<Rigidbody2D>().simulated = true;
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        Destroy(clone);
    }

}
