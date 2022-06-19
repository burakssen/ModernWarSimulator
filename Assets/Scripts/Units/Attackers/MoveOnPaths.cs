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

    private void Start()
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

    private void Update()
    {
        if (enableCouroutine) StartCoroutine(MoveByPaths());
    }

    private IEnumerator MoveByPaths()
    {
        enableCouroutine = false;

        var p0 = MultiplyOnXZ(paths[pathNum].GetChild(0).position, Global.mapSize);
        var p1 = MultiplyOnXZ(paths[pathNum].GetChild(2).position, Global.mapSize);
        var p2 = MultiplyOnXZ(paths[pathNum].GetChild(3).position, Global.mapSize);
        var p3 = MultiplyOnXZ(paths[pathNum].GetChild(1).position, Global.mapSize);

        while (time < 1)
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
        if (pathNum > paths.Count - 1)
        {
            pathNum = 0;
            paths.Clear();
            ChooseRandomPath();
        }

        enableCouroutine = true;
    }

    private void ChooseRandomPath()
    {
        var ratio = random.Next(0, 100);

        var leftCounterRatio = 0; //= (Global.leftCounter / (Global.leftCounter + Global.rightCounter)) * 100;

        var leftSide = new List<PathList>();
        var rightSide = new List<PathList>();

        foreach (var _paths in allPaths)
        {
            if (_paths.side == PathList.Side.Left)
                leftSide.Add(_paths);

            if (_paths.side == PathList.Side.Right)
                rightSide.Add(_paths);
        }

        if (ratio > leftCounterRatio)
        {
            var val = random.Next(0, leftSide.Count);

            foreach (var path in leftSide[val].paths) paths.Add(path);
        }
        else
        {
            var val = random.Next(0, rightSide.Count);

            foreach (var path in rightSide[val].paths) paths.Add(path);
        }
    }

    private Vector3 MultiplyOnXZ(Vector3 vector, float val)
    {
        return new Vector3(vector.x * val, vector.y, vector.z * val);
    }

    public void SetPathPosition(Transform pos)
    {
        if (paths.Count <= 0)
            return;


        foreach (var path in paths) path.position = pos.position;
    }
}

[Serializable]
public class PathList
{
    public List<Transform> paths;

    public Side side;

    [Serializable]
    public enum Side
    {
        Left,
        Right
    }
}