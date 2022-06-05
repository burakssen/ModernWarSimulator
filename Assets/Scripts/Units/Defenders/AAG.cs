using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAG : BaseDefence
{
    [SerializeField] private bool shoot = false;
    [SerializeField] private GameObject bullet;
    [Range(0, 1)]
    [SerializeField] private float projectileSpeed;

    private Vector3 direction;
    private IEnumerator enumerator;
    private TiltRotate tiltRotate;

    private void Start()
    {
        tiltRotate = GetComponent<TiltRotate>();
        enumerator = Shoot();
    }

    public void Update()
    {
        if (!setTargets)
            return;
        
        SetTargets(currentTarget);
        
        if (!shoot)
            return;
        
        StartCoroutine(enumerator);
        shoot = false;
        setTargets = false;
    }

    public override void SetTargets(GameObject target)
    {
        base.SetTargets(target);
        tiltRotate.SetCurrentTarget(target);
        direction = tiltRotate.GetDirection();
    }
    
    public IEnumerator Shoot()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            yield return new WaitForSeconds(0.2f);

            GameObject b = Instantiate(bullet, spawnPoint.transform.position, Quaternion.identity);
            b.GetComponent<Rigidbody>().AddForce(direction * (direction.magnitude * projectileSpeed), ForceMode.Force);
        }
    }
}
