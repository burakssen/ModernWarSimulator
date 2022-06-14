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
    [SerializeField] public GameObject missile;
    public List<GameObject> missiles;
    
    public float damage = 100;
    public float health = 1000;

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

    public void SetValues(float damage, float health)
    {
        this.damage = damage;
        this.health = health;
    }

    public void SetMissile(GameObject missile)
    {
        this.missile = missile;
    }
    public void DestroyAttacker()
    {
        Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("DefenceMissile"))
            return;
        
        health -= collision.transform.GetComponent<MissileBase>().GetDamage();
        if(health<=0)
            DestroyAttacker();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetType() != typeof(BoxCollider))
            return;
        
        if (other.transform.CompareTag("Defence") && currentTarget == null)
        {
            SetTargets(other.gameObject);
            setTargets = true;
        }
    }
}
