using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using TMPro;

public class CodeWindow : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private void OnEnable()
    {
        Background.color = ColorPalette.BgDark;

        Selector.GetChild(0).GetComponent<Image>().color = new Color(ColorPalette.SlateNormal.r, ColorPalette.SlateNormal.g, ColorPalette.SlateNormal.b, 0.5f);
        Selector.GetChild(1).GetComponent<Image>().color = new Color(ColorPalette.SlateNormal.r, ColorPalette.SlateNormal.g, ColorPalette.SlateNormal.b, 0.5f);
        Selector.GetChild(2).GetComponent<Image>().color = new Color(ColorPalette.SlateNormal.r, ColorPalette.SlateNormal.g, ColorPalette.SlateNormal.b, 0.5f);
        Selector.GetChild(3).GetComponent<Image>().color = new Color(ColorPalette.SlateNormal.r, ColorPalette.SlateNormal.g, ColorPalette.SlateNormal.b, 0.5f);
        Selector.GetChild(4).GetComponent<Image>().color = ColorPalette.SlateNormal;
        Selector.GetChild(5).GetComponent<Image>().color = ColorPalette.SlateNormal;
        Selector.GetChild(6).GetComponent<Image>().color = ColorPalette.SlateNormal;
        Selector.GetChild(7).GetComponent<Image>().color = ColorPalette.SlateNormal;
        Selector.GetChild(8).GetComponent<Image>().color = ColorPalette.SlateNormal;
        Selector.GetChild(9).GetComponent<Image>().color = ColorPalette.SlateNormal;
        Selector.GetChild(10).GetComponent<Image>().color = ColorPalette.SlateNormal;
        Selector.GetChild(11).GetComponent<Image>().color = ColorPalette.SlateNormal;
        Selector.gameObject.SetActive(false);

        Zoom.fillRect.GetComponent<Image>().color = ColorPalette.SlateNormal;
        Zoom.transform.GetChild(0).GetComponent<Image>().color = ColorPalette.SlateDark;
        Zoom.targetGraphic.color = ColorPalette.LightColor;
        ZoomText.color = ColorPalette.SlateNormal;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            List<Block> deleted = new List<Block>();
            for (int i = 0; i < Blocks.BlockCount; i++)
            {
                if (Blocks[i].IsSelected)
                {
                    deleted.Add(Blocks[i]);
                    Destroy(Blocks[i].gameObject);
                }
            }
            for (int i = 0; i < Links.LinkCount; i++)
            {
                if (deleted.Contains(Links[i].Input.Block) || deleted.Contains(Links[i].Output.Block))
                {
                    Destroy(Links[i].gameObject);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
    }

    public void Run()
    {
        CodeSave code = CodeSave.LoadForExecute("saveData1.save");
        if (code == null)
        {
            return;
        }
        CodeExecutor.Run(code);
    }

    public void Scale(float value)
    {
        ZoomText.text = string.Format("Zoom: {0}x", value);
    }
    public void Save()
    {
        Debug.Log("Save to saveData1.save");
        CodeSave.Save(this, "saveData1.save");
    }
    public void Load()
    {
        Debug.Log("Load from saveData1.save");
        Blocks.transform.DetachChildren();
        Links.transform.DetachChildren();
        CodeSave.Load(this, "saveData1.save");
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Load();
    }
    public void Hide()
    {
        Save();
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            CodeLinks.DropDraggedLink();
            CodeBlocks.DeselectAll();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Selector.gameObject.SetActive(true);
        Selector.transform.position = eventData.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        ((RectTransform)Selector.transform).sizeDelta = (eventData.position - (Vector2)Selector.transform.position) * new Vector2(1, -1);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Selector.gameObject.SetActive(false);
        CodeBlocks.DeselectAll();
        foreach (var block in Blocks.GetBlocks())
        {
            Rect r1 = new Rect((Vector2)block.transform.position + ((RectTransform)block.transform).rect.min, ((RectTransform)block.transform).rect.size);
            Rect r2 = new Rect(Mathf.Min(Selector.position.x, eventData.position.x), Mathf.Min(Selector.position.y, eventData.position.y),
                Mathf.Abs(Selector.position.x - eventData.position.x), Mathf.Abs(Selector.position.y - eventData.position.y));
            if (!(r1.min.x > r2.max.x || r1.min.y > r2.max.y || r2.min.x > r1.max.x || r2.min.y > r1.max.y))
            {
                block.SetSelected(true);
            }
            else
            {
            }
        }
    }


    public Image Background { get { return GetComponent<Image>(); } }
    public CodeBlockMenu Menu { get { return transform.GetChild(0).GetComponent<CodeBlockMenu>(); } }
    public CodeLinks Links { get { return transform.GetChild(1).GetComponent<CodeLinks>(); } }
    public CodeBlocks Blocks { get { return transform.GetChild(2).GetComponent<CodeBlocks>(); } }
    public Transform Selector { get { return transform.GetChild(3); } }
    public Slider Zoom { get { return transform.GetChild(4).GetChild(0).GetComponent<Slider>(); } }
    public TextMeshProUGUI ZoomText { get { return transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>(); } }
    public Transform Buttons { get { return transform.GetChild(5); } }
    public Button ButtonSave { get { return Buttons.GetChild(0).GetComponent<Button>(); } }
    public Button ButtonLoad { get { return Buttons.GetChild(1).GetComponent<Button>(); } }

    public static CodeWindow Instance { get { return Database.Instance.CodeWindow; } }
}
