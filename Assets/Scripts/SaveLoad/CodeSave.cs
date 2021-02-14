﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class CodeSave
{
    private BlockSave[] blocks;
    private LinkSave[] links;

    public static void Save(CodeWindow code, string fileName)
    {
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Save data/");

        CodeSave converted = new CodeSave();
        converted.blocks = new BlockSave[code.Blocks.BlockCount];
        // save block data in correct format
        for (int i = 0; i < code.Blocks.BlockCount; i++)
        {
            // get values of all integrated parameters
            Parameter[] param = new Parameter[code.Blocks[i].IntegratedParamCount];
            for (int j = 0; j < code.Blocks[i].IntegratedParamCount; j++)
            {
                param[j] = code.Blocks[i].GetIntegratedParamValue(j);
            }
            converted.blocks[i] = new BlockSave(code.Blocks[i].ID, code.Blocks[i].transform.position, param);
        }
        // save links between blocks
        converted.links = new LinkSave[code.Links.LinkCount];
        for (int i = 0; i < code.Links.LinkCount; i++)
        {
            Block bIn = code.Links[i].Input.Block;
            Block bOut = code.Links[i].Output.Block;
            converted.links[i] = new LinkSave(CodeBlocks.GetBlockIndex(bIn), CodeBlocks.GetBlockIndex(bOut),
                Connector.GetConnectorIndex(code.Links[i].Input), Connector.GetConnectorIndex(code.Links[i].Output));
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.streamingAssetsPath + "/Save data/" + fileName, FileMode.OpenOrCreate);
        bf.Serialize(file, converted);
        file.Close();
    }
    public static void Load(CodeWindow code, string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.streamingAssetsPath + "/Save data/" + fileName, FileMode.Open);
        CodeSave converted = (CodeSave)bf.Deserialize(file);
        file.Close();

        foreach (var block in converted.blocks)
        {
            Block.InstantiateBlock(block.ID, block.Position, block.InputValues);
        }
        foreach (var link in converted.links)
        {
            Link.InstantiateLink(code.Blocks[link.InputBlockIndex].GetInput(link.InputBlockInputIndex),
                code.Blocks[link.OutputBlockIndex].GetOutput(link.OuputBlockOutputIndex));
        }
    }
    public static CodeSave LoadForExecute(string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.streamingAssetsPath + "/Save data/" + fileName, FileMode.Open);
        CodeSave converted = (CodeSave)bf.Deserialize(file);
        file.Close();

        return converted;
    }

    public BlockSave[] GetFlowBlocks(BlockSave block)
    {
        List<BlockSave> flowing = new List<BlockSave>();
        for (int l = 0; l < links.Length; l++)
        {
            // output block in link is equal to given block and
            // connector to which it is linked is of type flow
            if (blocks[links[l].OutputBlockIndex] == block &&
                Database.Instance[block.ID].Output(links[l].OuputBlockOutputIndex) <= ConnectorID.FlowIfFalse)
            {
                flowing.Add(blocks[links[l].InputBlockIndex]);
            }
        }
        return flowing.ToArray();
    }
    public KeyValuePair<int, BlockSave>[] GetBlocksConnectedToInputs(BlockSave block)
    {
        List<KeyValuePair<int, BlockSave>> data = new List<KeyValuePair<int, BlockSave>>();
        for (int i = 0; i < block.InputValues.Length; i++)
        {
            // skip flow input connectors
            if (Database.Instance[block.ID].Input(i) <= ConnectorID.FlowIfFalse)
            {
                continue;
            }
            for (int l = 0; l < links.Length; l++)
            {
                // add blocks that are linked to current connector
                if (blocks[links[l].InputBlockIndex] == block && links[l].InputBlockInputIndex == i)
                {
                    data.Add(new KeyValuePair<int, BlockSave>(i, blocks[links[l].OutputBlockIndex]));
                }
            }
        }
        return data.ToArray();
    }
    public BlockSave GetBlock(int i) { return blocks[i]; }
    public LinkSave GetLink(int i) { return links[i]; }

    public int BlockCount { get { return blocks.Length; } }
    public int LinkCount { get { return links.Length; } }
    public int UpdateBlock 
    { 
        get 
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].ID == BlockID.Update)
                {
                    return i;
                }
            }
            return -1;
        } 
    }
}
[Serializable]
public class BlockSave
{
    private BlockID id;
    private float x, y;
    private Parameter[] inputValues;

    public BlockSave(BlockID id, Vector2 position, Parameter[] inputValues)
    {
        this.x = position.x;
        this.y = position.y;
        this.id = id;
        this.inputValues = inputValues;
    }

    public BlockID ID { get { return id; } }
    public Vector2 Position { get { return new Vector2(x, y); } }
    public Parameter[] InputValues { get { return inputValues; } }
}
[Serializable]
public class LinkSave
{
    public int InputBlockIndex { get; private set; }
    public int OutputBlockIndex { get; private set; }
    public int InputBlockInputIndex { get; private set; }
    public int OuputBlockOutputIndex { get; private set; }

    public LinkSave(int inputBlockIndex, int outputBlockIndex, int inputBlockInputIndex, int outputBlockOutputIndex)
    {
        this.InputBlockIndex = inputBlockIndex;
        this.OutputBlockIndex = outputBlockIndex;
        this.InputBlockInputIndex = inputBlockInputIndex;
        this.OuputBlockOutputIndex = outputBlockOutputIndex;
    }
}

/*
[Serializable]
public class LinkSave
{
    private uint outputIndex;
    private uint inputIndex;

    public LinkSave(int outputIndex, int inputIndex)
    {
        this.outputIndex = outputIndex;
        this.inputIndex = inputIndex;
    }

    public uint Output { get { return outputIndex; } }
    public uint Input { get { return inputIndex; } }
}*/