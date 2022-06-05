using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class HomingMissile : MissileBase
{
        
        [FormerlySerializedAs("thurst")]
        [SerializeField]
        
        protected float thrust = 0f;
        
        [SerializeField] [Range(0, 2000)] private float thrustLimit = 100f;
        
        [SerializeField]
        private float maxAngularVelocity = 20;
        
        [SerializeField]
        [Range(-10, 10)]
        private float xP, xI, xD;

        [SerializeField]
        [Range(-10, 10)]
        private float yP, yI, yD;

        [SerializeField]
        [Range(-10, 10)]
        private float zP, zI, zD;

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
                if (firstLaunch)
                {
                
                        rigidbody.AddForce(Vector3.up , ForceMode.Impulse);
                        firstLaunch = false;
                        StartCoroutine(EnableAlgorithm(0.5f));
                }
        
                xPIDController.SetValues(xP, xI, xD);
                yPIDController.SetValues(yP, yI, yD);
                zPIDController.SetValues(zP, zI, zD);
        }
        
        

        private void FixedUpdate()
        {
                if (target && enable)
                {
                        
                        HomingMissileAlgorithm();
                }
        }

        IEnumerator EnableAlgorithm(float delay)
        {
                yield return new WaitForSeconds(delay);
                rigidbody.angularDrag = 0.05f;
                enable = true;
        }

        public override void SetTarget(GameObject t)
        {
                base.SetTarget(t);
                firstLaunch = true;
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
                rigidbody.AddRelativeForce((Vector3.forward) * (thrust * Time.fixedDeltaTime));
        }
}
