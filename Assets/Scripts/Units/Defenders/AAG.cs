using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAG : BaseDefence
{
    [SerializeField] private bool shoot = false;
    [SerializeField] private GameObject bullet;
    [Range(0, 1)] [SerializeField] private float projectileSpeed;
    private float timeStamp;

    private Vector3 direction;
    private IEnumerator enumerator;
    private TiltRotate tiltRotate;

    private void Start()
    {
        tiltRotate = GetComponent<TiltRotate>();
    }

    public void Update()
    {   
        if (Global.gameState != Global.GameState.play)
            return;


        GetComponent<SphereCollider>().enabled = true;

        if (!shoot || !currentTarget)
            return;
        
        if (timeStamp <= Time.time)
        {
            StartCoroutine(Shoot());
            timeStamp = Time.time + 4 * 0.2f;
        }

    }

    public void FixedUpdate()
    {
        direction = tiltRotate.GetDirection();
    }

    public override void SetTargets(GameObject target)
    {
        base.SetTargets(target);
        tiltRotate.SetCurrentTarget(target);
        shoot = true;
    }

    public IEnumerator Shoot()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            yield return new WaitForSeconds(0.2f);

            var b = Instantiate(bullet, spawnPoint.transform.position, Quaternion.identity);
            b.GetComponent<Rigidbody>().AddForce(direction * (direction.magnitude * projectileSpeed), ForceMode.Force);
        }
    }
}