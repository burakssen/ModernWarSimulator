using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // Start is called before the first frame update
    private Action algorithm;
    public bool useDirectional = false;
    [SerializeField]
    [Range(0, 2000)]
    private float thurst = 1000f;
    [SerializeField]
    private float maxAngulerVelocity = 20;
    [SerializeField] GameObject target;

    [SerializeField]
    [Range(-10, 10)]
    private float xP, xI, xD;

    [SerializeField]
    [Range(-10, 10)]
    private float yP, yI, yD;

    [SerializeField]
    [Range(-10, 10)]
    private float zP, zI, zD;

    private PIDController xPIDController;
    private PIDController yPIDController;
    private PIDController zPIDController;

    [SerializeField] private bool enable = false;
    private bool firstLaunch = true;
    
    private new Rigidbody rigidbody;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.maxAngularVelocity = maxAngulerVelocity; 
        xPIDController = new PIDController(xP, xI, xD);
        yPIDController = new PIDController(yP, yI, yD);
        zPIDController = new PIDController(zP, zI, zD);
        algorithm = HomingMissileAlgorithm;
        Destroy(gameObject, 7f);
    }
    void Update()
    {
        if (firstLaunch)
        {
            rigidbody.AddForce(Vector3.up * 20, ForceMode.Impulse);
            firstLaunch = false;
            StartCoroutine(EnableAlgorithm(0.5f));
        }
        
        xPIDController.SetValues(xP, xI, xD);
        yPIDController.SetValues(yP, yI, yD);
        zPIDController.SetValues(zP, zI, zD);
    }

    private void FixedUpdate()
    {
        if(target && enable)
            RunAlgorithm(algorithm);
    }

    IEnumerator EnableAlgorithm(float delay)
    {
        yield return new WaitForSeconds(delay);
        rigidbody.angularDrag = 0.05f;
        enable = true;
    }

    void RunAlgorithm(Action _algorithm, object[] args = null)
    {
        _algorithm.DynamicInvoke();
    }

    public void SetTarget(GameObject t)
    {
        firstLaunch = true;
        target = t;
    }

    void HomingMissileAlgorithm()
    {
        var targetDirection = -transform.position + target.transform.position;
        Vector3 rotationDirection = Vector3.RotateTowards(transform.forward, targetDirection, 360, 0.00f);
        Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);

        float xAngleError = Mathf.DeltaAngle(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.x);
        float xTorqueCorrection = xPIDController.GetOutput(xAngleError, Time.fixedDeltaTime);

        float yAngleError = Mathf.DeltaAngle(transform.rotation.eulerAngles.y, targetRotation.eulerAngles.y);
        float yTorqueCorrection = yPIDController.GetOutput(yAngleError, Time.fixedDeltaTime);

        float zAngleError = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, targetRotation.eulerAngles.z);
        float zTorqueCorrection = zPIDController.GetOutput(zAngleError, Time.fixedDeltaTime);

        rigidbody.AddRelativeTorque((xTorqueCorrection * Vector3.right) + (yTorqueCorrection * Vector3.up) + (zTorqueCorrection * Vector3.forward));
        rigidbody.AddRelativeForce((Vector3.forward) * thurst * Time.fixedDeltaTime);
    }

    void DirectionalMissileAlgorithm()
    {
        Debug.Log("Directional");
    }
}
