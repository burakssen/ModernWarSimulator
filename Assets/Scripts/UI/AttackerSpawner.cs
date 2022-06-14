using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackerSpawner : MonoBehaviour
{
    private List<AttackerSelectionSerializer> attackerSelectionSerializers;
    [SerializeField] private List<AttackerSelection> attackerSelections;
    [SerializeField] private List<MissileSelection> missileSelections;
    public bool spawn = false;
    private MapLoader mapLoader;
    public List<Tuple<AttackerSelection, float>> GameObjectstoBeInstantiated;
    void Start()
    {
        GameObjectstoBeInstantiated = new List<Tuple<AttackerSelection, float>>();
        mapLoader = FindObjectOfType<MapLoader>();
        attackerSelectionSerializers = new List<AttackerSelectionSerializer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawn)
            return;
        
        attackerSelectionSerializers = mapLoader.GetAttackerSelectionSerializers();

        if (attackerSelectionSerializers == null)
            return;

        int time = 1;
        foreach (var attacker in attackerSelectionSerializers)
        {
            foreach(var selection in attackerSelections)
            {
                for (int i = 0; i < attacker.numberOfAttacker; i++)
                {
                    if (attacker.attackerName == selection.attackerName)
                    {
                        selection.attackerType = attacker.attackerType;
                        AttackerSelection attackerSelection = ScriptableObject.CreateInstance<AttackerSelection>();
                        attackerSelection.damage = selection.damage;
                        attackerSelection.health = selection.health;
                        attackerSelection.image = selection.image;
                        attackerSelection.attackerName = selection.attackerName;
                        attackerSelection.attackerType = selection.attackerType;
                        attackerSelection.gameObject = selection.gameObject;
                        GameObjectstoBeInstantiated.Add(new Tuple<AttackerSelection, float>(attackerSelection, time));
                        time++;
                    }
                }
            }
        }

        foreach (var objs in GameObjectstoBeInstantiated)
        {
            StartCoroutine(Spawn(objs.Item1, Random.Range(0f, 2f)));
        }
        
        spawn = false;
    }

    IEnumerator Spawn(AttackerSelection selection, float delay)
    {
        yield return new WaitForSeconds(delay * 2f);

        GameObject obj = Instantiate(selection.gameObject, transform, true);
        obj.transform.position = transform.position;
        obj.GetComponent<MoveOnPaths>().SetPathPosition(transform);
        AttackerBase attackerBase = obj.GetComponent<AttackerBase>();
        attackerBase.SetValues(selection.damage, selection.health);

        foreach (var missile in missileSelections)
        {
            if (selection.attackerType == AttackerSelection.AttackerType.FreeFall &&
                (missile.attackerType == MissileSelection.AttackerType.FreeFall))
            {
                attackerBase.SetMissile(missile.missile);
            }
            else if (selection.attackerType == AttackerSelection.AttackerType.Directional &&
                     (missile.attackerType == MissileSelection.AttackerType.Directional))
            {
                attackerBase.SetMissile(missile.missile);
            }
        }

    }
}
