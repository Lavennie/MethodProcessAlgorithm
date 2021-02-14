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
        foreach (var graphic in graphics)
        {
            graphic.color = Database.GetColor(litColor);
        }
    }
    public void Unhighlight()
    {
        foreach (var graphic in graphics)
        {
            graphic.color = Database.GetColor(restColor);
        }
    }
}
