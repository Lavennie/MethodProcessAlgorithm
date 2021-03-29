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
    public TMP_InputField[] vector2Input;
    public TMP_InputField[] vector3Input;
    public IntegratedParameterDirection directionInput;

    private float closeHeight;
    private bool open = false;
    private ConnectorID type;

    private void Awake()
    {
        closeHeight = (((RectTransform)background.transform).rect.height - Database.CIRCUIT_WIDTH) / 2.0f;
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
        this.type = type;

        ((RectTransform)circuitLineLeft.transform).sizeDelta *= new Vector2(Database.CIRCUIT_WIDTH, 1);
        ((RectTransform)circuitLineBottom.transform).sizeDelta *= new Vector2(1, Database.CIRCUIT_WIDTH);
        ((RectTransform)circuitLineRight.transform).sizeDelta *= new Vector2(Database.CIRCUIT_WIDTH, 1);

        switch (type)
        {
            case ConnectorID.FlowNormal:
            case ConnectorID.FlowIfTrue:
            case ConnectorID.FlowIfFalse:
            case ConnectorID.Pickup:
            case ConnectorID.Enemy:
                break;
            case ConnectorID.Int:
                integerInput.transform.parent.gameObject.SetActive(true);
                integerInput.text = (paramValue != null) ? paramValue.GetValue().ToString() : "0";
                break;
            case ConnectorID.Float:
                floatInput.transform.parent.gameObject.SetActive(true);
                floatInput.text = string.Format("{0:0.0###}", (paramValue != null) ? paramValue.GetValue() : 0.0f);
                break;
            case ConnectorID.Bool:
                boolInput.transform.parent.gameObject.SetActive(true);
                boolInput.isOn = ((ParamBool)paramValue).GetValue();
                break;
            case ConnectorID.Vector2:
                vector2Input[0].transform.parent.gameObject.SetActive(true);
                for (int i = 0; i < 2; i++)
                {
                    vector2Input[i].text = string.Format("{0:0.0###}", (paramValue != null) ? ((ParamVector2)paramValue).GetValue()[i] : 0.0f);
                }
                break;
            case ConnectorID.Vector3:
                vector3Input[0].transform.parent.gameObject.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    vector3Input[i].text = string.Format("{0:0.0###}", (paramValue != null) ? ((ParamVector3)paramValue).GetValue()[i] : 0.0f);
                }
                break;
            case ConnectorID.Direction2:
                directionInput.transform.parent.gameObject.SetActive(true);
                directionInput.SetX((paramValue != null) ? ((ParamVector2)paramValue).GetValue()[0] : 0.0f);
                directionInput.SetY((paramValue != null) ? ((ParamVector2)paramValue).GetValue()[1] : 0.0f);
                break;
            default:
                Debug.LogError(type + " is not an implemented type for integrated parameters", this);
                break;
        }

        // force close
        ((RectTransform)background.transform).offsetMin =  new Vector2(((RectTransform)background.transform).offsetMin.x, closeHeight);
        ((RectTransform)background.transform).offsetMax = new Vector2(((RectTransform)background.transform).offsetMax.x, -closeHeight);
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
            // 8203 is some unwanted character that is at end
            return new ParamFloat(float.Parse(floatInput.textComponent.text.Trim((char)8203)));
        }
        else if (boolInput.IsActive())
        {
            return new ParamBool(boolInput.isOn);
        }
        else if (vector2Input[0].IsActive() && vector2Input[1].IsActive())
        {
            // 8203 is some unwanted character that is at end
            return new ParamVector2(float.Parse(vector2Input[0].text.Trim((char)8203)),
                                    float.Parse(vector2Input[1].text.Trim((char)8203)));
        }
        else if (vector3Input[0].IsActive() && vector3Input[1].IsActive() && vector3Input[2].IsActive())
        {
            // 8203 is some unwanted character that is at end
            return new ParamVector3(float.Parse(vector3Input[0].text.Trim((char)8203)),
                                    float.Parse(vector3Input[1].text.Trim((char)8203)),
                                    float.Parse(vector3Input[2].text.Trim((char)8203)));
        }
        else if (directionInput.enabled)
        {
            return new ParamVector2(directionInput.GetX(), directionInput.GetY());
        }
        else
        {
            return new ParamVoid();
        }
    }

    public bool HasIntegratedParam
    {
        get
        {
            return integerInput.IsActive() ||
                floatInput.IsActive() ||
                boolInput.IsActive() ||
                (vector2Input[0].IsActive() && vector2Input[1].IsActive()) ||
                (vector3Input[0].IsActive() && vector3Input[1].IsActive() && vector3Input[2].IsActive()) ||
                directionInput.gameObject.activeInHierarchy;
        }
    }
}
