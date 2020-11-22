using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class Block : MonoBehaviour, IBeginDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private BlockID id;
    private bool clicked = false;

    public static Block InstantiateBlock(BlockID id, Vector2 position, Parameter[] inputValues)
    {
        BlockData data = Database.Instance[id];

        Block block = Instantiate(Database.Instance.blockPrefab, CodeWindow.Instance.Blocks.transform);
        block.id = id;
        block.NameText.text = data.Name;
        block.GroupText.text = data.Group;
        ((RectTransform)block.transform).sizeDelta += new Vector2(0,
            ((RectTransform)Database.Instance.inputPrefab.transform).sizeDelta.y * Mathf.Max(data.InputCount, data.OutputCount));
        block.transform.position = position;

        for (int i = 0; i < data.InputCount; i++)
        {
            Connector input = Instantiate(Database.Instance.inputPrefab, block.InputContainer);
            ((RectTransform)input.transform).anchoredPosition -= new Vector2(0, i * ((RectTransform)input.transform).sizeDelta.y);

            IntegratedParameter integratedParam = Instantiate(Database.Instance.integratedParamPrefab, block.InputContainer);
            ((RectTransform)integratedParam.transform).anchoredPosition -= new Vector2(0, i * ((RectTransform)input.transform).sizeDelta.y);
            integratedParam.Init(data.Input(i), (inputValues != null) ? inputValues[i] : null);
        }
        for (int i = 0; i < data.OutputCount; i++)
        {
            Connector output = Instantiate(Database.Instance.outputPrefab, block.OutputContainer);
            ((RectTransform)output.transform).anchoredPosition = new Vector2(0, -i * ((RectTransform)output.transform).sizeDelta.y);
        }

        return block;
    }

    private void OnEnable()
    {
        Outline.color = ColorPalette.LightColor;
        Background.color = ColorPalette.BgNormal;
        Group.color = ColorPalette.BgLight;
        GroupText.color = ColorPalette.SlateNormal;
        NameText.color = ColorPalette.SlateNormal;
    }

    private void Start()
    {
        SetSelected(false);
    }

    public Connector GetInput(int i)
    {
        return InputContainer.GetChild(i * 2).GetComponent<Connector>();
    }
    public Connector GetOutput(int i)
    {
        return OutputContainer.GetChild(i).GetComponent<Connector>();
    }

    public void SetDragging(bool enabled)
    {
        if (GetComponent<BlockDrag>().dragging == enabled)
        {
            return;
        }
        if (enabled)
        {
            GetComponent<BlockDrag>().offset = transform.position - Input.mousePosition;
        }
        GetComponent<BlockDrag>().dragging = enabled;
        this.enabled = !enabled;
    }
    public void SetSelected(bool selected)
    {
        Outline.gameObject.SetActive(selected);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        clicked = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        clicked = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (clicked && eventData.button == PointerEventData.InputButton.Left)
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                CodeBlocks.DeselectAll();
            }
            SetSelected(!IsSelected);
        }
    }

    public BlockID ID { get { return id; } }
    public int InputCount { get { return InputContainer.childCount / 2; } }
    public int OutputCount { get { return OutputContainer.childCount; } }
    public int IntegratedParamCount { get { return InputCount; } }
    public Parameter this[int i] { get { return InputContainer.GetChild(i * 2 + 1).GetComponent<IntegratedParameter>().GetValue(); } }

    public bool IsSelected { get { return Outline.gameObject.activeSelf; } }

    public Image Outline { get { return transform.GetChild(0).GetComponent<Image>(); } }
    public Image Background { get { return transform.GetChild(1).GetComponent<Image>(); } }
    public Image Group { get { return transform.GetChild(2).GetComponent<Image>(); } }
    public TextMeshProUGUI GroupText { get { return Group.transform.GetChild(0).GetComponent<TextMeshProUGUI>(); } }
    public TextMeshProUGUI NameText { get { return transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>(); } }
    public Transform InputContainer { get { return transform.GetChild(4); } }
    public Transform OutputContainer { get { return transform.GetChild(5); } }
}
