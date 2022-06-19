using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F35 : AttackerBase
{
    private List<Tuple<GameObject, Vector3>> spawned;

    public override void Start()
    {
        base.Start();
        spawned = new List<Tuple<GameObject, Vector3>>();
        foreach (var point in spawnPoints)
        {
            var m = Instantiate(missile, point.transform.position, missile.transform.localRotation);
            m.transform.parent = point.transform;
            missiles.Add(m);
            spawned.Add(new Tuple<GameObject, Vector3>(m, m.transform.position));
        }
    }

    public override void SetTargets(GameObject target)
    {
        base.SetTargets(target);

        var time = 1f;
        if (missiles.Count == 0)
            foreach (var point in spawnPoints)
            {
                var m = Instantiate(missile, point.transform.position, missile.transform.localRotation);
                m.transform.parent = point.transform;
                missiles.Add(m);
                spawned.Add(new Tuple<GameObject, Vector3>(m, m.transform.position));
            }

        foreach (var missile in missiles)
        {
            if (!missile)
                continue;

            StartCoroutine(SetTargetToMissile(missile, time));
            time += 1f;
        }

        Invoke(nameof(MissileClear), time);
    }

    private void MissileClear()
    {
        missiles.Clear();
    }

    private IEnumerator SetTargetToMissile(GameObject missile, float time)
    {
        yield return new WaitForSeconds(time);
        var missileBase = missile.GetComponent<MissileBase>();
        missileBase.SetTarget(currentTarget);
        missileBase.enable = true;
        missile.transform.parent = null;
    }
}