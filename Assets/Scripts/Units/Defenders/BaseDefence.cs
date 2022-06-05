using UnityEngine;

public abstract class BaseDefence : MonoBehaviour
{
    [SerializeField] public GameObject[] spawnPoints;
    [SerializeField] public bool setTargets = false;

    [SerializeField] public GameObject currentTarget;
    public virtual void SetTargets(GameObject target)
    {
        currentTarget = target;
    }
}
