using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFallMissile : MissileBase
{
    private GameObject parent;
    bool activate;
    [SerializeField] private Vector3 currentVelocity;
    private bool disable;
    public override void Start()
    {
        base.Start();
        parent = transform.parent.transform.parent.transform.parent.gameObject;
        disable = false;
        activate = false;
    }
    
    void FixedUpdate()
    {
        if (disable || !target)
            return;

        currentVelocity = parent.GetComponent<AttackerBase>().GetVelocity();

        if (activate)
        {
            transform.parent = null;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            rigidbody.velocity += currentVelocity * 2f;
            disable = true;
            return;
        }
        
        Vector3 deltaPosition = transform.position - target.transform.position;
        float time = Mathf.Sqrt(2 * deltaPosition.y / Physics.gravity.magnitude);
        float timeNeeded = Mathf.Abs(deltaPosition.z / currentVelocity.z);
        
        if (Mathf.Abs(time - timeNeeded) < 1f)
        {
            activate = true;
        }
    }

    public override void SetTarget(GameObject t)
    {
        base.SetTarget(t);
    }
}
