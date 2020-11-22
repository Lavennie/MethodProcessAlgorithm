using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class CodeSave
{
    private BlockSave[] blocks;
    private Tuple<int, int>[] links;

    public static void Save(CodeBlocks code, string fileName)
    {
        CodeSave converted = new CodeSave();
        converted.blocks = new BlockSave[code.BlockCount];
        // save block data in correct format
        for (int i = 0; i < code.BlockCount; i++)
        {
            // get values of all integrated parameters
            Parameter[] param = new Parameter[code[i].IntegratedParamCount];
            for (int j = 0; j < code[i].IntegratedParamCount; j++)
            {
                param[i] = code[i][j];
            }
            converted.blocks[i] = new BlockSave(code[i].ID, code[i].transform.position, param);
        }
        // save links between formated blocks
        List<Tuple<int, int>> linksList = new List<Tuple<int, int>>();
        for (int b = 0; b < code.BlockCount; b++)
        {
            for (int o = 0; o < code[b].OutputCount; o++)
            {
                Block linkedBlock = code[b].GetOutput(o).Block;

                int block1 = b;
                int block2 = Array.IndexOf<Block>(code.GetBlocks(), linkedBlock);

                linksList.Add(new Tuple<int, int>(block1, block2));
            }
        }
        converted.links = linksList.ToArray();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.streamingAssetsPath + "/" + fileName, FileMode.OpenOrCreate);
        bf.Serialize(file, code);
        file.Close();
    }
    public static void Load(CodeBlocks code, string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.streamingAssetsPath + "/" + fileName, FileMode.Open);
        CodeSave converted = (CodeSave)bf.Deserialize(file);
        file.Close();

        code.transform.DetachChildren();
        foreach (var block in converted.blocks)
        {
            Block.InstantiateBlock(block.ID, block.Position, block.InputValues);
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

    public BlockID ID { get { return ID; } }
    public Vector2 Position { get { return new Vector2(x, y); } }
    public Parameter[] InputValues { get { return inputValues; } }
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