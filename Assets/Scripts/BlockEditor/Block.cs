﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class Block : MonoBehaviour, IBeginDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public enum HighlightOption
    {
        Disabled,
        Enabled,
        Error,
    }

    private BlockID id;
    private bool clicked = false;

    private void Start()
    {
        SetSelected(false);
    }

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
            input.Init(data.Input(i));

            // create integrated param for connector
            IntegratedParameter integratedParam = Instantiate(Database.Instance.integratedParamPrefab, block.InputContainer);
            ((RectTransform)integratedParam.transform).anchoredPosition -= new Vector2(0, i * ((RectTransform)input.transform).sizeDelta.y);
            integratedParam.Init(data.Input(i), (inputValues != null) ? inputValues[i] : null);
        }
        for (int i = 0; i < data.OutputCount; i++)
        {
            Connector output = Instantiate(Database.Instance.outputPrefab, block.OutputContainer);
            ((RectTransform)output.transform).anchoredPosition = new Vector2(0, -i * ((RectTransform)output.transform).sizeDelta.y);
            output.Init(data.Output(i));
        }

        return block;
    }

    public Connector GetInput(int i)
    {
        return InputContainer.GetChild(i * 2).GetComponent<Connector>();
    }
    public Connector GetOutput(int i)
    {
        return OutputContainer.GetChild(i).GetComponent<Connector>();
    }
    public Parameter GetIntegratedParamValue(int i)
    {
        return InputContainer.GetChild(i * 2 + 1).GetComponent<IntegratedParameter>().GetValue();
    }

    public void SetDragging(bool enabled, bool bringFront)
    {
        if (GetComponent<BlockDrag>().dragging == enabled)
        {
            return;
        }
        if (enabled)
        {
            GetComponent<BlockDrag>().offset = transform.position - Input.mousePosition;
        }
        for (int i = 0; i < IntegratedParamCount; i++)
        {
            InputContainer.GetChild(i * 2 + 1).GetComponent<IntegratedParameter>().enabled = !enabled;
        }
        GetComponent<BlockDrag>().dragging = enabled;
        this.enabled = !enabled;

        if (enabled)
        {
            if (bringFront)
            {
                transform.SetParent(CodeWindow.Instance.DraggedBlocks, false);
            }
        }
        else
        {
            transform.SetParent(CodeWindow.Instance.Blocks.transform, false);
        }
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
        if (clicked)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    CodeBlocks.DeselectAll();
                }
                SetSelected(!IsSelected);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                CodeBlocks.DeleteBlocks(new Block[1] { this });
            }
        }
    }

    public void SetHighlightOption(HighlightOption option)
    {
        switch (option)
        {
            case HighlightOption.Disabled:
                GetComponent<ColoredElementHighlight>().Unhighlight();
                break;
            case HighlightOption.Enabled:
                GetComponent<ColoredElementHighlight>().Highlight();
                break;
            case HighlightOption.Error:
                GetComponent<ColoredElementHighlight>().SetCustomColor(ColorPalette.Slot.LightError);
                break;
        }
    }

    public BlockID ID { get { return id; } }
    public int InputCount { get { return InputContainer.childCount / 2; } }
    public int OutputCount { get { return OutputContainer.childCount; } }
    public int IntegratedParamCount { get { return InputCount; } }

    public bool IsSelected { get { return Outline.gameObject.activeSelf; } }

    public Image Outline { get { return transform.GetChild(0).GetComponent<Image>(); } }
    public Image Background { get { return transform.GetChild(2).GetComponent<Image>(); } }
    public Image Group { get { return transform.GetChild(3).GetComponent<Image>(); } }
    public TextMeshProUGUI GroupText { get { return Group.transform.GetChild(0).GetComponent<TextMeshProUGUI>(); } }
    public TextMeshProUGUI NameText { get { return transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>(); } }
    public Transform InputContainer { get { return transform.GetChild(5); } }
    public Transform OutputContainer { get { return transform.GetChild(6); } }
    public Transform Lights { get { return transform.GetChild(7); } }
}
