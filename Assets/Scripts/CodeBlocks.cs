using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeBlocks : MonoBehaviour
{
    public static void DeselectAll()
    {
        for (int i = 0; i < Instance.BlockCount; i++)
        {
            Instance[i].SetSelected(false);
        }
    }
    public static void BeginDragingSelected()
    {
        for (int i = 0; i < Instance.BlockCount; i++)
        {
            if (Instance[i].IsSelected)
            {
                Instance[i].SetDragging(true);
            }
        }
    }
    public static void StopDragging()
    {
        for (int i = 0; i < Instance.BlockCount; i++)
        {
            Instance[i].SetDragging(false);
        }
    }

    public static int GetBlockIndex(Block block)
    {
        return block.transform.GetSiblingIndex();
    }
    public Block[] GetBlocks()
    {
        Block[] blocks = new Block[BlockCount];
        for (int i = 0; i < BlockCount; i++)
        {
            blocks[i] = this[i];
        }
        return blocks;
    }

    public int BlockCount { get { return transform.childCount; } }
    public Block this[int i] { get { return transform.GetChild(i).GetComponent<Block>(); } }
    public static CodeBlocks Instance { get { return CodeWindow.Instance.Blocks; } }
}
