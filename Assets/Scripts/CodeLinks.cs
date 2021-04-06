using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeLinks : MonoBehaviour
{
    public Link[] GetLinks()
    {
        Link[] links = new Link[LinkCount];
        for (int i = 0; i < LinkCount; i++)
        {
            links[i] = this[i];
        }
        return links;
    }

    public static List<Link> GetDraggedLinks()
    {
        List<Link> links = new List<Link>();
        for (int i = 0; i < Instance.LinkCount; i++)
        {
            // when dragging collision is disabled
            if (!Instance[i].CollisionEnabled)
            {
                links.Add(Instance[i]);
            }
        }
        return links;
    }
    public static void DropDraggedLink()
    {
        foreach (var link in GetDraggedLinks())
        {
            Destroy(link.gameObject);
        }
    }
    public static bool IsDraggingLink(out ConnectorType type)
    {
        for (int i = 0; i < Instance.LinkCount; i++)
        {
            // when dragging collision is disabled
            if (!Instance[i].CollisionEnabled)
            {
                type = (Instance[i].Input != null) ? ConnectorType.Input : ConnectorType.Output;
                return true;
            }
        }
        type = ConnectorType.Input | ConnectorType.Output;
        return false;
    }

    public static Link[] GetConnectorLinks(Connector connector)
    {
        List<Link> links = new List<Link>();
        foreach (var link in Instance.GetLinks())
        {
            if ((connector.Type == ConnectorType.Input && link.Input == connector) ||
                (connector.Type == ConnectorType.Output && link.Output == connector))
            {
                links.Add(link);
            }
        }
        return links.ToArray();
    }

    public int LinkCount { get { return transform.childCount; } }
    public Link this[int i] { get { return transform.GetChild(i).GetComponent<Link>(); } }

    public static CodeLinks Instance { get { return CodeWindow.Instance.Links; } }
}
