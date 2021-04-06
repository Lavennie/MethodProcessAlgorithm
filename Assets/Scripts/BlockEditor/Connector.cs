using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class Connector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private const float HIGHLIGHT_ADD = 0.7f;

    [SerializeField] private ConnectorType type;
    private ConnectorID dataType;

    private Color color;

    private void OnEnable()
    {
        ((RectTransform)Dot.transform).sizeDelta = new Vector2(Database.CIRCUIT_DOT_RADIUS, Database.CIRCUIT_DOT_RADIUS);
        if (type == ConnectorType.Input)
        {
            ((RectTransform)Line.transform).offsetMax = new Vector2(-Database.CIRCUIT_DOT_RADIUS, ((RectTransform)Line.transform).anchorMin.y);
        }
        else
        {
            ((RectTransform)Line.transform).offsetMin = new Vector2(Database.CIRCUIT_DOT_RADIUS, ((RectTransform)Line.transform).anchorMin.y);

        }
        ((RectTransform)Line.transform).sizeDelta = new Vector2(((RectTransform)Line.transform).sizeDelta.x - 3, Database.CIRCUIT_WIDTH);
    }

    public void Init(ConnectorID dataType)
    {
        this.dataType = dataType;
        Dot.sprite = Database.GetSprite(dataType);
    }

    public static int GetConnectorIndex(Connector connector)
    {
        switch (connector.type)
        {
            case ConnectorType.Input:
                return connector.transform.GetSiblingIndex() / 2;
            case ConnectorType.Output:
                return connector.transform.GetSiblingIndex();
            default:
                return -1;
        }
    }

    public static bool CanConnect(Connector input, Connector output)
    {
        // cannot connect connectors of same block
        if (input.Block == output.Block)
        {
            return false;
        }

        // type of connectors matches
        if (ConnectorTypesMatch(input.dataType, output.dataType))
        {
            // only one link can be connected into input
            if (input.dataType > ConnectorID.FlowIfFalse)
            {
                // 1 because the pending link is also in array
                if (CodeLinks.GetConnectorLinks(input).Length > 1)
                {
                    return false;
                }
            }
            // flow inputs can accept multiple connection, but they must not be duplicate
            else
            {
                int connectedCount = 0;
                foreach (var link in CodeLinks.GetConnectorLinks(input))
                {
                    if (link.Output == output)
                    {
                        connectedCount++;
                    }
                }
                // 1 because the pending link is also in array
                if (connectedCount > 1)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
    public static bool ConnectorTypesMatch(ConnectorID c1, ConnectorID c2)
    {
        if (c1 == c2 ||
               (c1 == ConnectorID.Vector2 && c2 == ConnectorID.Direction2) ||
               (c1 == ConnectorID.Direction2 && c2 == ConnectorID.Vector2) ||
               (c1 <= ConnectorID.FlowIfFalse && c2 <= ConnectorID.FlowIfFalse))
        {
            return true;
        }
        return false;
    }

    public Connector GetConnected()
    {
        foreach (var link in CodeWindow.Instance.Links.GetLinks())
        {
            if (Type == ConnectorType.Input && link.Input == this)
            {
                return link.Output;
            }
            else if (Type == ConnectorType.Output && link.Output == this)
            {
                return link.Input;
            }
        }
        return null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<ColoredElementHighlight>().Highlight();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<ColoredElementHighlight>().Unhighlight();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (CodeLinks.IsDraggingLink(out ConnectorType linkType))
            {
                foreach (var link in CodeLinks.GetDraggedLinks())
                {
                    link.TryConnect(this);
                }
                CodeLinks.DropDraggedLink();
            }
            else
            {
                Link.InstantiateLink((type == ConnectorType.Input) ? this : null, (type == ConnectorType.Output) ? this : null).CollisionEnabled = false;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Link.InstantiateLink((type == ConnectorType.Input) ? this : null, (type == ConnectorType.Output) ? this : null).CollisionEnabled = false;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        // while dragging connector pick another one
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // find connector that mouse is hovering
            Connector connector = null;
            foreach (var hovering in eventData.hovered)
            {
                connector = hovering.GetComponent<Connector>();
                if (connector != null)
                {
                    break;
                }
            }
            if (connector != null)
            {
                // do not drag twice from same connector
                foreach (var link in CodeLinks.GetDraggedLinks())
                {
                    if (link.Input == connector || link.Output == connector)
                    {
                        return;
                    }
                }
                if (connector != this && ConnectorTypesMatch(connector.DataType, DataType) && connector.type == type)
                {
                    Link.InstantiateLink((connector.type == ConnectorType.Input) ? connector : null, (connector.type == ConnectorType.Output) ? connector : null).CollisionEnabled = false;
                }
            }

        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // find connector that mouse is hovering
            Connector connector = null;
            foreach (var hovering in eventData.hovered)
            {
                connector = hovering.GetComponent<Connector>();
                if (connector != null)
                {
                    break;
                }
            }
            // if such connector exists and it is not self try connecting
            if (connector != null && connector != this)
            {
                foreach (var link in CodeLinks.GetDraggedLinks())
                {
                    link.TryConnect(connector);
                }
            }
            CodeLinks.DropDraggedLink();
        }
    }

    public Block Block { get { return transform.parent.parent.GetComponent<Block>(); } }
    public Image Dot { get { return transform.GetChild(0).GetComponent<Image>(); } }
    public Image Line { get { return transform.GetChild(1).GetComponent<Image>(); } }
    public ConnectorType Type { get { return type; } }
    public ConnectorID DataType { get { return dataType; } }
}

[System.Flags]
public enum ConnectorType
{
    Input = 1,
    Output = 2,
}