using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CodeBlockMenuEntry : MonoBehaviour, IPointerClickHandler
{
    private BlockID id;

    private void OnEnable()
    {
        Text.color = ColorPalette.BgLight;
    }

    public static CodeBlockMenuEntry InstantiateMenuEntry(BlockID id, Transform parent)
    {
        BlockData data = Database.Instance[id];

        CodeBlockMenuEntry entry = Instantiate(Database.Instance.menuBlockEntryPrefab, parent);
        entry.id = id;
        entry.Text.text = data.Name;
        return entry;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Block.InstantiateBlock(id, eventData.position, null).SetDragging(true);
        }
    }

    public TextMeshProUGUI Text { get { return transform.GetChild(0).GetComponent<TextMeshProUGUI>(); } }
}
