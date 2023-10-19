using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitDebugDisplay : MonoBehaviour
{
    public int numSteps = 1000;
    public float timeStep = 0.1f;
    public bool usePhysicsTimeStep;
    public bool relativeToBody;
    public GravityBody centralBody;
    public float width = 100;
    public bool useThickLines;
    public Color pathColor;
    [SerializeField] private VirtualBody[] virtualBodies;

    public bool showTrajectory;

    void DrawTrajectory()
    {
        GravityBody[] bodies = FindObjectsOfType<GravityBody>();
        virtualBodies = new VirtualBody[bodies.Length];
        var drawPoints = new Vector3[bodies.Length][];
        int referenceFrameIndex = 0;

        Vector3 referenceBodyInitialPosition = Vector3.zero;

        for (int i = 0; i < virtualBodies.Length; i++)
        {
            virtualBodies[i] = new VirtualBody(bodies[i]);
            drawPoints[i] = new Vector3[numSteps];

            if (bodies[i] == centralBody && relativeToBody)
            {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = virtualBodies[i].position;
            }
        }

        //Simulate

        for (int step = 0; step < numSteps; step++)
        {
            Vector3 referenceBodyPosition = relativeToBody ? virtualBodies[referenceFrameIndex].position : Vector3.zero;

            for (int i = 0; i < virtualBodies.Length; i++)
            {
                virtualBodies[i].velocity += CalculateAcceleration(i, virtualBodies) * timeStep;
            }

            //update position
            for (int i = 0; i < virtualBodies.Length; i++)
            {
                Vector3 newPos = virtualBodies[i].position + virtualBodies[i].velocity * timeStep;
                virtualBodies[i].position = newPos;
                if (relativeToBody)
                {
                    var referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
                    newPos -= referenceFrameOffset;
                }
                if (relativeToBody && i == referenceFrameIndex)
                {
                    newPos = referenceBodyInitialPosition;
                }
                drawPoints[i][step] = newPos;
            }
        }

        // draw paths

        for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++)
        {
            pathColor = bodies[bodyIndex].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().sharedMaterial.color;
            if (useThickLines)
            {
                var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
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
        for (int j = 0; j < virtualBodies.Length; j++)
        {
            if (i == j || !virtualBodies[j].cantBeAtracted)
            {
                continue;
            }
            if(virtualBodies[i].position.sqrMagnitude < ((virtualBodies[j].position - virtualBodies[i].position).normalized * virtualBodies[j].gravityRadius).sqrMagnitude)
            {
                float sqrDst = (virtualBodies[j].position - virtualBodies[i].position).sqrMagnitude;
                Vector3 forceDir = (virtualBodies[j].position - virtualBodies[i].position).normalized;
                acceleration += forceDir * Universe.gravitationalConstant * virtualBodies[j].mass / sqrDst;
            }
            else{
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

        public VirtualBody(GravityBody body)
        {
            position = body.transform.position;
            velocity = body.initialVelocity;
            mass = body.mass;
            cantBeAtracted = body.cantBeAtracted;
            gravityRadius = body.gravityRadius;
        }
    }

}
