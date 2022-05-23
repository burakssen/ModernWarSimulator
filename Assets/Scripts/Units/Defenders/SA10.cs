using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA10 : BaseDefence
{
    private List<Tuple<GameObject, Vector3>> spawned;
    [SerializeField] public GameObject missle;
    public List<GameObject> missiles;
    public virtual void Start()
    {
       
        spawned = new List<Tuple<GameObject, Vector3>>();
        foreach (var point in spawnPoints)
        {
            GameObject m = Instantiate(missle, point.transform.position, missle.transform.localRotation);
            missiles.Add(m);
            spawned.Add(new Tuple<GameObject, Vector3>(m, m.transform.position));
        }
    }
    
    public override void SetTargets()
    {
        if (targetPool.Count <= 0)
            return;
        
        int time = 3;
        foreach (var missile in missiles)
        {
            StartCoroutine(SetTargetToMissile(missile, time));
            time += 3;
        }
    }
    
    IEnumerator SetTargetToMissile(GameObject missile, float time)
    {
        yield return new WaitForSeconds(time);
        if (targetPool.Count > 0)
        {
            GameObject target = targetPool[0];
            missile.GetComponent<Missile>().SetTarget(target);
        }
    }

}
