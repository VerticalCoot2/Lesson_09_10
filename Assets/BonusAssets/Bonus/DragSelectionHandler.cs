using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DragSelectionHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image SelectionBoxImage;
    Vector2 StartPos;
    Rect SelectionRect;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        {
            MySelectable.DeselectAll(new BaseEventData(EventSystem.current));
        }
        SelectionBoxImage.gameObject.SetActive(true);
        StartPos = eventData.position;
        SelectionRect = new Rect();
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        // X axis
        if (eventData.position.x < StartPos.x)
        {
            SelectionRect.xMin = eventData.position.x;
            SelectionRect.xMax = StartPos.x;
        }
        else
        {
            SelectionRect.xMin = StartPos.x;
            SelectionRect.xMax = eventData.position.x;
        }
        // Y axis 
        if (eventData.position.y < StartPos.y)
        {
            SelectionRect.yMin = eventData.position.y;
            SelectionRect.yMax = StartPos.y;
        }
        else
        {
            SelectionRect.yMin = StartPos.y;
            SelectionRect.yMax = eventData.position.y;
        }

        SelectionBoxImage.rectTransform.offsetMin = SelectionRect.min;
        SelectionBoxImage.rectTransform.offsetMax = SelectionRect.max;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SelectionBoxImage.gameObject.SetActive(false);
        foreach (MySelectable selected in MySelectable.allMySelectables)
        {
            if (SelectionRect.Contains(Camera.main.WorldToScreenPoint(selected.transform.position)))
            {
                selected.OnSelect(eventData);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        float myDistance = 0;
        foreach (RaycastResult item in results)
        {
            if (item.gameObject == gameObject)
            {
                myDistance = item.distance;
                break;
            }
        }
        GameObject nextObject = null;
        float maxDistance = Mathf.Infinity;

        foreach (RaycastResult item in results)
        {
            if (item.distance > myDistance && item.distance < maxDistance)
            {
                nextObject = item.gameObject;
                maxDistance = item.distance;
            }
        }
        if (nextObject)
        {
            ExecuteEvents.Execute<IPointerClickHandler>(nextObject, eventData, (x, y) => { x.OnPointerClick((PointerEventData)y); });
        }
    }
}
