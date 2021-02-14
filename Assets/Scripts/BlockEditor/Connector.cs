using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class Connector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private const float HIGHLIGHT_ADD = 0.7f;

    [SerializeField] private ConnectorType type;
    private ConnectorID dataType;

    private Color color;
    private Connector linked;

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
        ((RectTransform)Line.transform).sizeDelta = new Vector2(((RectTransform)Line.transform).sizeDelta.x, Database.CIRCUIT_WIDTH);
    }

    public void Init(ConnectorID dataType)
    {
        this.dataType = dataType;
        Dot.sprite = Database.GetSprite(dataType);
    }

    public static int GetConnectorIndex(Connector connector)
    {
        return connector.transform.GetSiblingIndex() / 2;
    }

    public Connector GetLinkedConnector()
    {
        return linked;
    }
    public Block GetLinked()
    {
        return linked.Block;
    }

    public static bool CanConnect(Connector input, Connector output)
    {
        if (input.dataType == output.dataType ||
            (input.dataType <= ConnectorID.FlowIfFalse && output.dataType <= ConnectorID.FlowIfFalse))
        {
            return true;
        }
        return false;
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