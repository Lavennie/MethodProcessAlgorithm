using UnityEngine;
using UnityEngine.UI;

public class ColoredElementHighlight : MonoBehaviour
{
    [SerializeField] private Graphic[] graphics;
    [SerializeField] private ColorPalette.Slot restColor;
    [SerializeField] private ColorPalette.Slot litColor;

    protected virtual void OnEnable()
    {
        Unhighlight();
    }
    public void Highlight()
    {
        SetCustomColor(litColor);
    }
    public void Unhighlight()
    {
        SetCustomColor(restColor);
    }
    public void SetCustomColor(ColorPalette.Slot slot)
    {
        foreach (var graphic in graphics)
        {
            graphic.color = Database.GetColor(slot);
        }
    }
}
