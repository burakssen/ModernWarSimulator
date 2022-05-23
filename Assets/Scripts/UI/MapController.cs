using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public Transform MapToScroll;
    Vector3 PreviousWorldPoint;
    void ProcessMiddleButton()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.up, Vector3.zero);
        float distance = 0;
        if (plane.Raycast( ray, out distance))
        {
            Vector3 worldPoint = ray.GetPoint( distance);
 
            if (Input.GetMouseButtonDown(2))
            {
                PreviousWorldPoint = worldPoint;
            }
            
            if (Input.GetMouseButton(2))
            {
                Vector3 worldDelta = worldPoint - PreviousWorldPoint;
                MapToScroll.position += worldDelta;
            }
 
            PreviousWorldPoint = worldPoint;
        }
    }
 
    void Update ()
    {
        ProcessMiddleButton();
    }
}
