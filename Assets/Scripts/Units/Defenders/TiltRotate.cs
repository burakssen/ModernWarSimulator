using System;
using UnityEngine;

public class TiltRotate : MonoBehaviour
{
    [SerializeField] private GameObject tilt;
    [SerializeField] private GameObject rotate;
    [SerializeField] private GameObject tiltPoint;
    [SerializeField] private GameObject rotatePoint;
    [SerializeField] public bool tiltable;
    [SerializeField] public bool rotatable;
    private float tiltAngle;
    private float currentTiltAngle;
    private float rotateAngle;
    private float currentRotateAngle;
    private float tAngle;
    private float rAngle;
    private GameObject currentTarget;

    [SerializeField] private float Speed = 200f;
    private Vector3 direction;

    public void Update()
    {
        if (!currentTarget)
            return;

        direction = currentTarget.transform.position - transform.position;
        tiltAngle = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;
        rotateAngle = 90f - Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        if (tiltable)
            Tilt();

        if (rotatable)
            Rotate();
    }

    public void SetCurrentTarget(GameObject target)
    {
        currentTarget = target;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }


    public void Tilt()
    {
        if (Math.Abs(Mathf.RoundToInt(currentTiltAngle) - Mathf.RoundToInt(tiltAngle)) < 2 || tiltAngle < 0 ||
            tiltAngle > 85)
            return;

        var lAngle = Speed * Time.deltaTime;

        if (currentTiltAngle > tiltAngle) lAngle = -lAngle;

        tilt.transform.RotateAround(tiltPoint.transform.position, -tiltPoint.transform.right, lAngle);
        currentTiltAngle += lAngle;
    }

    public void Rotate()
    {
        if (Math.Abs(Mathf.RoundToInt(currentRotateAngle) - Mathf.RoundToInt(rotateAngle)) < 2)
            return;

        var rAngle = Speed * Time.deltaTime;

        if (currentRotateAngle > rotateAngle) rAngle = -rAngle;

        rotate.transform.RotateAround(rotatePoint.transform.position, tiltPoint.transform.up, rAngle);
        currentRotateAngle += rAngle;
    }
}