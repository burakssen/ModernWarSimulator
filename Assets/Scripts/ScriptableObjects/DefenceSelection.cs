using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DefenceSelectionCard", order = 3)]    
public class DefenceSelection : ScriptableObject
{
        [SerializeField] public float cost;
        [SerializeField] private string name;
        [SerializeField] public GameObject gameObject;
        [SerializeField] public float health;
        public enum DefenceType
        {
                Homing,
                Directional
        };

        [SerializeField] private float damage;

        [SerializeField] public DefenceType defenceType;
}
