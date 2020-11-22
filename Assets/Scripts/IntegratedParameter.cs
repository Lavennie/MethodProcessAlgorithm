using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public sealed class IntegratedParameter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image background;
    public Image circuitLineLeft;
    public Image circuitLineBottom;
    public Image circuitLineRight;

    public TMP_InputField integerInput;
    public TMP_InputField floatInput;
    public Toggle boolInput;

    private float closeHeight;
    private bool open = false;

    private void Awake()
    {
        closeHeight = (((RectTransform)background.transform).rect.height - Database.CIRCUIT_WIDTH) / 2.0f;
    }

    private void OnEnable()
    {
        background.color = ColorPalette.BgDark;
        circuitLineLeft.color = ColorPalette.BgLight;
        circuitLineBottom.color = ColorPalette.BgLight;
        circuitLineRight.color = ColorPalette.BgLight;
    }

    private void Update()
    {
        float target = (open) ? 0 : closeHeight;
        float openCloseSpeed = Time.deltaTime * 100.0f;
        ((RectTransform)background.transform).offsetMin = Vector2.MoveTowards(
            ((RectTransform)background.transform).offsetMin,
            new Vector2(((RectTransform)background.transform).offsetMin.x,
            target), openCloseSpeed);
        ((RectTransform)background.transform).offsetMax = Vector2.MoveTowards(
            ((RectTransform)background.transform).offsetMax,
            new Vector2(((RectTransform)background.transform).offsetMax.x,
            -target), openCloseSpeed);
    }

    public void Init(ConnectorID type, Parameter paramValue)
    {
        ((RectTransform)circuitLineLeft.transform).sizeDelta *= new Vector2(Database.CIRCUIT_WIDTH, 1);
        ((RectTransform)circuitLineBottom.transform).sizeDelta *= new Vector2(1, Database.CIRCUIT_WIDTH);
        ((RectTransform)circuitLineRight.transform).sizeDelta *= new Vector2(Database.CIRCUIT_WIDTH, 1);

        switch (type)
        {
            case ConnectorID.FlowNormal:
            case ConnectorID.FlowIfTrue:
            case ConnectorID.FlowIfFalse:
                break;
            case ConnectorID.Int:
                integerInput.transform.parent.gameObject.SetActive(true);
                integerInput.textComponent.text = (paramValue != null) ? paramValue.ToString() : "0";
                integerInput.textComponent.color = ColorPalette.BgLight;
                integerInput.customCaretColor = true;
                integerInput.caretColor = ColorPalette.BgLight;
                integerInput.selectionColor = ColorPalette.LightColor;
                break;
            case ConnectorID.Float:
                floatInput.transform.parent.gameObject.SetActive(true);
                floatInput.textComponent.text = (paramValue != null) ? paramValue.ToString() : "0.0";
                floatInput.textComponent.color = ColorPalette.BgLight;
                floatInput.customCaretColor = true;
                floatInput.caretColor = ColorPalette.BgLight;
                floatInput.selectionColor = ColorPalette.LightColor;
                break;
            case ConnectorID.Bool:
                boolInput.transform.parent.gameObject.SetActive(true);
                boolInput.isOn = (paramValue != null) ? ((ParamBool)paramValue).GetValue() : true;
                break;
            case ConnectorID.Direction2:
            default:
                Debug.LogError(type + " is not an implemented type for integrated parameters", this);
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HasIntegratedParam)
        {
            open = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        open = false;
    }

    public Parameter GetValue()
    {
        if (integerInput.IsActive())
        {
            return new ParamInteger(int.Parse(integerInput.textComponent.text));
        }
        else if (floatInput.IsActive())
        {
            return new ParamFloat(float.Parse(integerInput.textComponent.text));
        }
        else if (boolInput.IsActive())
        {
            return new ParamBool(boolInput.isOn);
        }
        else 
        {
            return null; 
        }

    }

    public bool HasIntegratedParam
    {
        get
        {
            return integerInput.IsActive() ||
                floatInput.IsActive() ||
                boolInput.IsActive();
        }
    }
}
