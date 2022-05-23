using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public abstract class BaseDefence : MonoBehaviour
{
    [SerializeField] public GameObject[] spawnPoints;
    [SerializeField] public List<GameObject> targetPool;
    public GameObject currentTarget;
    [SerializeField] private bool setTargets = false;
    [SerializeField] private GameObject tilt;
    [SerializeField] private GameObject rotate;
    [SerializeField] private GameObject tiltPoint;
    [SerializeField] private GameObject rotatePoint;
    [SerializeField] private bool shoot = false;
    [SerializeField] private GameObject bullet;
    [Range(0, 1000)]
    [SerializeField] private float projectileSpeed;
    private float tiltAngle;
    private float currentTiltAngle;
    private float rotateAngle;
    private float currentRotateAngle;
    private float tAngle;
    private float rAngle;
    private Vector3 direction;

    [SerializeField] private float Speed = 200f;

    public void Start()
    {
        
    }
    
    public virtual void Update()
    {
        direction = currentTarget.transform.position - transform.position;
        tiltAngle = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;
        rotateAngle = 90f - Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        
        Tilt();
        Rotate();
        
        if (setTargets)
        {
            SetTargets();
            setTargets = false;
            if (targetPool.Count > 0)
                targetPool.Remove(targetPool[0]);
        }

        if (shoot)
        {
            StartCoroutine(Shoot());
            shoot = false;
        }
    }

    public abstract void SetTargets();
    
    public void Tilt()
    {

        if (Math.Abs(Mathf.RoundToInt(currentTiltAngle) - Mathf.RoundToInt(tiltAngle)) < 2 || tiltAngle < 0 || tiltAngle > 85)
            return;
        
        float lAngle = Speed * Time.deltaTime;

        if (currentTiltAngle > tiltAngle)
        {
            lAngle = -lAngle;
        }
        
        tilt.transform.RotateAround(tiltPoint.transform.position, -tiltPoint.transform.right, lAngle);
        currentTiltAngle += lAngle;
    }

    public void Rotate()
    {
        if (Math.Abs(Mathf.RoundToInt(currentRotateAngle) - Mathf.RoundToInt(rotateAngle)) < 2)
            return;
        
        float rAngle = Speed * Time.deltaTime;

        if (currentRotateAngle > rotateAngle)
        {
            rAngle = -rAngle;
        }
        
        rotate.transform.RotateAround(rotatePoint.transform.position, tiltPoint.transform.up, rAngle);
        currentRotateAngle += rAngle;
    }

    public IEnumerator Shoot()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            yield return new WaitForSeconds(0.2f);

            GameObject b = Instantiate(bullet, spawnPoint.transform.position, Quaternion.identity);
            b.GetComponent<Rigidbody>().AddForce(direction * direction.magnitude, ForceMode.Force);
        }
    }
}
