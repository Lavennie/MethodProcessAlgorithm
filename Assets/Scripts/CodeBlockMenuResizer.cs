using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CodeBlockMenuResizer : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        BlockMenu.sizeDelta = new Vector2(Mathf.Clamp(eventData.position.x, 
            10, ((RectTransform)CodeWindow.Instance.transform).rect.width - 10), BlockMenu.sizeDelta.y);
    }

    public RectTransform BlockMenu { get { return (RectTransform)transform.parent; } }
}
