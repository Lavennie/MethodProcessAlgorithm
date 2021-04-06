using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CodeBlockMenuEntry : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private BlockID id;

    public static CodeBlockMenuEntry InstantiateMenuEntry(BlockID id, Transform parent)
    {
        BlockData data = Database.Instance[id];

        CodeBlockMenuEntry entry = Instantiate(Database.Instance.menuBlockEntryPrefab, parent);
        entry.id = id;
        entry.Text.text = data.Name;
        ((RectTransform)entry.transform).anchoredPosition = new Vector2(0, (parent.childCount - 1) * -((RectTransform)entry.transform).sizeDelta.y);
        return entry;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Block.InstantiateBlock(id, eventData.position, null).SetDragging(true, true);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Block.InstantiateBlock(id, eventData.position, null).SetDragging(true, true);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            CodeBlocks.StopDragging();
        }
    }

    public TextMeshProUGUI Text { get { return transform.GetChild(0).GetComponent<TextMeshProUGUI>(); } }
}
