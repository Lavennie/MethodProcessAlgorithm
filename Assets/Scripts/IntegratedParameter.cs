using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IntegratedParameter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image background;
    public Image circuitLineLeft;
    public Image circuitLineBottom;
    public Image circuitLineRight;

    private float closeHeight;
    private bool open = false;

    private void Awake()
    {
        closeHeight = (((RectTransform)background.transform).rect.height - BlockDatabase.CIRCUIT_WIDTH) / 2.0f;
    }

    private void Update()
    {
        float target = (open) ? 0 : closeHeight;
        float openCloseSpeed = Time.deltaTime * 100.0f;
        ((RectTransform)background.transform).offsetMin = Vector2.MoveTowards(
            ((RectTransform)background.transform).offsetMin,
            new Vector2(((RectTransform)background.transform).offsetMin.x,
            target), openCloseSpeed);
        ((RectTransform)background.transform).offsetMax = Vector2.MoveTowards(
            ((RectTransform)background.transform).offsetMax,
            new Vector2(((RectTransform)background.transform).offsetMax.x,
            -target), openCloseSpeed);
    }

    public void ApplyColor(Color lightColor, Color darkColor)
    {
        background.color = darkColor;
        circuitLineLeft.color = lightColor;
        circuitLineBottom.color = lightColor;
        circuitLineRight.color = lightColor;

        ((RectTransform)circuitLineLeft.transform).sizeDelta *= new Vector2(BlockDatabase.CIRCUIT_WIDTH, 1);
        ((RectTransform)circuitLineBottom.transform).sizeDelta *= new Vector2(1, BlockDatabase.CIRCUIT_WIDTH);
        ((RectTransform)circuitLineRight.transform).sizeDelta *= new Vector2(BlockDatabase.CIRCUIT_WIDTH, 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        open = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        open = false;
    }
}
