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
    [SerializeField] public GameObject healthBar;
    private HealthBarHandler healthBarHandler;
    public List<GameObject> missiles;
    [SerializeField] public GameObject explosion;

    public float damage = 100;
    public float health = 1000;
    public float initialHealth;
    [SerializeField] private float point = 500;

    public void Awake()
    {
        healthBarHandler = healthBar.GetComponent<HealthBarHandler>();
    }

    public virtual void Start()
    {
        velocity = new Vector3();
        Global.attackerNumber += 1;
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
        this.initialHealth = health;
        healthBarHandler.SetHealthBarValue(this.health);
    }

    public void SetMissile(GameObject missile)
    {
        this.missile = missile;
    }

    public void DestroyAttacker()
    {
        Global.attackerNumber -= 1;
        Global.point += point;
        if (Global.attackerNumber <= 0)
            Global.gameState = Global.GameState.won;
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("DefenceMissile"))
            return;
        
        health -= collision.transform.GetComponent<MissileBase>().GetDamage();
        collision.transform.GetComponent<MissileBase>().DestroyMissile();
        healthBarHandler.SetHealthBarValue(health / initialHealth);
        if (health <= 0)
            DestroyAttacker();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetType() != typeof(BoxCollider))
            return;


        if (other.transform.CompareTag("Defence") || other.CompareTag("Base"))
        {
            if (currentTarget == null)
                SetTargets(other.gameObject);
            else
                SetTargets(currentTarget);
            setTargets = true;
        }
    }
}