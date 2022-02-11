using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AttackerSelectionCard", order = 1)]
public class AttackerSelection : ScriptableObject
{
    public Image image;
    public enum AttackerType
    {
        Directional,
        Homing
    };
    
    public string attackerName;
    public int damage;
    
    public AttackerType attackerType;

    public int reloadTime;
    public int range;
}