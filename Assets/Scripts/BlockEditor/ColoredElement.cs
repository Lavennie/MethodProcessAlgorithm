using UnityEngine;
using UnityEngine.UI;

public class ColoredElement : MonoBehaviour
{
    [SerializeField] private ColorPalette.RecolorObject<Graphic>[] graphics;

    protected virtual void OnEnable()
    {
        foreach (var graphic in graphics)
        {
            graphic.obj.color = Database.GetColor(graphic.colorSlot);
        }
    }
}
