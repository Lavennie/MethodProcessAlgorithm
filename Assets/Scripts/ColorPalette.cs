using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "ScriptableObjects/Color palette", order = 1)]
public class ColorPalette : ScriptableObject
{
    public enum Slot : uint
    {
        BackgroundNormal,
        BackgroundLight,
        BackgroundDark,
        ForegroundNormal,
        ForegroundDark,
        Light1,
        InverseLight,
        FadedLight1,
        Copper,
        Plastic,
        White,
        HalfOpacityForeground,
        GlowingLight,
        Light2,
        Light3,
        FadedLight2,
        FadedLight3,
        Light1Fixed,
        Light2Fixed,
        Light3Fixed,
        FadedLight1Fixed,
        FadedLight2Fixed,
        FadedLight3Fixed,
        LightError,
    }

    [SerializeField] private Color light1 = new Color(0.0f, 1.0f, 0.976f);
    [SerializeField] private Color light2 = new Color(0.0f, 1.0f, 0.976f);
    [SerializeField] private Color light3 = new Color(0.0f, 1.0f, 0.976f);
    [SerializeField] private Color lightError = new Color(1.0f, 0.0f, 0.0f);
    [SerializeField] private Color copper = new Color(0.961f, 0.608f, 0.259f);
    [SerializeField] private Color plastic = new Color(0.005f, 0.839f, 1.0f);
    [SerializeField] private Color bgLight = new Color(0.337f, 0.337f, 0.337f);
    [SerializeField] private Color bgNormal = new Color(0.118f, 0.118f, 0.118f);
    [SerializeField] private Color bgDark = new Color(0.0f, 0.0f, 0.0f);
    [SerializeField] private Color slateNormal = new Color(0.863f, 0.863f, 0.863f);
    [SerializeField] private Color slateDark = new Color(0.686f, 0.686f, 0.686f);

    [SerializeField] private RecolorObject<Material>[] materials;
    [SerializeField] private RecolorObject<Material>[] textOutlines;

    public int usedColor = 0;

    public Color GetColorFromSlot(Slot slot)
    {
        switch (slot)
        {
            case Slot.Light1:
                if (usedColor == 1)
                {
                    return light2;
                }
                else if (usedColor == 2)
                {
                    return light3;
                }
                else
                {
                    return light1;
                }
            case Slot.Light2:
                if (usedColor == 1)
                {
                    return light3;
                }
                else if (usedColor == 2)
                {
                    return light1;
                }
                else
                {
                    return light2;
                }
            case Slot.Light3:
                if (usedColor == 1)
                {
                    return light1;
                }
                else if (usedColor == 2)
                {
                    return light2;
                }
                else
                {
                    return light3;
                }
            case Slot.Light1Fixed:
                return light1;
            case Slot.Light2Fixed:
                return light2;
            case Slot.Light3Fixed:
                return light3;
            case Slot.InverseLight:
                Color l = GetColorFromSlot(Slot.Light1);
                return new Color(1.0f - l.r, 1.0f - l.g, 1.0f - l.b, l.a);
            case Slot.FadedLight1:
                Color l1 = GetColorFromSlot(Slot.Light1);
                return new Color(l1.r - 0.6f, l1.g - 0.6f, l1.b - 0.6f, l1.a);
            case Slot.FadedLight2:
                Color l2 = GetColorFromSlot(Slot.Light2);
                return new Color(l2.r - 0.6f, l2.g - 0.6f, l2.b - 0.6f, l2.a);
            case Slot.FadedLight3:
                Color l3 = GetColorFromSlot(Slot.Light3);
                return new Color(l3.r - 0.6f, l3.g - 0.6f, l3.b - 0.6f, l3.a);
            case Slot.FadedLight1Fixed:
                Color l1f = GetColorFromSlot(Slot.Light1Fixed);
                return new Color(l1f.r - 0.6f, l1f.g - 0.6f, l1f.b - 0.6f, l1f.a);
            case Slot.FadedLight2Fixed:
                Color l2f = GetColorFromSlot(Slot.Light2Fixed);
                return new Color(l2f.r - 0.6f, l2f.g - 0.6f, l2f.b - 0.6f, l2f.a);
            case Slot.FadedLight3Fixed:
                Color l3f = GetColorFromSlot(Slot.Light3Fixed);
                return new Color(l3f.r - 0.6f, l3f.g - 0.6f, l3f.b - 0.6f, l3f.a);
            case Slot.LightError:
                return lightError;
            case Slot.GlowingLight:
                return new Color(light1.r + 0.7f, light1.g + 0.7f, light1.b + 0.7f, light1.a);
            case Slot.Copper:
                return copper;
            case Slot.Plastic:
                return plastic;
            case Slot.BackgroundDark:
                return bgDark;
            case Slot.BackgroundNormal:
                return bgNormal;
            case Slot.BackgroundLight:
                return bgLight;
            case Slot.ForegroundDark:
                return slateDark;
            case Slot.ForegroundNormal:
                return slateNormal;
            case Slot.HalfOpacityForeground:
                return new Color(slateNormal.r, slateNormal.g, slateNormal.b, 0.5f);
            case Slot.White:
                return Color.white;
            default:
                return new Color(1.0f, 0.0f, 1.0f);
        }
    }
    public RecolorObject<Material> GetMaterial(int index) { return materials[index]; }
    public RecolorObject<Material> GetTextMaterial(int index) { return textOutlines[index]; }
    public int MaterialCount { get { return materials.Length; } }
    public int TextMaterialCount { get { return textOutlines.Length; } }

    [System.Serializable]
    public class RecolorObject<T>
    {
        public Slot colorSlot;
        public T obj;
    }
}
