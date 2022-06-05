using System;
using UnityEngine;

public class MissileBase : MonoBehaviour
{
    [SerializeField] public GameObject target;
    [SerializeField] public bool enable = false;
    protected new Rigidbody rigidbody;

    public virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    
    public virtual void SetTarget(GameObject t)
    {
        target = t;
    }
}
