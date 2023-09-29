using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rb;


    [InspectorName("SubmergeDepth")]
    [Range(0.01f, 5f)]
    public float depthBeforeSubmerged = 1f;

    [Range(0.01f, 5f)]
    public float displacementAmount = 3f;

    public int floaterCount = 1;

    [Range(0.01f, 5f)]
    public float waterDrag = 0.99f;

    [Range(0.01f, 5f)]
    public float waterAngularDrag = 0.5f;
    private void Awake()
    {
    }
    private void FixedUpdate()
    {
        rb.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);
        float waveHeigth = WaveController.instance.GetWaveHeight(transform.position.x);
        if (transform.position.y < waveHeigth)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeigth - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rb.AddForce(displacementMultiplier * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
