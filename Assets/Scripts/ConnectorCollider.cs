using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectorCollider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Connector connector;

    private void Awake()
    {
        connector = GetComponentInParent<Connector>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        connector.Highlight();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        connector.Unhighlight();
    }
}
