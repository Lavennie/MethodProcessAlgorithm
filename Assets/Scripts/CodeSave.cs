using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class CodeSave
{
    private BlockSave[] blocks;

    public static void Save(CodeSave code, string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.streamingAssetsPath + "/" + fileName, FileMode.OpenOrCreate);
        bf.Serialize(file, code);
        file.Close();
    }
    public static CodeSave Load(string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.streamingAssetsPath + "/" + fileName, FileMode.Open);
        CodeSave code = (CodeSave)bf.Deserialize(file);
        file.Close();
        return code;
    }
}
[Serializable]
public class BlockSave
{
    private float x, y;
    private BlockID id;
    private Parameter[] inputValues;

    public BlockSave(BlockID id, Vector2 position, Parameter[] inputValues)
    {
        this.x = position.x;
        this.y = position.y;
        this.id = id;
        this.inputValues = inputValues;
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