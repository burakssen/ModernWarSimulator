using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA10 : BaseDefence
{
    private bool spawned = false;
    [SerializeField] public GameObject missile;
    public List<GameObject> missiles;
   
    public void Update()
    {
        if (Global.gameState == Global.GameState.play && !spawned)
        {
            foreach (var point in spawnPoints)
            {
                GameObject m = Instantiate(missile, point.transform.position, missile.transform.localRotation);
                m.transform.SetParent(transform);
                missiles.Add(m);
            }

            spawned = true;
        }
        
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
        MissileBase missileBase = missile.GetComponent<MissileBase>();
        missileBase.SetTarget(currentTarget);
        missileBase.enable = true;
    }

}
