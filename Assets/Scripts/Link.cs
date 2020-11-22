using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Link : MonoBehaviour
{
    private Connector input;
    private Connector output;

    private void OnEnable()
    {
        GetComponent<Image>().color = ColorPalette.SlateNormal;
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
        if (input == null)
        {
            input = to;
        }
        else if (output == null)
        {
            output = to;
        }
        else
        {
            return false;
        }
        CollisionEnabled = true;
        return true;
    }

    public bool CollisionEnabled
    {
        get { return GetComponent<Image>().raycastTarget; }
        set
        {
            GetComponent<Image>().raycastTarget = value;
            transform.GetChild(0).GetComponent<Image>().raycastTarget = value;
        }
    }
    public Connector Input { get { return input; } }
    public Connector Output { get { return output; } }
}
