using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
public class DirectionalMissile : MissileBase
{
    [SerializeField] private float speed = 20f;
    public override void Start()
    {
        base.Start();
    }

    public void Update()
    {
        if (enable)
        {
            transform.parent = null;
            StartCoroutine(Rotate());
            StartCoroutine(Go());
        }
    }
    
    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(0.1f);
        transform.LookAt(target.transform);
    }
    
    IEnumerator Go()
    {
        yield return new WaitForSeconds(1f);
        rigidbody.velocity = transform.forward * speed;
    }
}
