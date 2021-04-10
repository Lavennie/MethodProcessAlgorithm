using System.Linq;
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
                Instance[i].SetDragging(true, false);
            }
        }
    }
    public static void StopDragging()
    {
        for (int i = 0; i < Instance.BlockCount; i++)
        {
            Instance[i].SetDragging(false, false);

        }
        while (Instance.DraggedBlockCount > 0)
        {
            CodeWindow.Instance.DraggedBlocks.GetChild(0).GetComponent<Block>().SetDragging(false, false);
        }
    }

    public static void UpdateAllBlocksHighlight()
    {
        for (int i = 0; i < Instance.BlockCount; i++)
        {
            if (IsLoopConnected(Instance[i]))
            {
                Instance[i].SetHighlightOption(Block.HighlightOption.Error);
            }
            else if (IsConnectedToUpdate(Instance[i]))
            {
                Instance[i].SetHighlightOption(Block.HighlightOption.Enabled);
            }
            else
            {
                Instance[i].SetHighlightOption(Block.HighlightOption.Disabled);
            }
        }
    }
    public static bool IsConnectedToUpdate(Block block)
    {
        if (block.ID == BlockID.Update)
        {
            return true;
        }
        // block has flow connector that needs to be connected to update
        if (block.InputCount > 0 && block.GetInput(0).DataType <= ConnectorID.FlowIfFalse)
        {
            Connector connected = block.GetInput(0).GetConnected();
            if (connected == null)
            {
                return false;
            }
            else
            {
                return IsConnectedToUpdate(connected.Block);
            }
        }
        // block is of type to get data (has no flow input), needs to be connected through its outputs
        else
        {
            for (int i = 0; i < block.OutputCount; i++)
            {
                Connector connected = block.GetOutput(i).GetConnected();
                if (connected != null && IsConnectedToUpdate(connected.Block))
                {
                    return true;
                }
            }
            return false;
        }
    }
    public static bool IsLoopConnected(Block block, Block checkStartBlock = null)
    {
        if (block == checkStartBlock)
        {
            return true;
        }

        if (checkStartBlock == null)
        {
            checkStartBlock = block;
        }

        if (block.InputCount > 0)
        {
            for (int i = 0; i < block.InputCount; i++)
            {
                Connector input = block.GetInput(i);
                // multiple blocks can be connected to flow input
                if (input.DataType <= ConnectorID.FlowIfFalse)
                {
                    Connector[] connected = input.GetConnectedMultiple();
                    for (int j = 0; j < connected.Length; j++)
                    {
                        if (IsLoopConnected(connected[j].Block, checkStartBlock))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    Connector connected = block.GetInput(i).GetConnected();
                    if (connected != null)
                    {
                        if (IsLoopConnected(connected.Block, checkStartBlock))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public static Block[] GetSelectedBlocks()
    {
        List<Block> blocks = new List<Block>();
        for (int i = 0; i < Instance.BlockCount; i++)
        {
            if (Instance[i].IsSelected)
            {
                blocks.Add(Instance[i]);
            }
        }
        return blocks.ToArray();
    }

    public static void DeleteBlocks(Block[] blocks)
    {
        for (int i = 0; i < CodeLinks.Instance.LinkCount; i++)
        {
            if (blocks.Contains(CodeLinks.Instance[i].Input.Block) || blocks.Contains(CodeLinks.Instance[i].Output.Block))
            {
                DestroyImmediate(CodeLinks.Instance[i].gameObject);
            }
        }
        for (int i = 0; i < blocks.Length; i++)
        {
            DestroyImmediate(blocks[i].gameObject);
        }
        UpdateAllBlocksHighlight();
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
    public int DraggedBlockCount { get { return CodeWindow.Instance.DraggedBlocks.childCount; } }
    public Block this[int i] { get { return transform.GetChild(i).GetComponent<Block>(); } }
    public static CodeBlocks Instance { get { return CodeWindow.Instance.Blocks; } }
}
