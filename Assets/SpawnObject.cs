using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private GameObject missile;
    [SerializeField] float timer = 4f;
    [SerializeField] private GameObject targetObj;
    private float timeVal;

    void Start()
    {
        timeVal = timer;
    }
    void Update()
    {
        timeVal -= Time.deltaTime;
        if (timeVal <= 0f)
        {
            GameObject t = Instantiate(missile, transform.position, missile.transform.rotation);
            t.GetComponent<Missile>().SetTarget(targetObj);
            timeVal = timer;
        }
    }
}
