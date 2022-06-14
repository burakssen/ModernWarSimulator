using UnityEngine;

public abstract class BaseDefence : MonoBehaviour
{
    [SerializeField] public GameObject[] spawnPoints;
    [SerializeField] public bool setTargets = false;
    public float health = 100;
    public float damage = 1000;

    [SerializeField] public GameObject currentTarget;
    public virtual void SetTargets(GameObject target)
    {
        currentTarget = target;
    }

    public virtual void Update()
    {
        if(health <= 0)
            DestroyDefence();
    }

    public void SetValues(float damage, float health)
    {
        this.damage = damage;
        this.health = health;
    }

    public void DestroyDefence()
    {
        Destroy(gameObject);
    }
    

    public void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("AttackerMissile"))
            return;

        health -= collision.transform.GetComponent<MissileBase>().GetDamage();
        collision.transform.GetComponent<MissileBase>().DestroyMissile();
        if(health<=0)
            DestroyDefence();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetType() != typeof(BoxCollider))
            return;
        
        if (other.transform.CompareTag("Attacker") && currentTarget == null)
        {
            currentTarget = other.transform.gameObject;
            setTargets = true;
        }
    }
}
