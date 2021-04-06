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
        Red,
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
            case TriggerColor.Red:
                Recolor.RefreshColors(1);
                break;
            case TriggerColor.Blue:
                Recolor.RefreshColors(2);
                break;
        }
        Triggered = this;
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
