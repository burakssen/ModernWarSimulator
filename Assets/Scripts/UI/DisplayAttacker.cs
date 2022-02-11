using UnityEngine;
using UnityEngine.UI;

public class DisplayAttacker : MonoBehaviour
{
    public AttackerSelection[] attackerSelections;
    public GameObject uiObject;
    public GameObject uiSeperator;
    void Start()
    {
        foreach (var attackerSelection in attackerSelections)
        {
            Instantiate(uiSeperator, transform, false);
            GameObject uiObj = Instantiate(uiObject, transform, false);
            Transform uiObjTransform = uiObject.transform;
            for (int i = 0; i < uiObjTransform.childCount; i++)
            {
                Transform child = uiObjTransform.GetChild(i);
                if (child.tag == "Image")
                {
                    child.gameObject.GetComponent<Image>().sprite = attackerSelection.image.sprite;
                }

                if (child.tag == "Label")
                {
                    child.gameObject.GetComponent<Text>().text = attackerSelection.attackerName;
                }
            }
        }
    }
}
