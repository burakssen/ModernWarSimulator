using System;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject missile;
    [SerializeField] private GameObject target;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 3f, 3f);
    }

    private void Spawn()
    {
        var m = Instantiate(missile, transform);
        m.transform.parent = null;

        m.GetComponent<MissileBase>().target = target;
    }
}