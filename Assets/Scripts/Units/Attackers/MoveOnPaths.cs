using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnPaths : MonoBehaviour
{
    [SerializeField] private List<Transform> paths;
    private int pathNum;
    private Vector3 objectPosition;
    private float speed;
    private float rotationSpeed;
    private bool enableCouroutine;
    private float time;
    
    void Start()
    {
        pathNum = 0;
        time = 0f;
        speed = 0.25f;
        rotationSpeed = 2f;
        enableCouroutine = true;
    }
    void Update()
    {
        if (enableCouroutine)
        {
            StartCoroutine(MoveByPaths());
        }
    }

    private IEnumerator MoveByPaths()
    {
        enableCouroutine = false;
        
        Vector3 p0 = paths[pathNum].GetChild(0).position;
        Vector3 p1 = paths[pathNum].GetChild(2).position;
        Vector3 p2 = paths[pathNum].GetChild(3).position;
        Vector3 p3 = paths[pathNum].GetChild(1).position;
        
        while(time < 1)
        {
            time += Time.deltaTime * speed;
            objectPosition = paths[pathNum].GetComponent<Path>().GetPointAtTimeWithControlPoints(time, p0, p1, p2, p3);
            transform.GetComponent<AttackerBase>().SetVelocity(objectPosition - transform.position);
            var rotation = Quaternion.LookRotation(-objectPosition + transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, time * rotationSpeed);
            transform.position = objectPosition;
            yield return new WaitForFixedUpdate();
        }

        time = 0f;
        pathNum += 1;
        if(this.pathNum > paths.Count - 1)
        {
            this.pathNum = 0;
        }

        enableCouroutine = true;
    }
}
