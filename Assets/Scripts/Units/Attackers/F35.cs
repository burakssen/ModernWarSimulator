using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F35 : AttackerBase
{
    private List<Tuple<GameObject, Vector3>> spawned;
    [SerializeField] public GameObject missile;
    public List<GameObject> missiles;
    public override void Start()
    {
        base.Start();
        spawned = new List<Tuple<GameObject, Vector3>>();
        foreach (var point in spawnPoints)
        {
            GameObject m = Instantiate(missile, point.transform.position, missile.transform.localRotation);
            m.transform.parent = point.transform;
            missiles.Add(m);
            spawned.Add(new Tuple<GameObject, Vector3>(m, m.transform.position));
        }
    }

    public void Update()
    {
        if (!currentTarget)
            return;
        
        if(setTargets)
        {
            SetTargets(currentTarget);
            setTargets = false;
        }
    }
    
    public override void SetTargets(GameObject target)
    {
        base.SetTargets(target);
        
        float time = 0.5f;
        foreach (var missile in missiles)
        {
            if(!missile)
                continue;
            
            StartCoroutine(SetTargetToMissile(missile, time));
            time += 0.5f;
        }
    }

    IEnumerator SetTargetToMissile(GameObject missile, float time)
    {
        yield return new WaitForSeconds(time);
        missile.GetComponent<MissileBase>().SetTarget(currentTarget);
        missile.GetComponent<MissileBase>().enable = true;
    }
}