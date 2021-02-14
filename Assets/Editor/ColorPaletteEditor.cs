using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColorPalette))]
public class ColorPaletteEditor : Editor
{
    private int segmentCount = 3;
    private float ambientPercent = 0.2f;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ColorPalette cp = (ColorPalette)target;

        EditorGUILayout.Separator();

        segmentCount = EditorGUILayout.IntSlider("Segments", segmentCount, 1, 10);
        ambientPercent = EditorGUILayout.Slider("Ambient part", ambientPercent, 0.0f, 1.0f);

        if (GUILayout.Button("Refresh"))
        {
            for (int i = 0; i < cp.MaterialCount; i++)
            {
                cp.GetMaterial(i).obj.color = cp.GetColorFromSlot(cp.GetMaterial(i).colorSlot);

                if (cp.GetMaterial(i).obj.shader.name.StartsWith("Simple Toon"))
                {
                    cp.GetMaterial(i).obj.SetFloat("_Steps", segmentCount);
                    if(cp.GetMaterial(i).obj.name != "Circuit")
                    {
                        cp.GetMaterial(i).obj.SetFloat("_AmbientCol", ambientPercent);
                    }
                }
            }
        }
    }
}
