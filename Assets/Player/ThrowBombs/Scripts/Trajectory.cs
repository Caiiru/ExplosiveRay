using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Trajectory : MonoBehaviour
{
    public int numSteps = 1000;
    public float timeStep = 0.1f;
    public bool usePhysicsTimeStep;
    public float width = 100;
    public bool useThickLines;
    public Color pathColor;
    public Transform startPos;
    void Start()
    {

    }


    void DrawTrajectory()
    {
        gravityBomb[] bodies = FindObjectsOfType<gravityBomb>();
        var virtualBodies = new VirtualBody[bodies.Length];
        var drawPoints = new Vector3[bodies.Length][];

        for (int i = 0; i < virtualBodies.Length; i++)
        {
            virtualBodies[i] = new VirtualBody(bodies[i]);
            drawPoints[i] = new Vector3[numSteps];

        } //Simulate

        for (int step = 0; step < numSteps; step++)
        {

            for (int i = 0; i < virtualBodies.Length; i++)
            {
                virtualBodies[i].velocity += CalculateAcceleration(i, virtualBodies) * timeStep;
            }

            //update position
            for (int i = 0; i < virtualBodies.Length; i++)
            {
                Vector3 newPos = virtualBodies[i].position + virtualBodies[i].velocity * timeStep;
                virtualBodies[i].position = newPos;
                drawPoints[i][step] = newPos;
            }
        }


        //draw paths 


        for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++)
        {
            if (useThickLines)
            {
                //var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
                var lineRenderer = startPos.GetComponent<LineRenderer>();
                lineRenderer.enabled = true;
                lineRenderer.positionCount = drawPoints[bodyIndex].Length;
                lineRenderer.SetPositions(drawPoints[bodyIndex]);
                lineRenderer.startColor = pathColor;
                lineRenderer.endColor = pathColor;
                lineRenderer.widthMultiplier = width;
            }
            else
            {
                for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++)
                {
                    if (i == 0)
                    {
                        Debug.DrawLine(startPos.position, drawPoints[bodyIndex][i + 1], pathColor);
                    }
                    else
                        Debug.DrawLine(drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1], pathColor);
                }

                var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
                if (lineRenderer)
                {
                    lineRenderer.enabled = false;
                }
            }
        }
    }
    Vector3 CalculateAcceleration(int i, VirtualBody[] virtualBodies)
    {
        Vector3 acceleration = Vector3.zero;
        if (virtualBodies[i].cantBeAtracted) return acceleration;
        for (int j = 0; j < virtualBodies.Length; j++)
        {
            if (i == j)
            {
                continue;
            }

            if (virtualBodies[i].position.sqrMagnitude < ((virtualBodies[j].position - virtualBodies[i].position).normalized * virtualBodies[j].gravityRadius).sqrMagnitude)
            {
                float sqrDst = (virtualBodies[j].position - virtualBodies[i].position).sqrMagnitude;
                Vector3 forceDir = (virtualBodies[j].position - virtualBodies[i].position).normalized;
                acceleration += forceDir * Universe.gravitationalConstant * virtualBodies[j].mass / sqrDst;
            }
            else
            {
                acceleration = Vector3.zero;
            }


        }
        return acceleration;
    }
    private void OnValidate()
    {
        if (usePhysicsTimeStep)
        {
            timeStep = Universe.physicsTimeStep;
        }
    }
    private void Update()
    {
        if (!Application.isPlaying)
        {
            DrawTrajectory();
        }
    }

    class VirtualBody
    {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;
        public float gravityRadius;
        public bool cantBeAtracted;

        public VirtualBody(gravityBomb body)
        {
            position = body.transform.position;
            velocity = body.velocity;
            cantBeAtracted = body.cantBeAtracted;
            gravityRadius = body.radius;
        }
    }
}
