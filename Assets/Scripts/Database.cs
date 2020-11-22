using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Database : MonoBehaviour, IEnumerable<KeyValuePair<BlockID, BlockData>>
{
    public static Database Instance { get; private set; }
    public const float CIRCUIT_WIDTH = 3.0f;
    public const float CIRCUIT_DOT_RADIUS = 8.0f;

    [Header("Color set")]
    public ColorPalette pallete;

    [Header("Block part prefabs")]
    public Block blockPrefab;
    public Connector inputPrefab;
    public Connector outputPrefab;
    public IntegratedParameter integratedParamPrefab;
    public CodeBlockMenuEntry menuBlockEntryPrefab;
    public Link linkPrefab;

    [Header("In scene references")]
    public CodeWindow codeWindow;

    private Dictionary<BlockID, BlockData> data = new Dictionary<BlockID, BlockData>
    {
        { BlockID.Update, new BlockData("Update", "Control", new ConnectorID[0], new ConnectorID[1] { ConnectorID.FlowNormal })},
        { BlockID.Rotate, new BlockData("Rotate", "Transform", new ConnectorID[2] { ConnectorID.FlowNormal, ConnectorID.Float }, new ConnectorID[1] { ConnectorID.FlowNormal })},
        { BlockID.Move, new BlockData("Move", "Transform", new ConnectorID[2] { ConnectorID.FlowNormal, ConnectorID.Direction2 }, new ConnectorID[1] { ConnectorID.FlowNormal })},
    };

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator<KeyValuePair<BlockID, BlockData>> GetEnumerator()
    {
        foreach (var block in data)
        {
            yield return block;
        }
    }
    IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

    public BlockData this[BlockID id] { get { return data[id]; } }
}
[System.Serializable]
public class ColorPalette
{
    public Color lightColor = new Color(0.0f, 1.0f, 0.976f);
    public Color bgLight = new Color(0.337f, 0.337f, 0.337f);
    public Color bgNormal = new Color(0.118f, 0.118f, 0.118f);
    public Color bgDark = new Color(0.0f, 0.0f, 0.0f);
    public Color slataNormal = new Color(0.863f, 0.863f, 0.863f);
    public Color slateDark = new Color(0.686f, 0.686f, 0.686f);

    public static Color LightColor { get { return Database.Instance.pallete.lightColor; } }
    public static Color BgLight { get { return Database.Instance.pallete.bgLight; } }
    public static Color BgNormal { get { return Database.Instance.pallete.bgNormal; } }
    public static Color BgDark { get { return Database.Instance.pallete.bgDark; } }
    public static Color SlateNormal { get { return Database.Instance.pallete.slataNormal; } }
    public static Color SlateDark { get { return Database.Instance.pallete.slateDark; } }
}

public enum BlockID : uint
{
    Update,
    Rotate,
    Move,
    If,
    LineOfSight,
}
public enum ConnectorID : uint
{
    FlowNormal,
    FlowIfTrue,
    FlowIfFalse,
    Int,
    Float,
    Bool,
    Direction2,
}
public struct BlockData
{
    private string name;
    private string group;
    private ConnectorID[] inputs;
    private ConnectorID[] outputs;

    public BlockData(string name, string group, ConnectorID[] inputs, ConnectorID[] outputs) 
    {
        this.name = name;
        this.group = group;
        this.inputs = inputs;
        this.outputs = outputs;
    }

    public string Name { get { return name; } }
    public string Group { get { return group; } }
    public int InputCount { get { return inputs.Length; } }
    public int OutputCount { get { return outputs.Length; } }
    public ConnectorID Input(int index) { return inputs[index]; }
    public ConnectorID Outputs(int index) { return outputs[index]; }
}
