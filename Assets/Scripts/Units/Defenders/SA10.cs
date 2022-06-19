using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA10 : BaseDefence
{
    private bool spawned = false;
    [SerializeField] public GameObject missile;
    public List<GameObject> missiles;
    private bool setTargets = false;
    private float timeStamp;

    private void FixedUpdate()
    {
        if (Global.gameState != Global.GameState.play)
            return;
        
        var missileNotExist = true;

        if (timeStamp <= Time.time)
        {
            if (missiles.Count > 0)
            {
                foreach (var missile in missiles) missileNotExist = missileNotExist && missile;

                setTargets = false;
                if (missileNotExist == false) missiles.Clear();
            }
            else
            {
                missileNotExist = false;
            }


            if (!missileNotExist)
            {
                GetComponent<SphereCollider>().enabled = true;
                var index = 0;
                foreach (var point in spawnPoints)
                {
                    var m = Instantiate(missile, point.transform.position, transform.localRotation);
                    m.transform.SetParent(spawnPoints[index].transform);
                    missiles.Add(m);
                    index++;
                }

                setTargets = true;
            }

            if (setTargets)
                Set();
            
            timeStamp = Time.time + missiles.Count * 3 + 5f;
        }
    }

    public void Set()
    {
        

        var time = 3;
        foreach (var missile in missiles)
        {
            if (!missile)
                continue;
            StartCoroutine(SetTargetToMissile(missile, time));
            time += 3;
        }
    }


    public override void SetTargets(GameObject target)
    {
        base.SetTargets(target);
    }

    private IEnumerator SetTargetToMissile(GameObject missile, float time)
    {
        yield return new WaitForSeconds(time);
        var missileBase = missile.GetComponent<MissileBase>();
        missileBase.SetTarget(currentTarget);
    }
}