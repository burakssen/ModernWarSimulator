using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA10 : BaseDefence
{
    private List<Tuple<GameObject, Vector3>> spawned;
    [SerializeField] public GameObject missile;
    public List<GameObject> missiles;
    public virtual void Start()
    {
        spawned = new List<Tuple<GameObject, Vector3>>();
        foreach (var point in spawnPoints)
        {
            GameObject m = Instantiate(missile, point.transform.position, missile.transform.localRotation);
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
        
        int time = 3;
        foreach (var missile in missiles)
        {
            if(!missile)
                continue;
            StartCoroutine(SetTargetToMissile(missile, time));
            time += 3;
        }
    }

    IEnumerator SetTargetToMissile(GameObject missile, float time)
    {
        yield return new WaitForSeconds(time);
        missile.GetComponent<MissileBase>().SetTarget(currentTarget);
    }

}
