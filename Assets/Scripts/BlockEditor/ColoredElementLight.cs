using UnityEngine;

public class ColoredElementLight : MonoBehaviour
{
    [SerializeField] private ColorPalette.RecolorObject<Light>[] lights;

    protected virtual void OnEnable()
    {
        foreach (var light in lights)
        {
            light.obj.color = Database.GetColor(light.colorSlot);
        }
    }
}
