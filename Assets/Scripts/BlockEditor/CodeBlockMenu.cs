using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodeBlockMenu : MonoBehaviour
{
    private void OnEnable()
    {
        foreach (var block in Database.Instance)
        {
            CodeBlockMenuEntry.InstantiateMenuEntry(block.Key, Container);
        }
    }

    public Image Background { get { return transform.GetChild(0).GetComponent<Image>(); } }
    public Image Resizer { get { return transform.GetChild(1).GetComponent<Image>(); } }
    public Transform Container { get { return Background.transform.GetChild(0); } }
}
