using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class CodeWindow : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private void OnEnable()
    {
        Background.color = ColorPalette.BgDark;

        Selector.GetChild(0).GetComponent<Image>().color = new Color(ColorPalette.SlateNormal.r, ColorPalette.SlateNormal.g, ColorPalette.SlateNormal.b, 0.5f);
        Selector.GetChild(1).GetComponent<Image>().color = new Color(ColorPalette.SlateNormal.r, ColorPalette.SlateNormal.g, ColorPalette.SlateNormal.b, 0.5f);
        Selector.GetChild(2).GetComponent<Image>().color = new Color(ColorPalette.SlateNormal.r, ColorPalette.SlateNormal.g, ColorPalette.SlateNormal.b, 0.5f);
        Selector.GetChild(3).GetComponent<Image>().color = new Color(ColorPalette.SlateNormal.r, ColorPalette.SlateNormal.g, ColorPalette.SlateNormal.b, 0.5f);
        Selector.GetChild(4).GetComponent<Image>().color = ColorPalette.SlateNormal;
        Selector.GetChild(5).GetComponent<Image>().color = ColorPalette.SlateNormal;
        Selector.GetChild(6).GetComponent<Image>().color = ColorPalette.SlateNormal;
        Selector.GetChild(7).GetComponent<Image>().color = ColorPalette.SlateNormal;
        Selector.GetChild(8).GetComponent<Image>().color = ColorPalette.SlateNormal;
        Selector.GetChild(9).GetComponent<Image>().color = ColorPalette.SlateNormal;
        Selector.GetChild(10).GetComponent<Image>().color = ColorPalette.SlateNormal;
        Selector.GetChild(11).GetComponent<Image>().color = ColorPalette.SlateNormal;
        Selector.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            CodeLinks.DropDraggedLink();
            CodeBlocks.DeselectAll();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Selector.gameObject.SetActive(true);
        Selector.transform.position = eventData.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        ((RectTransform)Selector.transform).sizeDelta = (eventData.position - (Vector2)Selector.transform.position) * new Vector2(1, -1);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Selector.gameObject.SetActive(false);
        CodeBlocks.DeselectAll();
        foreach (var block in Blocks.GetBlocks())
        {
            Rect r1 = new Rect((Vector2)block.transform.position + ((RectTransform)block.transform).rect.min, ((RectTransform)block.transform).rect.size);
            Rect r2 = new Rect(Mathf.Min(Selector.position.x, eventData.position.x), Mathf.Min(Selector.position.y, eventData.position.y),
                Mathf.Abs(Selector.position.x - eventData.position.x), Mathf.Abs(Selector.position.y - eventData.position.y));
            if (!(r1.min.x > r2.max.x || r1.min.y > r2.max.y || r2.min.x > r1.max.x || r2.min.y > r1.max.y))
            {
                block.SetSelected(true);
            }
            else
            {
            }
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var block in Blocks.GetBlocks())
        {
            Rect r1 = new Rect((Vector2)block.transform.position + ((RectTransform)block.transform).rect.min, ((RectTransform)block.transform).rect.size);
            Gizmos.DrawLine(r1.min, r1.max);
            Gizmos.DrawSphere(r1.min, 10);
        }
    }


    public Image Background { get { return GetComponent<Image>(); } }
    public CodeBlockMenu Menu { get { return transform.GetChild(0).GetComponent<CodeBlockMenu>(); } }
    public CodeLinks Links { get { return transform.GetChild(1).GetComponent<CodeLinks>(); } }
    public CodeBlocks Blocks { get { return transform.GetChild(2).GetComponent<CodeBlocks>(); } }
    public Transform Selector { get { return transform.GetChild(3); } }

    public static CodeWindow Instance { get { return Database.Instance.codeWindow; } }
}
