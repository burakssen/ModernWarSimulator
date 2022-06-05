using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName="ScriptableObjects/Waypoints", order=2)
]
public class WayPoint : ScriptableObject
{
    private List<GameObject> points;
    [SerializeField] private GameObject path;

    public void SetAllPoints()
    {
        points = new List<GameObject>();
        for (int i = 0; i < path.transform.childCount; i++)
        {
            points.Add(path.transform.GetChild(i).gameObject);
        }
        Debug.Log("HERE");
    }
    
    public GameObject GetNextPoint(int pointIndex)
    {
        return points[pointIndex];
    }

    public int GetNumberOfPoints()
    {
        return points.Count;
    }
}
