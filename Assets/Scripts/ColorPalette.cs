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
        Light,
        InverseLight,
        FadedLight,
        Copper,
        Plastic,
        White,
        HalfOpacityForeground,
        GlowingLight,
    }

    [SerializeField] private Color light = new Color(0.0f, 1.0f, 0.976f);
    [SerializeField] private Color copper = new Color(0.961f, 0.608f, 0.259f);
    [SerializeField] private Color plastic = new Color(0.005f, 0.839f, 1.0f);
    [SerializeField] private Color bgLight = new Color(0.337f, 0.337f, 0.337f);
    [SerializeField] private Color bgNormal = new Color(0.118f, 0.118f, 0.118f);
    [SerializeField] private Color bgDark = new Color(0.0f, 0.0f, 0.0f);
    [SerializeField] private Color slateNormal = new Color(0.863f, 0.863f, 0.863f);
    [SerializeField] private Color slateDark = new Color(0.686f, 0.686f, 0.686f);

    [SerializeField] private RecolorObject<Material>[] materials;

    public Color GetColorFromSlot(Slot slot)
    {
        switch (slot)
        {
            case Slot.Light:
                return light;
            case Slot.InverseLight:
                return new Color(1.0f - light.r, 1.0f - light.g, 1.0f - light.b, light.a);
            case Slot.FadedLight:
                return new Color(light.r - 0.3f, light.g - 0.3f, light.b - 0.3f, light.a);
            case Slot.GlowingLight:
                return new Color(light.r + 0.7f, light.g + 0.7f, light.b + 0.7f, light.a);
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
