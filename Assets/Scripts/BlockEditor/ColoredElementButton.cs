using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ColoredElementButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private ColorPalette.Slot textColor = ColorPalette.Slot.BackgroundNormal;
    [SerializeField] private ColorPalette.Slot restColor = ColorPalette.Slot.ForegroundNormal;
    [SerializeField] private ColorPalette.Slot hoverColor = ColorPalette.Slot.Light1;
    [SerializeField] private ColorPalette.Slot pressColor = ColorPalette.Slot.FadedLight1;

    public UnityEvent onClick;

    protected virtual void OnEnable()
    {
        GetComponent<Image>().color = Database.GetColor(restColor);
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Database.GetColor(textColor);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = Database.GetColor(hoverColor);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = Database.GetColor(restColor);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Image>().color = Database.GetColor(pressColor);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        AudioManager.PlaySoundEffect(AudioManager.SoundEffects.ButtonClick);
        onClick.Invoke();
        GetComponent<Image>().color = Database.GetColor(hoverColor);
    }
}
