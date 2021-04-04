using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recolor : MonoBehaviour
{
    [SerializeField] private ColorPalette palette;
    [SerializeField] private Canvas canvas;

    private static Recolor instance;

    private void Awake()
    {
        instance = this;
        RefreshColors(0);
    }

    public static void RefreshColors(int usedLight)
    {
        instance.palette.usedColor = usedLight;
        instance.canvas.gameObject.SetActive(!instance.canvas.gameObject.activeSelf);
        instance.canvas.gameObject.SetActive(!instance.canvas.gameObject.activeSelf);
        for (int i = 0; i < instance.palette.MaterialCount; i++)
        {
            instance.palette.GetMaterial(i).obj.color = instance.palette.GetColorFromSlot(instance.palette.GetMaterial(i).colorSlot);
        }
    }
}
