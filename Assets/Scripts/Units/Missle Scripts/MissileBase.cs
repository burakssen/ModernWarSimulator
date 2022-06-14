using System;
using UnityEngine;

public class MissileBase : MonoBehaviour
{
    [SerializeField] public GameObject target;
    [SerializeField] public bool enable = false;
    protected new Rigidbody rigidbody;
    private float damage = 100f;

    public virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    
    public virtual void SetTarget(GameObject t)
    {
        target = t;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void DestroyMissile()
    {
        Destroy(gameObject);
    }
}
