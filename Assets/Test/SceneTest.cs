using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTest : MonoBehaviour
{
    public Transform sphere1;
    public Transform sphere2;

    private float radius1 = 10f;
    private float radius2 = 5f;

    void OnDrawGizmos()
    {
        // Desenha as esferas
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sphere1.position, radius1);
        Gizmos.DrawWireSphere(sphere2.position, radius2);

        // Desenha a linha da esfera 1 para a esfera 2
        Gizmos.color = Color.blue;
        Vector3 direction = sphere2.position - sphere1.position;
        if (direction.magnitude > radius1)
        {
            direction = direction.normalized * radius1;
        }
        Gizmos.DrawLine(sphere1.position, sphere1.position + direction);
    }
}
