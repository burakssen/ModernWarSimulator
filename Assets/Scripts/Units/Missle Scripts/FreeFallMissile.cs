using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFallMissile : MissileBase
{
    private GameObject parent;
    private bool activate;
    [SerializeField] private Vector3 currentVelocity;

    public override void Start()
    {
        base.Start();
        parent = transform.parent.transform.parent.transform.parent.gameObject;
        activate = false;
    }

    private void FixedUpdate()
    {
        if (!enable || !target)
            return;

        currentVelocity = parent.GetComponent<AttackerBase>().GetVelocity();

        if (activate)
        {
            transform.parent = null;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            rigidbody.velocity += currentVelocity * 2f;
            enable = false;
            return;
        }

        var deltaPosition = transform.position - target.transform.position;
        var time = Mathf.Sqrt(2 * deltaPosition.y / Physics.gravity.magnitude);
        var timeNeeded = Mathf.Abs(deltaPosition.z / currentVelocity.z);

        if (Mathf.Abs(time - timeNeeded) < 1f) activate = true;
    }

    public override void SetTarget(GameObject t)
    {
        base.SetTarget(t);
    }
}