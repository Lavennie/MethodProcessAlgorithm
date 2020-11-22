using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class Connector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private const float HIGHLIGHT_ADD = 0.7f;

    public ConnectorType type;

    private Color color;
    private Connector linked;

    private void OnEnable()
    {
        this.color = ColorPalette.LightColor;
        Dot.color = ColorPalette.LightColor;
        Line.color = ColorPalette.LightColor;

        ((RectTransform)Dot.transform).sizeDelta *= new Vector2(Database.CIRCUIT_DOT_RADIUS, Database.CIRCUIT_DOT_RADIUS);
        ((RectTransform)Line.transform).sizeDelta *= new Vector2(1, Database.CIRCUIT_WIDTH);
    }

    public Connector GetLinkedConnector()
    {
        return linked;
    }
    public Block GetLinked()
    {
        return linked.Block;
    }

    public void Highlight()
    {
        Dot.color = color + new Color(HIGHLIGHT_ADD, HIGHLIGHT_ADD, HIGHLIGHT_ADD);
        Line.color = color + new Color(HIGHLIGHT_ADD, HIGHLIGHT_ADD, HIGHLIGHT_ADD);
    }
    public void Unhighlight()
    {
        Dot.color = color;
        Line.color = color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Highlight();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Unhighlight();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            CodeLinks.IsDraggingLink(out ConnectorType linkType);
            if (linkType.HasFlag(type))
            {
                Link.InstantiateLink((type == ConnectorType.Input) ? this : null, (type == ConnectorType.Output) ? this : null).CollisionEnabled = false;
            }
            else
            {
                foreach (var link in CodeLinks.GetDraggedLinks())
                {
                    link.TryConnect(this);
                }
                CodeLinks.DropDraggedLink();
            }
        }
    }

    public Block Block { get { return transform.parent.parent.GetComponent<Block>(); } }
    public Image Dot { get { return transform.GetChild(0).GetComponent<Image>(); } }
    public Image Line { get { return transform.GetChild(1).GetComponent<Image>(); } }
}

[System.Flags]
public enum ConnectorType
{
    Input = 1,
    Output = 2,
}