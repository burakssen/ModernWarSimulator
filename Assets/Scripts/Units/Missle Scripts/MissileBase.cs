using System;
using UnityEngine;

public class MissileBase : MonoBehaviour
{
    [SerializeField] public GameObject target;
    [SerializeField] public bool enable = false;
    [SerializeField] private GameObject explosion;
    protected new Rigidbody rigidbody;
    public float damage = 100f;

    public virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        SetTarget(target);
    }

    public virtual void SetTarget(GameObject t)
    {
        target = t;
        DestroyMissile(7f);
    }

    public float GetDamage()
    {
        return damage;
    }

    public void DestroyMissile(float time = 0f)
    {
        if(time == 0)
            Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject, time);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Plane")) Destroy(gameObject);
    }
}