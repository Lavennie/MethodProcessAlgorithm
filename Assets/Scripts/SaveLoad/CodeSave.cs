using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class CodeSave
{
    private BlockSave[] blocks;
    private LinkSave[] links;

#if UNITY_WEBGL
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void SyncFiles();
#endif

    public static void ClearSave(string fileName)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Save data/");

        CodeSave save = new CodeSave();
        save.blocks = new BlockSave[1]
        {
            new BlockSave(BlockID.Update, 
            new Vector2(((RectTransform)CodeWindow.Instance.Menu.transform).sizeDelta.x + 200, Screen.height -100), 
            new Parameter[0])
        };
        save.links = new LinkSave[0];

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.streamingAssetsPath + "/Save data/" + fileName, FileMode.OpenOrCreate);
        bf.Serialize(file, save);
        file.Close();
#elif UNITY_WEBGL
        int levelIndex = int.Parse(fileName.Split('.')[0].Substring(8, 1));
        PlayerPrefs.DeleteKey($"level{levelIndex}");
        PlayerPrefs.Save();
        SyncFiles();
#endif
    }
    public static void Save(CodeWindow code, string fileName)
    {
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

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Save data/");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.streamingAssetsPath + "/Save data/" + fileName, FileMode.OpenOrCreate);
        bf.Serialize(file, converted);
        file.Close();

#elif UNITY_WEBGL
        int levelIndex = int.Parse(fileName.Split('.')[0].Substring(8, 1));
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        using (ms)
        {
            bf.Serialize(ms, converted);
        }
        PlayerPrefs.SetString($"level{levelIndex}", System.Convert.ToBase64String(ms.ToArray()));
        PlayerPrefs.Save();
        SyncFiles();
#endif
    }
    public static void Load(CodeWindow code, string fileName)
    {
        CodeSave converted;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        BinaryFormatter bf = new BinaryFormatter();
        if (!File.Exists(Application.streamingAssetsPath + "/Save data/" + fileName))
        {
            ClearSave(fileName);
        }
        FileStream file = File.Open(Application.streamingAssetsPath + "/Save data/" + fileName, FileMode.Open);
        converted = (CodeSave)bf.Deserialize(file);
        file.Close();
#elif UNITY_WEBGL
        int levelIndex = int.Parse(fileName.Split('.')[0].Substring(8, 1));
        if (PlayerPrefs.HasKey($"level{levelIndex}"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(System.Convert.FromBase64String(PlayerPrefs.GetString($"level{levelIndex}", "")));
            converted = (CodeSave)bf.Deserialize(ms);
        }
        else
        {
            converted = new CodeSave();
            converted.blocks = new BlockSave[1]
            {
            new BlockSave(BlockID.Update,
            new Vector2(((RectTransform)CodeWindow.Instance.Menu.transform).sizeDelta.x + 200, Screen.height -100),
            new Parameter[0])
            };
            converted.links = new LinkSave[0];
        }
#endif

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
        CodeSave converted;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (!File.Exists(Application.streamingAssetsPath + "/Save data/" + fileName))
        {
            ClearSave(fileName);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.streamingAssetsPath + "/Save data/" + fileName, FileMode.Open);
        converted = (CodeSave)bf.Deserialize(file);
        file.Close();
#elif UNITY_WEBGL
        int levelIndex = int.Parse(fileName.Split('.')[0].Substring(8, 1));
        if (PlayerPrefs.HasKey($"level{levelIndex}"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(System.Convert.FromBase64String(PlayerPrefs.GetString($"level{levelIndex}", "")));
            converted = (CodeSave)bf.Deserialize(ms);
        }
        else
        {
            converted = new CodeSave();
            converted.blocks = new BlockSave[1]
            {
            new BlockSave(BlockID.Update,
            new Vector2(((RectTransform)CodeWindow.Instance.Menu.transform).sizeDelta.x + 200, Screen.height -100),
            new Parameter[0])
            };
            converted.links = new LinkSave[0];
        }
#endif
        return converted;
    }

    public BlockSave[] GetFlowBlocks(BlockSave block, bool type)
    {
        List<BlockSave> flowing = new List<BlockSave>();
        for (int l = 0; l < links.Length; l++)
        {
            // output block in link is equal to given block and
            // connector to which it is linked is of type flow
            if (blocks[links[l].OutputBlockIndex] == block &&
                Database.Instance[block.ID].Output(links[l].OuputBlockOutputIndex) <= ConnectorID.FlowIfFalse)
            {
                // and it fit the type (normal flow, flow when true or flow when false)
                if ((type == true && (Database.Instance[block.ID].Output(links[l].OuputBlockOutputIndex) == ConnectorID.FlowNormal ||
                                      Database.Instance[block.ID].Output(links[l].OuputBlockOutputIndex) == ConnectorID.FlowIfTrue)) ||
                    (type == false && Database.Instance[block.ID].Output(links[l].OuputBlockOutputIndex) == ConnectorID.FlowIfFalse))
                {
                    flowing.Add(blocks[links[l].InputBlockIndex]);
                }
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