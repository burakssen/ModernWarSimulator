using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UIElements;

public class Path : MonoBehaviour
{
    [SerializeField] private Transform[] controlPoints;

    private Vector3 gizmosPosition;

    private void OnDrawGizmos()
    {
        for (float i = 0; i <= 1; i += 0.05f)
        {
            gizmosPosition = GetPointAtTime(i);
            Gizmos.DrawSphere(gizmosPosition, 1f);
        }

        DrawGizmoLine(0, 1);
        DrawGizmoLine(2, 3);
    }

    public Vector3 GetPointAtTime(float time)
    {
        var p = MathF.Pow(1f - time, 3) * controlPoints[0].transform.position;
        p += 3 * MathF.Pow(1f - time, 2) * time * controlPoints[1].transform.position;
        p += 3 * (1f - time) * MathF.Pow(time, 2) * controlPoints[2].transform.position;
        p += MathF.Pow(time, 3) * controlPoints[3].transform.position;
        return p;
    }

    public Vector3 GetPointAtTimeWithControlPoints(float time, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var p = MathF.Pow(1f - time, 3) * p0;
        p += 3 * MathF.Pow(1f - time, 2) * time * p1;
        p += 3 * (1f - time) * MathF.Pow(time, 2) * p2;
        p += MathF.Pow(time, 3) * p3;
        return p;
    }

    private void DrawGizmoLine(int handle1, int handle2)
    {
        Gizmos.DrawLine(
            new Vector3(
                controlPoints[handle1].position.x,
                controlPoints[handle1].position.y,
                controlPoints[handle1].position.z
            ),
            new Vector3(
                controlPoints[handle2].position.x,
                controlPoints[handle2].position.y,
                controlPoints[handle2].position.z
            )
        );
    }
}