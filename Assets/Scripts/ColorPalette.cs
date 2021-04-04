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
        FadedLight,
        Copper,
        Plastic,
        White,
        HalfOpacityForeground,
        GlowingLight,
        Light2,
        Light3
    }

    [SerializeField] private Color light1 = new Color(0.0f, 1.0f, 0.976f);
    [SerializeField] private Color light2 = new Color(0.0f, 1.0f, 0.976f);
    [SerializeField] private Color light3 = new Color(0.0f, 1.0f, 0.976f);
    [SerializeField] private Color copper = new Color(0.961f, 0.608f, 0.259f);
    [SerializeField] private Color plastic = new Color(0.005f, 0.839f, 1.0f);
    [SerializeField] private Color bgLight = new Color(0.337f, 0.337f, 0.337f);
    [SerializeField] private Color bgNormal = new Color(0.118f, 0.118f, 0.118f);
    [SerializeField] private Color bgDark = new Color(0.0f, 0.0f, 0.0f);
    [SerializeField] private Color slateNormal = new Color(0.863f, 0.863f, 0.863f);
    [SerializeField] private Color slateDark = new Color(0.686f, 0.686f, 0.686f);

    [SerializeField] private RecolorObject<Material>[] materials;

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
            case Slot.InverseLight:
                return new Color(1.0f - light1.r, 1.0f - light1.g, 1.0f - light1.b, light1.a);
            case Slot.FadedLight:
                return new Color(light1.r - 0.3f, light1.g - 0.3f, light1.b - 0.3f, light1.a);
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
    public int MaterialCount { get { return materials.Length; } }

    [System.Serializable]
    public class RecolorObject<T>
    {
        public Slot colorSlot;
        public T obj;
    }
}
