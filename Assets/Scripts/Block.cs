using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Block : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI groupText;
    public Transform inputContainer;
    public Transform outputContainer;

    public void SetName(string name)
    {
        nameText.text = name;
    }
    public void SetGroup(string group)
    {
        groupText.text = group;
    }
}
