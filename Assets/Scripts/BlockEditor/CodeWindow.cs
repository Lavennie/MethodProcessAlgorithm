using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CodeWindow : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private void OnEnable()
    {
        Selector.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            CodeBlocks.DeleteBlocks(CodeBlocks.GetSelectedBlocks());
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
    }

    public void Run()
    {
        CodeSave code = CodeSave.LoadForExecute($"saveData{LevelManager.CurLevelNumber()}.save");
        if (code == null)
        {
            return;
        }
        CodeExecutor.Run(code);
    }
    public void Stop()
    {
        LevelManager.ReloadLevel();
    }

    public void Save()
    {
        CodeSave.Save(this, $"saveData{LevelManager.CurLevelNumber()}.save");
    }
    public void Load()
    {
        Blocks.transform.DetachChildren();
        Links.transform.DetachChildren();
        CodeSave.Load(this, $"saveData{LevelManager.CurLevelNumber()}.save");
        CodeBlocks.UpdateAllBlocksHighlight();
    }
    public void Clear()
    {
        CodeSave.ClearSave($"saveData{LevelManager.CurLevelNumber()}.save");
        Load();
    }
    public void ToMainMenu()
    {
        LevelManager.LoadMainMenu();
    }

    public void Show()
    {
        LevelManager.ReloadLevel();
        gameObject.SetActive(true);
        transform.parent.GetChild(0).gameObject.SetActive(false);
        transform.parent.GetChild(1).gameObject.SetActive(false);
        Load();
        CodeBlocks.UpdateAllBlocksHighlight();
    }
    public void Hide()
    {
        Save();
        gameObject.SetActive(false);
        transform.parent.GetChild(0).gameObject.SetActive(true);
        transform.parent.GetChild(1).gameObject.SetActive(true);
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
        if (eventData.button == PointerEventData.InputButton.Left ||
            eventData.button == PointerEventData.InputButton.Right)
        {
            Selector.gameObject.SetActive(true);
            Selector.transform.position = eventData.position;
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            foreach (var block in Blocks.GetBlocks())
            {
                block.SetDragging(true, false);
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        ((RectTransform)Selector.transform).sizeDelta = (eventData.position - (Vector2)Selector.transform.position) * new Vector2(1, -1);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        // select all blocks in area
        if (eventData.button == PointerEventData.InputButton.Left ||
            eventData.button == PointerEventData.InputButton.Right)
        {
            Selector.gameObject.SetActive(false);
            CodeBlocks.DeselectAll();
            Rect r1 = new Rect(Mathf.Min(Selector.position.x, eventData.position.x), Mathf.Min(Selector.position.y, eventData.position.y),
                Mathf.Abs(Selector.position.x - eventData.position.x), Mathf.Abs(Selector.position.y - eventData.position.y));
            foreach (var block in Blocks.GetBlocks())
            {
                Rect r2 = new Rect((Vector2)block.transform.position + ((RectTransform)block.transform).rect.min, ((RectTransform)block.transform).rect.size);
                if (!(r2.min.x > r1.max.x || r2.min.y > r1.max.y || r1.min.x > r2.max.x || r1.min.y > r2.max.y))
                {
                    block.SetSelected(true);
                }
            }
            // delete all blocks selected anew with area
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                CodeBlocks.DeleteBlocks(CodeBlocks.GetSelectedBlocks());
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            CodeBlocks.StopDragging();
        }
    }

    public Image Background { get { return GetComponent<Image>(); } }
    public CodeLinks Links { get { return transform.GetChild(0).GetComponent<CodeLinks>(); } }
    public CodeBlocks Blocks { get { return transform.GetChild(1).GetComponent<CodeBlocks>(); } }
    public Transform Selector { get { return transform.GetChild(2); } }
    public CodeBlockMenu Menu { get { return transform.GetChild(3).GetComponent<CodeBlockMenu>(); } }
    public Transform DraggedBlocks { get { return transform.GetChild(4); } }

    public static CodeWindow Instance { get { return Database.Instance?.CodeWindow; } }
}
