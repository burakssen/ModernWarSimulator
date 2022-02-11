using UnityEngine;
using UnityEngine.EventSystems;
public class DraggableComponent : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    
    private GameObject draggingObject;
    private Transform initialParent;
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle((RectTransform)draggingObject.transform, eventData.position,
                eventData.pressEventCamera, out var globalMousePosition))
        {
            draggingObject.transform.position = globalMousePosition;
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        DestroyImmediate(draggingObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Transform t = FindCanvas(gameObject).transform;
        GameObject obj = Instantiate(gameObject, t);
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, t.position.z - 1);
        draggingObject = obj;
    }
    
    public static GameObject FindCanvas(GameObject childObject)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.GetComponent<Canvas>())
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }
    
}
