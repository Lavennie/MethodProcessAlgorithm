using UnityEngine;
using TMPro;

public class ColoredElementInputText : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private ColorPalette.Slot text;
    [SerializeField] private bool customCaretColor = true;
    [SerializeField] private ColorPalette.Slot caret;
    [SerializeField] private ColorPalette.Slot selection;

    protected virtual void OnEnable()
    {
        input.textComponent.color = Database.GetColor(text);
        input.customCaretColor = customCaretColor;
        input.caretColor = Database.GetColor(caret);
        input.selectionColor = Database.GetColor(selection);
    }
}
