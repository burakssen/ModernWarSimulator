using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollide : MonoBehaviour
{
    public float health;

    public void OnCollisionEnter(Collision collision)
    {
        health -= 200;
        
        if(health <= 0)
            Destroy(gameObject);
    }
}
