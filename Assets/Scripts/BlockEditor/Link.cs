using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Link : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Connector input;
    private Connector output;

    private void OnEnable()
    {
        Unhighlight();
    }

    private void Update()
    {
        Vector2 p1;
        Vector2 p2;
        if (input != null && output != null)
        {
            p1 = input.Line.transform.position;
            p2 = output.Line.transform.position;
        }
        else if (input == null && output != null)
        {
            p1 = UnityEngine.Input.mousePosition;
            p2 = output.Line.transform.position;
        }
        else if (input != null && output == null)
        {
            p1 = input.Line.transform.position;
            p2 = UnityEngine.Input.mousePosition;
        }
        else
        {
            return;
        }
        transform.position = (p1 + p2) / 2.0f;
        ((RectTransform)transform).sizeDelta = new Vector2(Vector2.Distance(p1, p2), Database.CIRCUIT_WIDTH);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * (180.0f / Mathf.PI));
    }

    public static Link InstantiateLink(Connector input, Connector output)
    {
        Link link = Instantiate(Database.Instance.linkPrefab, CodeWindow.Instance.Links.transform);
        link.input = input;
        link.output = output;

        return link;
    }

    public bool TryConnect(Connector to)
    {
        if (input == null && output != null)
        {
            input = to;
        }
        else if (input != null && output == null)
        {
            output = to;
        }
        else
        {
            return false;
        }

        if (Connector.CanConnect(input, output))
        {
            CollisionEnabled = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Highlight()
    {
        GetComponent<ColoredElementHighlight>().Highlight();
    }
    public void Unhighlight()
    {
        GetComponent<ColoredElementHighlight>().Unhighlight();
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
        if (Vector2.Distance(eventData.position, input.transform.position) < Vector2.Distance(eventData.position, output.transform.position))
        {
            input = null;
        }
        else
        {
            output = null;
        }
        CollisionEnabled = false;
    }

    public bool CollisionEnabled
    {
        get { return GetComponent<Image>().raycastTarget; }
        set
        {
            if (value)
            {
                Highlight();
            }
            // set image and collider raycasting. When dragging around collision is disabled
            GetComponent<Image>().raycastTarget = value;
            transform.GetChild(0).GetComponent<Image>().raycastTarget = value;
        }
    }
    public Connector Input { get { return input; } }
    public Connector Output { get { return output; } }
}
