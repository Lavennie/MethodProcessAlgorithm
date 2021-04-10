using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlate : MonoBehaviour
{
    private static TriggerPlate triggered = null;
    private static TriggerColor lastTriggered = TriggerColor.None;

    public enum TriggerColor
    {
        Yellow,
        Green,
        Blue,
        None = 8,
    }

    [SerializeField] private TriggerColor color;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() == null)
        {
            return;
        }

        switch (color)
        {
            case TriggerColor.Yellow:
                Recolor.RefreshColors(0);
                break;
            case TriggerColor.Green:
                Recolor.RefreshColors(1);
                break;
            case TriggerColor.Blue:
                Recolor.RefreshColors(2);
                break;
        }
        other.transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
        Triggered = this;

        ParticleSystem ps = Instantiate(Database.Instance.triggerPS, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        ParticleSystem.MainModule main = ps.main;
        switch (color)
        {
            case TriggerColor.Yellow:
                main.startColor = Database.GetColor(ColorPalette.Slot.Light1Fixed);
                ps.transform.GetChild(0).GetComponent<Light>().color = Database.GetColor(ColorPalette.Slot.Light1Fixed);
                break;
            case TriggerColor.Green:
                main.startColor = Database.GetColor(ColorPalette.Slot.Light2Fixed);
                ps.transform.GetChild(0).GetComponent<Light>().color = Database.GetColor(ColorPalette.Slot.Light2Fixed);
                break;
            case TriggerColor.Blue:
                main.startColor = Database.GetColor(ColorPalette.Slot.Light3Fixed);
                ps.transform.GetChild(0).GetComponent<Light>().color = Database.GetColor(ColorPalette.Slot.Light3Fixed);
                break;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() == null)
        {
            return;
        }

        if (Triggered == this)
        {
            Triggered = null;
        }
    }

    public static TriggerPlate Triggered 
    { 
        get { return triggered; }
        set
        {
            if (triggered == value)
            {
                return;
            }
            if (triggered == null)
            {
                lastTriggered = TriggerColor.None;
            }
            else
            {
                lastTriggered = triggered.color;
            }
            triggered = value;
            // recolor
        }
    }
    public static TriggerColor LastColor { get { return lastTriggered; } }
    public TriggerColor Color { get { return color; } }
}
