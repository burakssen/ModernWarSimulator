using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public float health = 100;
    public float initialHealth;
    [SerializeField] private GameObject explosion;

    [SerializeField] public GameObject healthBar;
    private HealthBarHandler healthBarHandler;

    public void Awake()
    {
        healthBarHandler = healthBar.GetComponent<HealthBarHandler>();
    }


    public void SetValues(float health)
    {
        this.health = health;
        this.initialHealth = health;
        healthBarHandler.SetHealthBarValue(this.health);
    }

    public void DestroyBase()
    {
        Global.gameState = Global.GameState.lose;
        Instantiate(explosion, transform.position, Quaternion.identity);
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
            DestroyBase();
    }
}