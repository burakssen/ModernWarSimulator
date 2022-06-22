using System;
using UnityEngine;

public abstract class BaseDefence : MonoBehaviour
{
    [SerializeField] public GameObject[] spawnPoints;
    public float health = 100;
    public float initialHealth;

    [SerializeField] public GameObject currentTarget;
    [SerializeField] public GameObject healthBar;
    private HealthBarHandler healthBarHandler;

    [SerializeField] private GameObject explosion;
    public void Awake()
    {
        healthBarHandler = healthBar.GetComponent<HealthBarHandler>();
    }

    public virtual void SetTargets(GameObject target)
    {
        currentTarget = target;
    }

    public void SetValues(float health)
    {
        this.health = health;
        this.initialHealth = health;
        healthBarHandler.SetHealthBarValue(this.health);
    }

    public void DestroyDefence()
    {
        GameObject exp = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(exp, 2f);
        Destroy(gameObject);
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("AttackerMissile"))
            return;

        health -= collision.transform.GetComponent<MissileBase>().GetDamage();
        collision.transform.GetComponent<MissileBase>().DestroyMissile();
        healthBarHandler.SetHealthBarValue(health / initialHealth);
        if (health <= 0)
            DestroyDefence();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetType() != typeof(BoxCollider))
            return;

        if (other.transform.CompareTag("Attacker"))
        {
            if (currentTarget == null)
                SetTargets(other.transform.gameObject);
            else
            {
                if((transform.position - currentTarget.transform.position).magnitude < 50)
                    SetTargets(currentTarget);
                else
                {
                    SetTargets(other.transform.gameObject);
                }
            }
        }
    }
}