using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class HomingMissile : MissileBase
{
    [FormerlySerializedAs("thurst")] [SerializeField]
    protected float thrust = 0f;

    [SerializeField] private float maxAngularVelocity = 20;

    [SerializeField] [Range(-10, 10)] private float xP, xI, xD;

    [SerializeField] [Range(-10, 10)] private float yP, yI, yD;

    [SerializeField] [Range(-10, 10)] private float zP, zI, zD;

    private bool firstLaunch = false;
    private bool wait = true;
    private PIDController xPIDController;
    private PIDController yPIDController;
    private PIDController zPIDController;

    public override void Start()
    {
        base.Start();
        rigidbody.maxAngularVelocity = maxAngularVelocity;
        xPIDController = new PIDController(xP, xI, xD);
        yPIDController = new PIDController(yP, yI, yD);
        zPIDController = new PIDController(zP, zI, zD);
        
    }

    private void Update()
    {
        if (Global.gameState == Global.GameState.play && firstLaunch)
        {
            rigidbody.AddForce(Vector3.up * 20f, ForceMode.Impulse);
            firstLaunch = false;
            Invoke(nameof(EnableAlgorithm), 0.5f);
        }

        xPIDController.SetValues(xP, xI, xD);
        yPIDController.SetValues(yP, yI, yD);
        zPIDController.SetValues(zP, zI, zD);
    }


    private void FixedUpdate()
    {
        if (target && enable && firstLaunch == false) HomingMissileAlgorithm();
    }

    private void EnableAlgorithm()
    {
        enable = true;
        transform.parent = null;
        firstLaunch = false;
    }

    public override void SetTarget(GameObject t)
    {
        base.SetTarget(t);
        if (enable == false)
            firstLaunch = true;
    }

    private void HomingMissileAlgorithm()
    {
        var targetDirection = -transform.position + target.transform.position;
        var rotationDirection = Vector3.RotateTowards(transform.forward, targetDirection, 360, 0.00f);
        var targetRotation = Quaternion.LookRotation(rotationDirection);

        var xAngleError = Mathf.DeltaAngle(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.x);
        var xTorqueCorrection = xPIDController.GetOutput(xAngleError, Time.fixedDeltaTime);

        var yAngleError = Mathf.DeltaAngle(transform.rotation.eulerAngles.y, targetRotation.eulerAngles.y);
        var yTorqueCorrection = yPIDController.GetOutput(yAngleError, Time.fixedDeltaTime);

        var zAngleError = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, targetRotation.eulerAngles.z);
        var zTorqueCorrection = zPIDController.GetOutput(zAngleError, Time.fixedDeltaTime);


        rigidbody.AddRelativeTorque(xTorqueCorrection * Vector3.right + yTorqueCorrection * Vector3.up +
                                    zTorqueCorrection * Vector3.forward);
        rigidbody.AddRelativeForce(Vector3.forward * (thrust * Time.fixedDeltaTime));
    }
}