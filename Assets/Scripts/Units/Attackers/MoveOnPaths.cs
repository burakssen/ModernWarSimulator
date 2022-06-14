using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MoveOnPaths : MonoBehaviour
{
    [SerializeField] private List<PathList> allPaths; 
    [SerializeField] private List<Transform> paths;
    private int pathNum;
    private Vector3 objectPosition;
    private float speed;
    private float rotationSpeed;
    private bool enableCouroutine;
    private float time;
    private Random random;
    void Start()
    {
        random = new Random();
        pathNum = 0;
        time = 0f;
        speed = 0.25f;
        rotationSpeed = 2f;
        enableCouroutine = true;
        paths = new List<Transform>();
        ChooseRandomPath();
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
        
        Vector3 p0 = MultiplyOnXZ(paths[pathNum].GetChild(0).position, Global.mapSize);
        Vector3 p1 = MultiplyOnXZ(paths[pathNum].GetChild(2).position, Global.mapSize);
        Vector3 p2 = MultiplyOnXZ(paths[pathNum].GetChild(3).position, Global.mapSize);
        Vector3 p3 = MultiplyOnXZ(paths[pathNum].GetChild(1).position, Global.mapSize);
        
        while(time < 1)
        {
            time += Time.deltaTime * speed;
            objectPosition = paths[pathNum].GetComponent<Path>().GetPointAtTimeWithControlPoints(time, p0, p1, p2, p3);
            transform.GetComponent<AttackerBase>().SetVelocity(objectPosition - transform.localPosition);
            var rotation = Quaternion.LookRotation(-objectPosition + transform.localPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, time * rotationSpeed);
            transform.localPosition = objectPosition;
            yield return new WaitForFixedUpdate();
        }

        time = 0f;
        pathNum += 1;
        if(this.pathNum > paths.Count - 1)
        {
            this.pathNum = 0;
            paths.Clear();
            ChooseRandomPath();
        }

        enableCouroutine = true;
    }

    void ChooseRandomPath()
    {
        int val = random.Next(0, allPaths.Count);
        foreach (var path in allPaths[val].paths)
        {
            paths.Add(path);
        }
    }

    Vector3 MultiplyOnXZ(Vector3 vector, float val)
    {
        return new Vector3(vector.x * val, vector.y, vector.z * val);
    }

    public void SetPathPosition(Transform pos)
    {
        if (paths.Count <= 0)
            return;
        
        foreach (var path in paths)
        {
            path.position = pos.position;
        }
    }
}

[System.Serializable]
public class PathList
{
    public List<Transform> paths;
}
