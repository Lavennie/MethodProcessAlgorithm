using UnityEngine;
using UnityEngine.EventSystems;

public class BlockDrag : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public Vector2 offset = Vector2.zero;
    [HideInInspector]
    public bool dragging = false;

    private void Update()
    {
        if (!dragging)
        {
            return;
        }
        transform.position = (Vector2)Input.mousePosition + offset;
    }

    /// <summary>
    /// When block is entered from menu, it's dropped by clicking not by stopping drag
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GetComponent<Block>().SetDragging(false);
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!Block.IsSelected)
            {
                CodeBlocks.DeselectAll();
                Block.SetSelected(true);
            }
            CodeBlocks.BeginDragingSelected();
        }
    }
    public void OnDrag(PointerEventData eventData) { }
    public void OnEndDrag(PointerEventData eventData)
    {
        CodeBlocks.StopDragging();
    }

    public Block Block { get { return GetComponent<Block>(); } }
}
