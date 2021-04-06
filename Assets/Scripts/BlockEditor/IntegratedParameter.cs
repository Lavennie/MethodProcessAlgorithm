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

    public TextMeshProUGUI angleInput;
    public IntegratedParameterWheel angleWheel;
    public TMP_InputField floatInput;
    public Toggle boolInput;
    public TMP_InputField[] vector2Input;
    public TMP_InputField[] vector3Input;
    public TextMeshProUGUI[] directionInput;
    public IntegratedParameterWheel directionWheel;
    public Toggle[] colorInput;

    private float closeHeight;
    private bool open = false;
    private ConnectorID type;

    private void Awake()
    {
        closeHeight = (((RectTransform)background.transform).rect.height - Database.CIRCUIT_WIDTH) / 2.0f;
    }

    private void Update()
    {
        open = ShouldBeOpen;

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
            case ConnectorID.Angle:
                angleInput.transform.parent.parent.gameObject.SetActive(true);
                int angle = (paramValue != null) ? ((ParamInteger)paramValue).GetValue() : 0;
                angleInput.text = angle.ToString();
                angleWheel.PointToAngle(angle);
                break;
            case ConnectorID.Float:
                floatInput.transform.parent.gameObject.SetActive(true);
                floatInput.text = string.Format("{0:0.0###}", (paramValue != null) ? paramValue.GetValue() : 0.0f);
                break;
            case ConnectorID.Bool:
                boolInput.transform.parent.gameObject.SetActive(true);
                boolInput.isOn = (paramValue != null) ? ((ParamBool)paramValue).GetValue() : false;
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
                directionInput[0].transform.parent.parent.gameObject.SetActive(true);
                Vector2 direction = new Vector2((paramValue != null) ? ((ParamVector2)paramValue).GetValue()[0] : 0.0f,
                                                (paramValue != null) ? ((ParamVector2)paramValue).GetValue()[1] : 1.0f);
                directionInput[0].text = string.Format("{0:0.0#}", direction.x);
                directionInput[1].text = string.Format("{0:0.0#}", direction.y);
                directionWheel.PointToDirection(direction);
                break;
            case ConnectorID.Color:
                colorInput[0].transform.parent.gameObject.SetActive(true);
                if (paramValue != null)
                {
                    switch (((ParamColor)paramValue).GetValue())
                    {
                        case TriggerPlate.TriggerColor.Yellow:
                            colorInput[0].isOn = true;
                            colorInput[1].isOn = false;
                            colorInput[1].isOn = false;
                            break;
                        case TriggerPlate.TriggerColor.Red:
                            colorInput[0].isOn = false;
                            colorInput[1].isOn = true;
                            colorInput[1].isOn = false;
                            break;
                        case TriggerPlate.TriggerColor.Blue:
                            colorInput[0].isOn = false;
                            colorInput[1].isOn = false;
                            colorInput[1].isOn = true;
                            break;
                    }
                }
                else
                {
                    colorInput[0].isOn = true;
                    colorInput[1].isOn = false;
                    colorInput[1].isOn = false;
                }
                break;
            default:
                Debug.LogError(type + " is not an implemented type for integrated parameters", this);
                break;
        }

        // force close
        ((RectTransform)background.transform).offsetMin = new Vector2(((RectTransform)background.transform).offsetMin.x, closeHeight);
        ((RectTransform)background.transform).offsetMax = new Vector2(((RectTransform)background.transform).offsetMax.x, -closeHeight);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //open = false;
    }

    public Parameter GetValue()
    {
        if (angleInput.IsActive())
        {
            return new ParamInteger(int.Parse(angleInput.text));
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
        else if (directionInput[0].IsActive() && directionInput[1].IsActive())
        {
            return new ParamVector2(float.Parse(directionInput[0].text.Trim((char)8203)),
                                    float.Parse(directionInput[1].text.Trim((char)8203)));
        }
        else if (colorInput[0].IsActive() && colorInput[1].IsActive() && colorInput[2].IsActive())
        {
            if (colorInput[0].isOn)
            {
                return new ParamColor(TriggerPlate.TriggerColor.Yellow);
            }
            else if (colorInput[1].isOn)
            {
                return new ParamColor(TriggerPlate.TriggerColor.Red);
            }
            else if (colorInput[2].isOn)
            {
                return new ParamColor(TriggerPlate.TriggerColor.Blue);
            }
            else
            {
                return new ParamColor(TriggerPlate.TriggerColor.None);
            }
        }
        else
        {
            return new ParamVoid();
        }
    }

    public void UpdateAngleParamFromWheel(Vector2 direction, float angle)
    {
        angleInput.text = ((int)angle).ToString();
    }
    public void UpdateDirectionParamFromWheel(Vector2 direction, float angle)
    {
        directionInput[0].text = string.Format("{0:0.0#}", direction.x);
        directionInput[1].text = string.Format("{0:0.0#}", direction.y);
    }

    public bool HasIntegratedParam
    {
        get
        {
            return angleInput.IsActive() ||
                floatInput.IsActive() ||
                boolInput.IsActive() ||
                (vector2Input[0].IsActive() && vector2Input[1].IsActive()) ||
                (vector3Input[0].IsActive() && vector3Input[1].IsActive() && vector3Input[2].IsActive()) ||
                (directionInput[0].IsActive() && directionInput[1].IsActive()) ||
                (colorInput[0].IsActive() && colorInput[1].IsActive() && colorInput[2].IsActive());
        }
    }
    public bool ShouldBeOpen
    {
        get 
        {
            // if the connector is any type other than flow and it is not linked
            return type > ConnectorID.FlowIfFalse &&
                transform.parent.GetChild(transform.GetSiblingIndex() - 1).GetComponent<Connector>().GetConnected() == null;
        }
    }
}
