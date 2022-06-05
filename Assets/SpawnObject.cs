using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private GameObject missile;

    [SerializeField] private float time = 0f;

    [SerializeField] private GameObject target;
    private void Update()
    {
        if (Time.time > time)
        {
            time += 4f;
            GameObject g = Instantiate(missile);
            g.GetComponent<MissileBase>().SetTarget(target);
        }
    }
}
