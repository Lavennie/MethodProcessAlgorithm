using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverColorElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ColoredElementHighlight coloredElement;

    private void OnEnable()
    {
        coloredElement.Unhighlight();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        coloredElement.Highlight();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        coloredElement.Unhighlight();
    }
}
