using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerBase : MonoBehaviour
{
    [SerializeField] public GameObject[] spawnPoints;
    [SerializeField] public bool setTargets = false;
    protected Vector3 velocity;
    [SerializeField] public GameObject currentTarget;

    public virtual void Start()
    {
        velocity = new Vector3();
    }

    public virtual void SetTargets(GameObject target)
    {
        currentTarget = target;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void SetVelocity(Vector3 velocity)
    {
        this.velocity = velocity;
    }
}
