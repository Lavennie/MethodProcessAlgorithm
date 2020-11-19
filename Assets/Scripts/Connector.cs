using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Connector : MonoBehaviour
{
    private const float HIGHLIGHT_ADD = 0.7f;

    public Image circuitDot;
    public Image circuitLine;

    private Color color;

    public void ApplyColor(Color color)
    {
        this.color = color;
        circuitDot.color = color;
        circuitLine.color = color;

        ((RectTransform)circuitDot.transform).sizeDelta *= new Vector2(BlockDatabase.CIRCUIT_DOT_RADIUS, BlockDatabase.CIRCUIT_DOT_RADIUS);
        ((RectTransform)circuitLine.transform).sizeDelta *= new Vector2(1, BlockDatabase.CIRCUIT_WIDTH);
    }

    public void Highlight()
    {
        circuitDot.color = color + new Color(HIGHLIGHT_ADD, HIGHLIGHT_ADD, HIGHLIGHT_ADD);
        circuitLine.color = color + new Color(HIGHLIGHT_ADD, HIGHLIGHT_ADD, HIGHLIGHT_ADD);
    }
    public void Unhighlight()
    {
        circuitDot.color = color;
        circuitLine.color = color;
    }
}
