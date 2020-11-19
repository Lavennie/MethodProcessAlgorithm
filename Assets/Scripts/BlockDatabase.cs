using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BlockDatabase : MonoBehaviour
{
    public static BlockDatabase Instance { get; private set; }
    public const float CIRCUIT_WIDTH = 3.0f;
    public const float CIRCUIT_DOT_RADIUS = 8.0f;

    [Header("Color set")]
    public Color lightColor = new Color(0.0f, 1.0f, 0.976f);
    public Color bgLight = new Color(0.337f, 0.337f, 0.337f);
    public Color bgNormal = new Color(0.118f, 0.118f, 0.118f);
    public Color bgDark = new Color(0.0f, 0.0f, 0.0f);
    public Color slataNormal = new Color(0.863f, 0.863f, 0.863f);
    public Color slateDark = new Color(0.686f, 0.686f, 0.686f);

    [Header("Block part prefabs")]
    public Block blockPrefab;
    public Input inputPrefab;
    public Output outputPrefab;
    public IntegratedParameter integratedParamPrefab;

    [Header("In scene references")]
    public Transform blockContainer;

    private Dictionary<BlockID, BlockData> data = new Dictionary<BlockID, BlockData>
    {
        { BlockID.Update, new BlockData("Update", "Control", new ConnectorID[0], new ConnectorID[1] { ConnectorID.FlowNormal })},
        { BlockID.Rotate, new BlockData("Rotate", "Transform", new ConnectorID[2] { ConnectorID.FlowNormal, ConnectorID.Float }, new ConnectorID[1] { ConnectorID.FlowNormal })},
        { BlockID.Move, new BlockData("Move", "Transform", new ConnectorID[2] { ConnectorID.FlowNormal, ConnectorID.Direction2 }, new ConnectorID[1] { ConnectorID.FlowNormal })},
    };
    private Canvas canvas;

    private void OnEnable()
    {
        Instance = this;
        InstantiateBlock(BlockID.Move);
    }

    public static Block InstantiateBlock(BlockID id)
    {
        Block block = Instantiate(Instance.blockPrefab, Instance.blockContainer);
        block.SetName(Instance.data[id].Name);
        block.SetGroup(Instance.data[id].Name);
        ((RectTransform)block.transform).sizeDelta += new Vector2(0,
            ((RectTransform)Instance.inputPrefab.transform).sizeDelta.y * Mathf.Max(Instance.data[id].InputCount, Instance.data[id].OutputCount));

        for (int i = 0; i < Instance.data[id].InputCount; i++)
        {
            Input input = Instantiate(Instance.inputPrefab, block.inputContainer);
            input.ApplyColor(Instance.lightColor);
            ((RectTransform)input.transform).anchoredPosition -= new Vector2(0, i * ((RectTransform)input.transform).sizeDelta.y);

            IntegratedParameter integratedParam = Instantiate(Instance.integratedParamPrefab, block.inputContainer);
            ((RectTransform)integratedParam.transform).anchoredPosition -= new Vector2(0, i * ((RectTransform)input.transform).sizeDelta.y);
            integratedParam.ApplyColor(Instance.bgLight, Instance.bgDark);
        }
        for (int i = 0; i < Instance.data[id].OutputCount; i++)
        {
            Output output = Instantiate(Instance.outputPrefab, block.outputContainer);
            output.ApplyColor(Instance.lightColor);
            ((RectTransform)output.transform).anchoredPosition = new Vector2(0, -i * ((RectTransform)output.transform).sizeDelta.y);
        }

        return null;
    }
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
public sealed class BlockData
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
