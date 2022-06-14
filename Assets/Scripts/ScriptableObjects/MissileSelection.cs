using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MissileSelectionCar", order = 3)]
public class MissileSelection : ScriptableObject
{
        public enum AttackerType
        {
                FreeFall,
                Directional
        };

        [SerializeField] public GameObject missile;
        [SerializeField] public float damage;

        [SerializeField] public AttackerType attackerType;
}
